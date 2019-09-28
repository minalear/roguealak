using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Engine.UI.Controls
{
    public class Slider : Control
    {
        public Slider(Control parent, int x, int y, int width)
            : base(parent)
        {
            this.position = new Point(x, y);
            this.size = new Point(width, 1);
            this.scrollSize = width;

            this.sliderMode = SliderModes.Horizontal;
            this.setBarPosition();
        }
        public Slider(Control parent, int x, int y, SliderModes mode, int size)
            : base(parent)
        {
            this.position = new Point(x, y);

            if (mode == SliderModes.Horizontal)
            {
                this.size = new Point(size, 1);
                this.railToken = '═';
            }
            else
            {
                this.size = new Point(1, size);
                this.railToken = '║';
            }

            this.scrollSize = size;
            this.sliderMode = mode;
            this.setBarPosition();
        }

        public override void DrawStep()
        {
            this.drawRail();

            GraphicConsole.SetColors(this.barColor, this.fillColor);
            GraphicConsole.Put(this.barToken, this.barPosition.X, this.barPosition.Y);
            
            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (this.isMouseHover())
            {
                #region Scrolling
                int difference = InputManager.GetDistanceScrolled() / this.scrollSize;

                if (difference != 0)
                {
                    this.currentValue += difference;

                    if (this.currentValue < 0f)
                        this.currentValue = 0f;
                    else if (this.currentValue > 100f)
                        this.currentValue = 100f;

                    this.onValueChange();
                    InterfaceManager.DrawStep();
                }
                #endregion
            }
            
            base.Update(gameTime);
        }

        protected void onValueChange()
        {
            this.setBarPosition();

            if (this.ValueChanged != null)
                this.ValueChanged(this, this.currentValue / 100f);
        }

        private void setBarPosition()
        {
            if (this.sliderMode == SliderModes.Horizontal)
            {
                int x = (int)(this.Position.X + (this.currentValue / 100f * this.Size.X));

                this.barPosition.X = x;
                this.barPosition.Y = this.Position.Y;
            }
            else if (this.sliderMode == SliderModes.Vertical)
            {
                int y = (int)(this.Position.Y + (this.currentValue / 100f * this.Size.Y));

                this.barPosition.X = this.Position.X;
                this.barPosition.Y = y;
            }
        }
        private void drawRail()
        {
            GraphicConsole.SetColors(this.railColor, this.fillColor);
            if (this.sliderMode == SliderModes.Horizontal)
                DrawingUtilities.DrawLine(this.Position.X, this.Position.Y, this.Position.X + this.Size.X, this.Position.Y, this.railToken);
            else if (this.sliderMode == SliderModes.Vertical)
                DrawingUtilities.DrawLine(this.Position.X, this.Position.Y, this.Position.X, this.Position.Y + this.Size.Y, this.railToken);
        }

        private SliderModes sliderMode;
        private char railToken = '═';
        private char barToken = '▓';

        private Color barColor = Color.LightGray;
        private Color railColor = Color.DarkGray;
        private Color fillColor = Color.Black;
        private int scrollSize;

        private float currentValue = 0f;
        private Point barPosition;

        public SliderModes SliderMode { get { return this.sliderMode; } set { this.sliderMode = value; } }
        public char RailToken { get { return this.railToken; } set { this.railToken = value; } }
        public char BarToken { get { return this.barToken; } set { this.barToken = value; } }
        public Color BarColor { get { return this.barColor; } set { this.barColor = value; } }
        public Color RailColor { get { return this.railColor; } set { this.railColor = value; } }
        public Color FillColor { get { return this.fillColor; } set { this.fillColor = value; } }
        public float Value { get { return this.currentValue; } set { this.currentValue = value; this.onValueChange(); } }

        public enum SliderModes { Horizontal, Vertical }

        public event ValueChange ValueChanged;
        public delegate void ValueChange(object sender, float value);
    }
}
