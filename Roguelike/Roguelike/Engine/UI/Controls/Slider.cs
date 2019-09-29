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
            position = new Point(x, y);
            size = new Point(width, 1);
            scrollSize = width;

            sliderMode = SliderModes.Horizontal;
            setBarPosition();
        }
        public Slider(Control parent, int x, int y, SliderModes mode, int size)
            : base(parent)
        {
            position = new Point(x, y);

            if (mode == SliderModes.Horizontal)
            {
                size = new Point(size, 1);
                railToken = '═';
            }
            else
            {
                size = new Point(1, size);
                railToken = '║';
            }

            scrollSize = size;
            sliderMode = mode;
            setBarPosition();
        }

        public override void DrawStep()
        {
            drawRail();

            GraphicConsole.SetColors(barColor, fillColor);
            GraphicConsole.Put(barToken, barPosition.X, barPosition.Y);
            
            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (isMouseHover())
            {
                #region Scrolling
                int difference = InputManager.GetDistanceScrolled() / scrollSize;

                if (difference != 0)
                {
                    currentValue += difference;

                    if (currentValue < 0f)
                        currentValue = 0f;
                    else if (currentValue > 100f)
                        currentValue = 100f;

                    onValueChange();
                    InterfaceManager.DrawStep();
                }
                #endregion
            }
            
            base.Update(gameTime);
        }

        protected void onValueChange()
        {
            setBarPosition();

            if (ValueChanged != null)
                ValueChanged(this, currentValue / 100f);
        }

        private void setBarPosition()
        {
            if (sliderMode == SliderModes.Horizontal)
            {
                int x = (int)(Position.X + (currentValue / 100f * Size.X));

                barPosition.X = x;
                barPosition.Y = Position.Y;
            }
            else if (sliderMode == SliderModes.Vertical)
            {
                int y = (int)(Position.Y + (currentValue / 100f * Size.Y));

                barPosition.X = Position.X;
                barPosition.Y = y;
            }
        }
        private void drawRail()
        {
            GraphicConsole.SetColors(railColor, fillColor);
            if (sliderMode == SliderModes.Horizontal)
                DrawingUtilities.DrawLine(Position.X, Position.Y, Position.X + Size.X, Position.Y, railToken);
            else if (sliderMode == SliderModes.Vertical)
                DrawingUtilities.DrawLine(Position.X, Position.Y, Position.X, Position.Y + Size.Y, railToken);
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

        public SliderModes SliderMode { get { return sliderMode; } set { sliderMode = value; } }
        public char RailToken { get { return railToken; } set { railToken = value; } }
        public char BarToken { get { return barToken; } set { barToken = value; } }
        public Color BarColor { get { return barColor; } set { barColor = value; } }
        public Color RailColor { get { return railColor; } set { railColor = value; } }
        public Color FillColor { get { return fillColor; } set { fillColor = value; } }
        public float Value { get { return currentValue; } set { currentValue = value; onValueChange(); } }

        public enum SliderModes { Horizontal, Vertical }

        public event ValueChange ValueChanged;
        public delegate void ValueChange(object sender, float value);
    }
}
