using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Engine.UI.Controls
{
    public class InputBox : Control
    {
        public InputBox(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            this.text = string.Empty;

            this.Position = new Point(x, y);
            this.Size = new Point(width, height);

            if (height > 1)
                this.isMultiline = true;
        }
        public InputBox(Control parent, int x, int y, int width)
            : base(parent)
        {
            this.text = string.Empty;

            this.Position = new Point(x, y);
            this.Size = new Point(width, 1);
            this.isMultiline = false;
        }

        public override void DrawStep()
        {
            this.clearArea();

            GraphicConsole.SetColors(Color.Transparent, this.fillColor);
            DrawingUtilities.DrawRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

            if (this.text != string.Empty)
            {
                GraphicConsole.SetColors(this.textColor, this.fillColor);
                GraphicConsole.SetCursor(this.Position);
                GraphicConsole.Write(this.text);
            }

            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (this.hasFocus)
            {
                #region HasFocus Branch
                this.cursorCounter += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (this.cursorCounter >= cursorFlickerRate * 2)
                    this.cursorCounter = 0.0;
                if (this.cursorCounter > cursorFlickerRate)
                {
                    GraphicConsole.SetColors(this.textColor, this.fillColor);
                    GraphicConsole.SetCursor(this.Position.X + this.text.Length, this.Position.Y);
                    GraphicConsole.Write(this.cursor);
                }
                else
                {
                    GraphicConsole.SetColors(this.textColor, this.fillColor);
                    GraphicConsole.SetCursor(this.Position.X + this.text.Length, this.Position.Y);
                    GraphicConsole.Write(' ');
                }

                if (InputManager.InputStream.CanRead)
                {
                    #region Input Branch
                    char ch = (char)InputManager.InputStream.ReadByte();

                    if (ch == '\0') //NUL Terminator
                    {
                        if (!string.IsNullOrEmpty(this.text))
                        {
                            this.text = this.text.Remove(this.text.Length - 1);
                            this.DrawStep();
                        }
                    }
                    else if (ch == '\n' || ch == '\r')
                    {
                        this.onSubmit(null);
                        InputManager.InputStream.Flush();
                        InputManager.AcceptInput = false;

                        this.hasFocus = false;
                        this.DrawStep();
                    }
                    else if (ch == '\t')
                        return;
                    else
                    {
                        if (this.CharacterLimit == 0 || this.text.Length < this.characterLimit)
                        {
                            this.text += ch;
                            this.DrawStep();
                        }
                    }
                    #endregion
                }
                #endregion
            }

            if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
            {
                if (this.isMouseHover())
                {
                    this.hasFocus = true;
                    this.cursorCounter = 0.0;

                    this.DrawStep();
                    InputManager.AcceptInput = true;
                    InputManager.InputStream.Flush();
                }
                else
                {
                    this.hasFocus = false;
                    this.cursorCounter = 0.0;

                    this.DrawStep();
                    InputManager.AcceptInput = false;
                    InputManager.InputStream.Flush();
                }
            }
        }
        public void ForceSubmit(object sender)
        {
            this.onSubmit(sender);
        }
        public void Clear()
        {
            this.text = "";
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
            if (this.isMultiline)
            {
                this.text = TextUtilities.StripFormatting(text);
                this.text = TextUtilities.WordWrap(text, this.size.X);
            }
        }

        private string text;
        private Color textColor = Color.White;
        private Color fillColor = Color.Black;

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
        public string Text { get { return this.text; } set { this.text = value; } }
        public Color TextColor { get { return this.textColor; } set { this.textColor = value; } }
        public Color FillColor { get { return this.fillColor; } set { this.fillColor = value; } }
        public bool HasFocus { get { return this.hasFocus; } set { this.hasFocus = value; } }
        //public bool IsMultiline { get { return this.isMultiline; } set { this.isMultiline = value; } }
        public bool ShowCursor { get { return this.showCursor; } set { this.showCursor = value; } }
        public int CharacterLimit { get { return this.characterLimit; } set { this.characterLimit = value; } }
        #endregion
    }
}
