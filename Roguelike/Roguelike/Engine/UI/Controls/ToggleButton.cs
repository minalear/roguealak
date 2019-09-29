using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Engine.UI.Controls
{
    public class ToggleButton : Control
    {
        public ToggleButton(Control parent, string text, int x, int y)
            : base(parent)
        {
            setDefaults();

            text = text;
            position = new Point(x, y);
            size = new Point(text.Length + 2, 3);

            setTextPosition();
        }
        public ToggleButton(Control parent, string text, int x, int y, int width, int height)
            : base(parent)
        {
            setDefaults();

            text = text;
            position = new Point(x, y);
            size = new Point(width, height);

            setTextPosition();
        }
        
        public override void DrawStep()
        {
            clearArea();

            if (mode == ButtonModes.Hover)
            {
                //Fill Area
                GraphicConsole.SetColors(Color.Transparent, fillColorHover);
                DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

                //Write Text
                GraphicConsole.SetColors(textColorHover, fillColorHover);
                GraphicConsole.SetCursor(textPosition);
                GraphicConsole.Write(text);
            }
            else if (!enabled)
            {
                //Fill Area
                GraphicConsole.SetColors(Color.Transparent, fillColor);
                DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

                //Write Text
                GraphicConsole.SetColors(textColor, fillColor);
                GraphicConsole.SetCursor(textPosition);
                GraphicConsole.Write(text);
            }
            else if (enabled)
            {
                //Fill Area
                GraphicConsole.SetColors(Color.Transparent, fillColorPressed);
                DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

                //Write Text
                GraphicConsole.SetColors(textColorPressed, fillColorPressed);
                GraphicConsole.SetCursor(textPosition);
                GraphicConsole.Write(text);
            }

            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (isMouseHover())
            {
                mode = ButtonModes.Hover;

                if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                {
                    onButtonPress(MouseButtons.Left);

                    InterfaceManager.UpdateStep();
                    InterfaceManager.DrawStep();
                }
                else if (InputManager.MouseButtonIsDown(MouseButtons.Left))
                {
                    mode = ButtonModes.Pressed;
                    DrawStep();
                }
                else if (!wasHover())
                {
                    onButtonHover();
                    DrawStep();
                }
            }
            else if (wasHover())
            {
                if (enabled)
                    mode = ButtonModes.Pressed;
                else
                    mode = ButtonModes.Normal;

                InterfaceManager.DrawStep();
            }
        }

        //Event Methods
        protected void onButtonPress(MouseButtons button)
        {
            enabled = !enabled;

            if (Click != null)
                Click(this);

            if (enabled)
                mode = ButtonModes.Pressed;
            else
                mode = ButtonModes.Normal;
        }
        protected void onButtonHover()
        {
            if (Hover != null)
                Hover(this);
        }

        private void setDefaults()
        {
            textColor = DEFAULT_TEXT_COLOR;
            fillColor = DEFAULT_FILL_COLOR;

            textColorHover = DEFAULT_TEXT_HOVER_COLOR;
            fillColorHover = DEFAULT_FILL_HOVER_COLOR;

            textColorPressed = DEFAULT_TEXT_PRESSED_COLOR;
            fillColorPressed = DEFAULT_FILL_PRESSED_COLOR;
        }
        private void setTextPosition()
        {
            textPosition.X = Position.X + (Size.X / 2 - text.Length / 2);
            textPosition.Y = (Size.Y / 2) + Position.Y;
        }

        private string text;
        private Color textColor, fillColor;
        private Color textColorHover, fillColorHover;
        private Color textColorPressed, fillColorPressed;
        private ButtonModes mode;
        private Point textPosition;
        private bool enabled = false;

        private enum ButtonModes { Normal, Hover, Pressed }

        #region Properties
        public string Text { get { return text; } set { text = value; setTextPosition(); } }
        public Color TextColor { get { return textColor; } set { textColor = value; } }
        public Color FillColor { get { return fillColor; } set { fillColor = value; } }
        public Color TextColorHover { get { return textColorHover; } set { textColorHover = value; } }
        public Color FillColorHover { get { return fillColorHover; } set { fillColorHover = value; } }
        public Color TextColorPressed { get { return textColorPressed; } set { textColorPressed = value; } }
        public Color FillColorPressed { get { return fillColorPressed; } set { fillColorPressed = value; } }
        public bool Enabled { get { return enabled; } set { enabled = value; } }
        #endregion
        #region Constants
        private static Color DEFAULT_TEXT_COLOR = Color.White;
        private static Color DEFAULT_FILL_COLOR = Color.Black;

        private static Color DEFAULT_TEXT_HOVER_COLOR = Color.White;
        private static Color DEFAULT_FILL_HOVER_COLOR = new Color(170, 181, 187);

        private static Color DEFAULT_TEXT_PRESSED_COLOR = Color.Black;
        private static Color DEFAULT_FILL_PRESSED_COLOR = Color.White;
        #endregion

        public event ButtonToggled Click;
        public event ButtonHovered Hover;

        public delegate void ButtonToggled(object sender);
        public delegate void ButtonHovered(object sender);
    }
}
