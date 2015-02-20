using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Forms = System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Roguelike.Engine
{
    public static class InputManager
    {
        private static KeyboardStream _inputStream;
        private static bool _acceptInput;
        private static Forms.Form _windowForm;

        private static KeyboardState _priorKeyboardState;
        private static KeyboardState _currentKeyboardState;
        private static MouseState _priorMouseState;
        private static MouseState _currentMouseState;
        private static IDictionary<Keys, TimeSpan> _keyHeldTimes;
        private static IDictionary<MouseButtons, TimeSpan> _mouseButtonHeldTimes;
        private static IDictionary<MouseButtons, Func<MouseState, ButtonState>> _mouseButtonMaps;

        public static void Initialize()
        {
            _mouseButtonMaps = new Dictionary<MouseButtons, Func<MouseState, ButtonState>>
                {
                    { MouseButtons.Left, s => s.LeftButton },
                    { MouseButtons.Right, s => s.RightButton },
                    { MouseButtons.Middle, s => s.MiddleButton },
                    { MouseButtons.Extra1, s => s.XButton1 },
                    { MouseButtons.Extra2, s => s.XButton2 }
                };
            _keyHeldTimes = new Dictionary<Keys, TimeSpan>();
            foreach (var key in Enum.GetValues(typeof(Keys)))
            {
                _keyHeldTimes.Add((Keys)key, TimeSpan.Zero);
            }
            _mouseButtonHeldTimes = new Dictionary<MouseButtons, TimeSpan>();
            foreach (var mouseButton in Enum.GetValues(typeof(MouseButtons)))
            {
                _mouseButtonHeldTimes.Add((MouseButtons)mouseButton, TimeSpan.Zero);
            }

            _priorKeyboardState = _currentKeyboardState = Keyboard.GetState();
            _priorMouseState = _currentMouseState = Mouse.GetState();

            _inputStream = new KeyboardStream();

            AcceptInput = false;

            var control = Forms.Control.FromHandle(Program.Game.Window.Handle);
            _windowForm = control.FindForm();
        }
        public static void ResetTimeHeld(Keys key)
        {
            if (_keyHeldTimes.ContainsKey(key))
                _keyHeldTimes[key] = TimeSpan.Zero;
        }
        public static bool KeyWasPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _priorKeyboardState.IsKeyUp(key);
        }
        public static bool KeyWasPressedFor(Keys key, TimeSpan timeSpan)
        {
            return GetElapsedHeldTime(key).CompareTo(timeSpan) >= 0;
        }
        public static bool KeyWasPressedWithModifiers(Keys key, params Keys[] modifiers)
        {
            return KeyWasPressed(key) && modifiers.All(k => _currentKeyboardState.IsKeyDown(k));
        }
        public static bool KeyWasReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) && _priorKeyboardState.IsKeyDown(key);
        }
        public static TimeSpan GetElapsedHeldTime(Keys key)
        {
            return _keyHeldTimes[key];
        }
        public static TimeSpan GetElapsedHeldTime(MouseButtons mouseButton)
        {
            return _mouseButtonHeldTimes[mouseButton];
        }
        public static bool MouseButtonIsDown(MouseButtons button)
        {
            return _mouseButtonMaps[button](_currentMouseState) == ButtonState.Pressed;
        }
        public static bool MouseButtonIsUp(MouseButtons button)
        {
            return _mouseButtonMaps[button](_currentMouseState) == ButtonState.Released;
        }
        public static bool MouseButtonWasClicked(MouseButtons button)
        {
            return
            _mouseButtonMaps[button](_currentMouseState) == ButtonState.Released &&
            _mouseButtonMaps[button](_priorMouseState) == ButtonState.Pressed;
        }
        public static bool ButtonWasClickedWithKeyModifiers(MouseButtons button, params Keys[] modifierKeys)
        {
            return MouseButtonWasClicked(button) && modifierKeys.All(k => _currentKeyboardState.IsKeyDown(k));
        }
        public static bool ButtonWasReleased(MouseButtons button)
        {
            return
            _mouseButtonMaps[button](_currentMouseState) == ButtonState.Released &&
            _mouseButtonMaps[button](_priorMouseState) == ButtonState.Pressed;
        }
        public static int GetDistanceScrolled()
        {
            return _currentMouseState.ScrollWheelValue - _priorMouseState.ScrollWheelValue;
        }
        public static bool MouseIsScrollingUp()
        {
            return _currentMouseState.ScrollWheelValue > _priorMouseState.ScrollWheelValue;
        }
        public static bool MouseIsScrollingDown()
        {
            return _currentMouseState.ScrollWheelValue < _priorMouseState.ScrollWheelValue;
        }
        public static void Update(GameTime gameTime)
        {
            if (_windowForm.Focused)
            {
                // Keyboard
                _priorKeyboardState = _currentKeyboardState;
                _currentKeyboardState = Keyboard.GetState();
                foreach (var key in Enum.GetValues(typeof(Keys)))
                {
                    if (_currentKeyboardState[(Keys)key] == KeyState.Down)
                        _keyHeldTimes[(Keys)key] = _keyHeldTimes[(Keys)key] + gameTime.ElapsedGameTime;
                    else
                        _keyHeldTimes[(Keys)key] = TimeSpan.Zero;
                }
                // Mouse
                _priorMouseState = _currentMouseState;
                _currentMouseState = Mouse.GetState();
                foreach (var mouseButton in Enum.GetValues(typeof(MouseButtons)))
                {
                    if (_mouseButtonMaps[(MouseButtons)mouseButton](_currentMouseState) == ButtonState.Pressed)
                        _mouseButtonHeldTimes[(MouseButtons)mouseButton] += gameTime.ElapsedGameTime;
                    else
                        _mouseButtonHeldTimes[(MouseButtons)mouseButton] = TimeSpan.Zero;
                }

                if (_acceptInput)
                {
                    Keys[] pressedKeys = _currentKeyboardState.GetPressedKeys();
                    for (int i = 0; i < pressedKeys.Length; i++)
                    {
                        if (_priorKeyboardState.IsKeyUp(pressedKeys[i]))
                        {
                            char ch = getCharFromKey(pressedKeys[i]);
                            if (ch != '\t')
                                _inputStream.WriteByte((byte)ch);
                        }
                    }
                }
            }
        }
        public static Point GetCurrentMousePosition()
        {
            return new Point(_currentMouseState.X, _currentMouseState.Y);
        }
        public static Point GetPriorMousePosition()
        {
            return new Point(_priorMouseState.X, _priorMouseState.Y);
        }

        private static char getCharFromKey(Keys key)
        {
            bool shift = _currentKeyboardState.IsKeyDown(Keys.LeftShift) || _priorKeyboardState.IsKeyDown(Keys.RightShift);

            bool caps = false;
            if (shift || Forms.Control.IsKeyLocked(Forms.Keys.CapsLock))
                caps = true;

            switch (key)
            {
                //Alphabet keys
                case Keys.A: if (caps) { return 'A'; } else { return 'a'; }
                case Keys.B: if (caps) { return 'B'; } else { return 'b'; }
                case Keys.C: if (caps) { return 'C'; } else { return 'c'; }
                case Keys.D: if (caps) { return 'D'; } else { return 'd'; }
                case Keys.E: if (caps) { return 'E'; } else { return 'e'; }
                case Keys.F: if (caps) { return 'F'; } else { return 'f'; }
                case Keys.G: if (caps) { return 'G'; } else { return 'g'; }
                case Keys.H: if (caps) { return 'H'; } else { return 'h'; }
                case Keys.I: if (caps) { return 'I'; } else { return 'i'; }
                case Keys.J: if (caps) { return 'J'; } else { return 'j'; }
                case Keys.K: if (caps) { return 'K'; } else { return 'k'; }
                case Keys.L: if (caps) { return 'L'; } else { return 'l'; }
                case Keys.M: if (caps) { return 'M'; } else { return 'm'; }
                case Keys.N: if (caps) { return 'N'; } else { return 'n'; }
                case Keys.O: if (caps) { return 'O'; } else { return 'o'; }
                case Keys.P: if (caps) { return 'P'; } else { return 'p'; }
                case Keys.Q: if (caps) { return 'Q'; } else { return 'q'; }
                case Keys.R: if (caps) { return 'R'; } else { return 'r'; }
                case Keys.S: if (caps) { return 'S'; } else { return 's'; }
                case Keys.T: if (caps) { return 'T'; } else { return 't'; }
                case Keys.U: if (caps) { return 'U'; } else { return 'u'; }
                case Keys.V: if (caps) { return 'V'; } else { return 'v'; }
                case Keys.W: if (caps) { return 'W'; } else { return 'w'; }
                case Keys.X: if (caps) { return 'X'; } else { return 'x'; }
                case Keys.Y: if (caps) { return 'Y'; } else { return 'y'; }
                case Keys.Z: if (caps) { return 'Z'; } else { return 'z'; }

                //Decimal keys
                case Keys.D0: if (shift) { return ')'; } else { return '0'; }
                case Keys.D1: if (shift) { return '!'; } else { return '1'; }
                case Keys.D2: if (shift) { return '@'; } else { return '2'; }
                case Keys.D3: if (shift) { return '#'; } else { return '3'; }
                case Keys.D4: if (shift) { return '$'; } else { return '4'; }
                case Keys.D5: if (shift) { return '%'; } else { return '5'; }
                case Keys.D6: if (shift) { return '^'; } else { return '6'; }
                case Keys.D7: if (shift) { return '&'; } else { return '7'; }
                case Keys.D8: if (shift) { return '*'; } else { return '8'; }
                case Keys.D9: if (shift) { return '('; } else { return '9'; }

                //Decimal numpad keys
                case Keys.NumPad0: return '0';
                case Keys.NumPad1: return '1';
                case Keys.NumPad2: return '2';
                case Keys.NumPad3: return '3';
                case Keys.NumPad4: return '4';
                case Keys.NumPad5: return '5';
                case Keys.NumPad6: return '6';
                case Keys.NumPad7: return '7';
                case Keys.NumPad8: return '8';
                case Keys.NumPad9: return '9';

                //Other keys
                case Keys.Multiply: return '*';
                case Keys.Divide: return '/';
                case Keys.Add: return '+';
                case Keys.Subtract: return '-';
                case Keys.Decimal: return '.';

                //Special keys
                case Keys.OemTilde: if (shift) { return '~'; } else { return '`'; }
                case Keys.OemSemicolon: if (shift) { return ':'; } else { return ';'; }
                case Keys.OemQuotes: if (shift) { return '"'; } else { return '\''; }
                case Keys.OemQuestion: if (shift) { return '?'; } else { return '/'; }
                case Keys.OemPlus: if (shift) { return '+'; } else { return '='; }
                case Keys.OemPipe: if (shift) { return '|'; } else { return '\\'; }
                case Keys.OemPeriod: if (shift) { return '>'; } else { return '.'; }
                case Keys.OemOpenBrackets: if (shift) { return '{'; } else { return '['; }
                case Keys.OemCloseBrackets: if (shift) { return '}'; } else { return ']'; }
                case Keys.OemMinus: if (shift) { return '_'; } else { return '-'; }
                case Keys.OemComma: if (shift) { return '<'; } else { return ','; }
                case Keys.Space: return ' ';
                case Keys.Enter: return '\n';
                case Keys.Back: return '\0';
            }

            return '\t';
        }

        public static KeyboardState CurrentKeyboardState { get { return _currentKeyboardState; } }
        public static KeyboardState PriorKeyboardState { get { return _priorKeyboardState; } }
        public static MouseState CurrentMouseState { get { return _currentMouseState; } }
        public static MouseState PriorMouseState { get { return _priorMouseState; } }
        public static KeyboardStream InputStream { get { return _inputStream; } }
        public static bool AcceptInput { get { return _acceptInput; } set { _acceptInput = value; } }
    }
    public enum MouseButtons
    {
        Left,
        Middle,
        Right,
        Extra1,
        Extra2
    }

    public class KeyboardStream : Stream
    {
        private string _inlineStream = "";

        public override bool CanRead
        {
            get
            {
                if (_inlineStream.Length > 0)
                    return true;
                else
                    return false;
            }
        }
        public override bool CanSeek
        {
            get { return false; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }
        public override long Length
        {
            get { return 0; }
        }
        public override long Position
        {
            get
            {
                return 0;
            }
            set
            {
                return;
            }
        }

        public override void Flush()
        {
            _inlineStream = string.Empty;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[i] = (byte)_inlineStream[i];
            }

            return 0;
        }
        public override int ReadByte()
        {
            char ch = _inlineStream[0];

            string buffer = "";
            for (int i = 1; i < _inlineStream.Length; i++)
                buffer += _inlineStream[i];
            _inlineStream = buffer;

            return ch;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            foreach (byte b in buffer)
                this.WriteByte(b);
        }
        public override void WriteByte(byte value)
        {
            _inlineStream += (char)value;
        }
    }
}
