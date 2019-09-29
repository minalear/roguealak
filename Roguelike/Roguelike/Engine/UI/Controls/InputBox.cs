using System;
using OpenTK.Graphics;

namespace Roguelike.Engine.UI.Controls
{
    public class InputBox : Control
    {
        public InputBox(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            text = string.Empty;

            Position = new Point(x, y);
            Size = new Point(width, height);

            if (height > 1)
                isMultiline = true;
        }
        public InputBox(Control parent, int x, int y, int width)
            : base(parent)
        {
            text = string.Empty;

            Position = new Point(x, y);
            Size = new Point(width, 1);
            isMultiline = false;
        }

        public override void DrawStep()
        {
            clearArea();

            GraphicConsole.SetColors(Color4.Transparent, fillColor);
            DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

            if (text != string.Empty)
            {
                GraphicConsole.SetColors(textColor, fillColor);
                GraphicConsole.SetCursor(Position);
                GraphicConsole.Write(text);
            }

            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (hasFocus)
            {
                #region HasFocus Branch
                cursorCounter += gameTime.ElapsedTime.TotalMilliseconds;

                if (cursorCounter >= cursorFlickerRate * 2)
                    cursorCounter = 0.0;
                if (cursorCounter > cursorFlickerRate)
                {
                    GraphicConsole.SetColors(textColor, fillColor);
                    GraphicConsole.SetCursor(Position.X + text.Length, Position.Y);
                    GraphicConsole.Write(cursor);
                }
                else
                {
                    GraphicConsole.SetColors(textColor, fillColor);
                    GraphicConsole.SetCursor(Position.X + text.Length, Position.Y);
                    GraphicConsole.Write(' ');
                }

                if (InputManager.InputStream.CanRead)
                {
                    #region Input Branch
                    char ch = (char)InputManager.InputStream.ReadByte();

                    if (ch == '\0') //NUL Terminator
                    {
                        if (!string.IsNullOrEmpty(text))
                        {
                            text = text.Remove(text.Length - 1);
                            DrawStep();
                        }
                    }
                    else if (ch == '\n' || ch == '\r')
                    {
                        onSubmit(null);
                        InputManager.InputStream.Flush();
                        InputManager.AcceptInput = false;

                        hasFocus = false;
                        DrawStep();
                    }
                    else if (ch == '\t')
                        return;
                    else
                    {
                        if (CharacterLimit == 0 || text.Length < characterLimit)
                        {
                            text += ch;
                            DrawStep();
                        }
                    }
                    #endregion
                }
                #endregion
            }

            if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
            {
                if (isMouseHover())
                {
                    hasFocus = true;
                    cursorCounter = 0.0;

                    DrawStep();
                    InputManager.AcceptInput = true;
                    InputManager.InputStream.Flush();
                }
                else
                {
                    hasFocus = false;
                    cursorCounter = 0.0;

                    DrawStep();
                    InputManager.AcceptInput = false;
                    InputManager.InputStream.Flush();
                }
            }
        }
        public void ForceSubmit(object sender)
        {
            onSubmit(sender);
        }
        public void Clear()
        {
            text = "";
        }

        protected void onSubmit(object sender)
        {
            if (Submitted != null)
                Submitted(sender);

            InterfaceManager.UpdateStep();
            InterfaceManager.DrawStep();
        }
        private void wrapText()
        {
            if (isMultiline)
            {
                text = TextUtilities.StripFormatting(text);
                text = TextUtilities.WordWrap(text, size.X);
            }
        }

        private string text;
        private Color4 textColor = Color4.White;
        private Color4 fillColor = Color4.Black;

        private bool hasFocus = false;
        private bool isMultiline = false;
        private bool showCursor = true;

        private char cursor = '█';
        private double cursorCounter = 0.0;
        private double cursorFlickerRate = 600.0;
        private int characterLimit = 0;
        //private int line = 0; //for multiline cursor placement

        public event InputBoxSubmit Submitted;
        public delegate void InputBoxSubmit(object sender);

        #region Properties
        public string Text { get { return text; } set { text = value; } }
        public Color4 TextColor { get { return textColor; } set { textColor = value; } }
        public Color4 FillColor { get { return fillColor; } set { fillColor = value; } }
        public bool HasFocus { get { return hasFocus; } set { hasFocus = value; } }
        //public bool IsMultiline { get { return isMultiline; } set { isMultiline = value; } }
        public bool ShowCursor { get { return showCursor; } set { showCursor = value; } }
        public int CharacterLimit { get { return characterLimit; } set { characterLimit = value; } }
        #endregion
    }
}
