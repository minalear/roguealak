using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Engine.UI.Controls
{
    public class Title : Control
    {
        public Title(Control parent, string text, int x, int y)
            : base(parent)
        {
            this.text = text;
            this.Position = new Point(x, y);
            this.Size = new Point(text.Length, 1);

            this.textAlignMode = TextAlignModes.Left;
        }
        public Title(Control parent, string text, int x, int y, TextAlignModes alignMode)
            : base(parent)
        {
            this.text = text;
            this.Position = new Point(x, y);
            this.Size = new Point(text.Length, 1);

            this.textAlignMode = alignMode;
        }

        public override void DrawStep()
        {
            GraphicConsole.SetColors(this.textColor, this.fillColor);

            if (this.textAlignMode == TextAlignModes.Center)
            {
                int x = (int)(this.Position.X - this.text.Length / 2);

                GraphicConsole.SetCursor(x, this.Position.Y);
                GraphicConsole.Write(this.text);
            }
            else if (this.textAlignMode == TextAlignModes.Left)
            {
                GraphicConsole.SetCursor(this.Position.X, this.Position.Y);
                GraphicConsole.Write(this.text);
            }
            else if (this.textAlignMode == TextAlignModes.Right)
            {
                int x = (int)(this.Position.X - this.text.Length);
                
                GraphicConsole.SetCursor(x, this.Position.Y);
                GraphicConsole.Write(this.text);
            }

            base.DrawStep();
        }

        private string text;
        private TextAlignModes textAlignMode = TextAlignModes.Center;
        private Color textColor = Color.White;
        private Color fillColor = Color.Black;

        #region Properties
        public string Text { get { return this.text; } set { this.text = value; } }
        public TextAlignModes AlignMode { get { return this.textAlignMode; } set { this.textAlignMode = value; } }
        public Color TextColor { get { return this.textColor; } set { this.textColor = value; } }
        public Color FillColor { get { return this.fillColor; } set { this.fillColor = value; } } 
        #endregion

        public enum TextAlignModes { Center, Left, Right }
    }
}
