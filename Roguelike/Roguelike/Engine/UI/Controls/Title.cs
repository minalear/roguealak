using System;
using OpenTK.Graphics;

namespace Roguelike.Engine.UI.Controls
{
    public class Title : Control
    {
        public Title(Control parent, string text, int x, int y)
            : base(parent)
        {
            this.text = text;
            Position = new Point(x, y);
            Size = new Point(text.Length, 1);

            textAlignMode = TextAlignModes.Left;
        }
        public Title(Control parent, string text, int x, int y, TextAlignModes alignMode)
            : base(parent)
        {
            this.text = text;
            Position = new Point(x, y);
            Size = new Point(text.Length, 1);

            textAlignMode = alignMode;
        }

        public override void DrawStep()
        {
            GraphicConsole.SetColors(textColor, fillColor);

            if (textAlignMode == TextAlignModes.Center)
            {
                int x = (int)(Position.X - text.Length / 2);

                GraphicConsole.SetCursor(x, Position.Y);
                GraphicConsole.Write(text);
            }
            else if (textAlignMode == TextAlignModes.Left)
            {
                GraphicConsole.SetCursor(Position.X, Position.Y);
                GraphicConsole.Write(text);
            }
            else if (textAlignMode == TextAlignModes.Right)
            {
                int x = (int)(Position.X - text.Length);
                
                GraphicConsole.SetCursor(x, Position.Y);
                GraphicConsole.Write(text);
            }

            base.DrawStep();
        }

        private string text;
        private TextAlignModes textAlignMode = TextAlignModes.Center;
        private Color4 textColor = Color4.White;
        private Color4 fillColor = Color4.Black;

        #region Properties
        public string Text { get { return text; } set { text = value; } }
        public TextAlignModes AlignMode { get { return textAlignMode; } set { textAlignMode = value; } }
        public Color4 TextColor { get { return textColor; } set { textColor = value; } }
        public Color4 FillColor { get { return fillColor; } set { fillColor = value; } } 
        #endregion

        public enum TextAlignModes { Center, Left, Right }
    }
}
