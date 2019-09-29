using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using OpenTK.Input;

namespace Roguelike.Engine
{
    public static class InputManager
    {
        private static KeyboardStream _inputStream;
        private static bool _acceptInput;
        private static Game _gameWindow;

        private static KeyboardState _priorKeyboardState;
        private static KeyboardState _currentKeyboardState;
        private static MouseState _priorMouseState;
        private static MouseState _currentMouseState;
        private static IDictionary<Key, TimeSpan> _keyHeldTimes;
        private static IDictionary<MouseButtons, TimeSpan> _mouseButtonHeldTimes;
        private static IDictionary<MouseButtons, Func<MouseState, ButtonState>> _mouseButtonMaps;

        public static void Initialize(Game game)
        {
            _mouseButtonMaps = new Dictionary<MouseButtons, Func<MouseState, ButtonState>>
                {
                    { MouseButtons.Left, s => s.LeftButton },
                    { MouseButtons.Right, s => s.RightButton },
                    { MouseButtons.Middle, s => s.MiddleButton },
                    { MouseButtons.Extra1, s => s.XButton1 },
                    { MouseButtons.Extra2, s => s.XButton2 }
                };
            _keyHeldTimes = new Dictionary<Key, TimeSpan>();
            foreach (var key in Enum.GetValues(typeof(Key)))
            {
                _keyHeldTimes.Add((Key)key, TimeSpan.Zero);
            }
            _mouseButtonHeldTimes = new Dictionary<MouseButtons, TimeSpan>();
            foreach (var mouseButton in Enum.GetValues(typeof(MouseButtons)))
            {
                _mouseButtonHeldTimes.Add((MouseButtons)mouseButton, TimeSpan.Zero);
            }

            _priorKeyboardState = _currentKeyboardState = Keyboard.GetState();
            _priorMouseState = _currentMouseState = Mouse.GetState();

            game.Window.KeyPress += Window_KeyPress;

            _inputStream = new KeyboardStream();

            AcceptInput = false;

            _gameWindow = game;
        }

        public static void ResetTimeHeld(Key key)
        {
            if (_keyHeldTimes.ContainsKey(key))
                _keyHeldTimes[key] = TimeSpan.Zero;
        }
        public static bool KeyWasPressed(Key key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _priorKeyboardState.IsKeyUp(key);
        }
        public static bool KeyWasPressedFor(Key key, TimeSpan timeSpan)
        {
            return GetElapsedHeldTime(key).CompareTo(timeSpan) >= 0;
        }
        public static bool KeyWasPressedWithModifiers(Key key, params Key[] modifiers)
        {
            return KeyWasPressed(key) && modifiers.All(k => _currentKeyboardState.IsKeyDown(k));
        }
        public static bool KeyWasReleased(Key key)
        {
            return _currentKeyboardState.IsKeyUp(key) && _priorKeyboardState.IsKeyDown(key);
        }
        public static TimeSpan GetElapsedHeldTime(Key key)
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
        public static bool ButtonWasClickedWithKeyModifiers(MouseButtons button, params Key[] modifierKeys)
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
            if (_gameWindow.Window.Focused)
            {
                // Keyboard
                _priorKeyboardState = _currentKeyboardState;
                _currentKeyboardState = Keyboard.GetState();
                foreach (var key in Enum.GetValues(typeof(Key)))
                {
                    if (_currentKeyboardState[(Key)key])
                        _keyHeldTimes[(Key)key] = _keyHeldTimes[(Key)key] + gameTime.ElapsedTime;
                    else
                        _keyHeldTimes[(Key)key] = TimeSpan.Zero;
                }
                // Mouse
                _priorMouseState = _currentMouseState;
                _currentMouseState = Mouse.GetState();
                foreach (var mouseButton in Enum.GetValues(typeof(MouseButtons)))
                {
                    if (_mouseButtonMaps[(MouseButtons)mouseButton](_currentMouseState) == ButtonState.Pressed)
                        _mouseButtonHeldTimes[(MouseButtons)mouseButton] += gameTime.ElapsedTime;
                    else
                        _mouseButtonHeldTimes[(MouseButtons)mouseButton] = TimeSpan.Zero;
                }

                /* Functionality moved to Window_KeyPress */
                /* if (_acceptInput)
                {
                    var pressedKeys = _currentKeyboardState.GetPressedKeys();
                    for (int i = 0; i < pressedKeys.Length; i++)
                    {
                        if (_priorKeyboardState.IsKeyUp(pressedKeys[i]))
                        {
                            char ch = getCharFromKey(pressedKeys[i]);
                            if (ch != '\t')
                                _inputStream.WriteByte((byte)ch);
                        }
                    }
                }*/
            }
        }
        private static void Window_KeyPress(object sender, OpenTK.KeyPressEventArgs e)
        {
            if (_acceptInput)
            {
                if (e.KeyChar != '\t')
                {
                    _inputStream.WriteByte((byte)e.KeyChar);
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

        private static char getCharFromKey(Key key)
        {
            bool shift = _currentKeyboardState.IsKeyDown(Key.LShift) || _priorKeyboardState.IsKeyDown(Key.RShift);

            bool caps = false;
            /*if (shift || Forms.Control.IsKeyLocked(Forms.Keys.CapsLock))
                caps = true;*/
            // TODO: Fix caps lock

            switch (key)
            {
                //Alphabet keys
                case Key.A: if (caps) { return 'A'; } else { return 'a'; }
                case Key.B: if (caps) { return 'B'; } else { return 'b'; }
                case Key.C: if (caps) { return 'C'; } else { return 'c'; }
                case Key.D: if (caps) { return 'D'; } else { return 'd'; }
                case Key.E: if (caps) { return 'E'; } else { return 'e'; }
                case Key.F: if (caps) { return 'F'; } else { return 'f'; }
                case Key.G: if (caps) { return 'G'; } else { return 'g'; }
                case Key.H: if (caps) { return 'H'; } else { return 'h'; }
                case Key.I: if (caps) { return 'I'; } else { return 'i'; }
                case Key.J: if (caps) { return 'J'; } else { return 'j'; }
                case Key.K: if (caps) { return 'K'; } else { return 'k'; }
                case Key.L: if (caps) { return 'L'; } else { return 'l'; }
                case Key.M: if (caps) { return 'M'; } else { return 'm'; }
                case Key.N: if (caps) { return 'N'; } else { return 'n'; }
                case Key.O: if (caps) { return 'O'; } else { return 'o'; }
                case Key.P: if (caps) { return 'P'; } else { return 'p'; }
                case Key.Q: if (caps) { return 'Q'; } else { return 'q'; }
                case Key.R: if (caps) { return 'R'; } else { return 'r'; }
                case Key.S: if (caps) { return 'S'; } else { return 's'; }
                case Key.T: if (caps) { return 'T'; } else { return 't'; }
                case Key.U: if (caps) { return 'U'; } else { return 'u'; }
                case Key.V: if (caps) { return 'V'; } else { return 'v'; }
                case Key.W: if (caps) { return 'W'; } else { return 'w'; }
                case Key.X: if (caps) { return 'X'; } else { return 'x'; }
                case Key.Y: if (caps) { return 'Y'; } else { return 'y'; }
                case Key.Z: if (caps) { return 'Z'; } else { return 'z'; }

                //Decimal keys
                case Key.Number0: if (shift) { return ')'; } else { return '0'; }
                case Key.Number1: if (shift) { return '!'; } else { return '1'; }
                case Key.Number2: if (shift) { return '@'; } else { return '2'; }
                case Key.Number3: if (shift) { return '#'; } else { return '3'; }
                case Key.Number4: if (shift) { return '$'; } else { return '4'; }
                case Key.Number5: if (shift) { return '%'; } else { return '5'; }
                case Key.Number6: if (shift) { return '^'; } else { return '6'; }
                case Key.Number7: if (shift) { return '&'; } else { return '7'; }
                case Key.Number8: if (shift) { return '*'; } else { return '8'; }
                case Key.Number9: if (shift) { return '('; } else { return '9'; }

                //Decimal numpad keys
                case Key.Keypad0: return '0';
                case Key.Keypad1: return '1';
                case Key.Keypad2: return '2';
                case Key.Keypad3: return '3';
                case Key.Keypad4: return '4';
                case Key.Keypad5: return '5';
                case Key.Keypad6: return '6';
                case Key.Keypad7: return '7';
                case Key.Keypad8: return '8';
                case Key.Keypad9: return '9';

                //Other keys
                case Key.KeypadMultiply: return '*';
                case Key.KeypadDivide: return '/';
                case Key.KeypadAdd: return '+';
                case Key.KeypadSubtract: return '-';
                case Key.KeypadDecimal: return '.';

                //Special keys
                case Key.Tilde: if (shift) { return '~'; } else { return '`'; }
                case Key.Semicolon: if (shift) { return ':'; } else { return ';'; }
                case Key.Quote: if (shift) { return '"'; } else { return '\''; }
                case Key.Slash: if (shift) { return '?'; } else { return '/'; }
                case Key.Plus: if (shift) { return '+'; } else { return '='; }
                case Key.BackSlash: if (shift) { return '|'; } else { return '\\'; }
                case Key.Period: if (shift) { return '>'; } else { return '.'; }
                case Key.BracketLeft: if (shift) { return '{'; } else { return '['; }
                case Key.BracketRight: if (shift) { return '}'; } else { return ']'; }
                case Key.Minus: if (shift) { return '_'; } else { return '-'; }
                case Key.Comma: if (shift) { return '<'; } else { return ','; }
                case Key.Space: return ' ';
                case Key.Enter: return '\n';
                case Key.Back: return '\0';
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
                WriteByte(b);
        }
        public override void WriteByte(byte value)
        {
            _inlineStream += (char)value;
        }
    }
}
