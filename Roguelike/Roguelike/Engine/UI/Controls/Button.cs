using System;
using OpenTK.Graphics;
using Roguelike.Engine.Console;

namespace Roguelike.Engine.UI.Controls
{
    public class Button : Control
    {
        public Button(Control parent, string text, int x, int y)
            : base(parent)
        {
            setDefaults();

            this.text = text;
            position = new Point(x, y);
            size = new Point(text.Length + 2, 3);

            setTextPosition();
        }
        public Button(Control parent, string text, int x, int y, int width, int height)
            : base(parent)
        {
            setDefaults();

            this.text = text;
            position = new Point(x, y);
            size = new Point(width, height);

            setTextPosition();
        }
        
        public override void DrawStep()
        {
            clearArea();

            if (mode == ButtonModes.Normal)
            {
                //Fill Area
                GraphicConsole.Instance.SetColors(Color4.Transparent, fillColor);
                DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

                //Write Text
                GraphicConsole.Instance.SetColors(textColor, fillColor);
                GraphicConsole.Instance.SetCursor(textPosition);
                GraphicConsole.Instance.Write(text);
            }
            else if (mode == ButtonModes.Hover)
            {
                //Fill Area
                GraphicConsole.Instance.SetColors(Color4.Transparent, fillColorHover);
                DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

                //Write Text
                GraphicConsole.Instance.SetColors(textColorHover, fillColorHover);
                GraphicConsole.Instance.SetCursor(textPosition);
                GraphicConsole.Instance.Write(text);
            }
            else if (mode == ButtonModes.Pressed)
            {
                //Fill Area
                GraphicConsole.Instance.SetColors(Color4.Transparent, fillColorPressed);
                DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

                //Write Text
                GraphicConsole.Instance.SetColors(textColorPressed, fillColorPressed);
                GraphicConsole.Instance.SetCursor(textPosition);
                GraphicConsole.Instance.Write(text);
            }

            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (isMouseHover())
            {
                #region IsMouseHover Branch
                mode = ButtonModes.Hover;

                if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                {
                    onButtonPress(MouseButtons.Left);

                    InterfaceManager.UpdateStep();
                    InterfaceManager.DrawStep();
                }
                else if (InputManager.MouseButtonWasClicked(MouseButtons.Middle))
                {
                    onButtonPress(MouseButtons.Middle);

                    InterfaceManager.UpdateStep();
                    InterfaceManager.DrawStep();
                }
                else if (InputManager.MouseButtonWasClicked(MouseButtons.Right))
                {
                    onButtonPress(MouseButtons.Right);

                    InterfaceManager.UpdateStep();
                    InterfaceManager.DrawStep();
                }
                else if (InputManager.MouseButtonIsDown(MouseButtons.Left))
                {
                    mode = ButtonModes.Pressed;
                    DrawStep();
                }
                else if (InputManager.MouseButtonIsDown(MouseButtons.Middle))
                {
                    mode = ButtonModes.Pressed;
                    DrawStep();
                }
                else if (InputManager.MouseButtonIsDown(MouseButtons.Right))
                {
                    mode = ButtonModes.Pressed;
                    DrawStep();
                }
                else if (!wasHover())
                {
                    onButtonHover();
                    DrawStep();
                }
                #endregion
            }
            else if (wasHover())
            {
                mode = ButtonModes.Normal;
                DrawStep();
            }
            else
            {
                mode = ButtonModes.Normal;
            }

            if (InputManager.KeyWasReleased(KeyShortcut))
                onButtonPress(MouseButtons.Left);
        }

        //Event Methods
        protected void onButtonPress(MouseButtons button)
        {
            if (Click != null)
                Click(this, button);
            mode = ButtonModes.Normal;
        }
        protected void onButtonHover()
        {
            if (Hover != null)
                Hover(this);
        }

        public void Press()
        {
            if (Click != null)
                Click(this, MouseButtons.Left);
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
        private Color4 textColor, fillColor;
        private Color4 textColorHover, fillColorHover;
        private Color4 textColorPressed, fillColorPressed;
        private ButtonModes mode;
        private Point textPosition;

        private enum ButtonModes { Normal, Hover, Pressed }

        #region Properties
        public string Text { get { return text; } set { text = value; setTextPosition(); } }
        public Color4 TextColor { get { return textColor; } set { textColor = value; } }
        public Color4 FillColor { get { return fillColor; } set { fillColor = value; } }
        public Color4 TextColorHover { get { return textColorHover; } set { textColorHover = value; } }
        public Color4 FillColorHover { get { return fillColorHover; } set { fillColorHover = value; } }
        public Color4 TextColorPressed { get { return textColorPressed; } set { textColorPressed = value; } }
        public Color4 FillColorPressed { get { return fillColorPressed; } set { fillColorPressed = value; } }
        public OpenTK.Input.Key KeyShortcut { get; set; }
        #endregion
        #region Constants
        private static Color4 DEFAULT_TEXT_COLOR = Color4.White;
        private static Color4 DEFAULT_FILL_COLOR = Color4.Black;

        private static Color4 DEFAULT_TEXT_HOVER_COLOR = Color4.White;
        private static Color4 DEFAULT_FILL_HOVER_COLOR = new Color4(170, 181, 187, 255);

        private static Color4 DEFAULT_TEXT_PRESSED_COLOR = Color4.Black;
        private static Color4 DEFAULT_FILL_PRESSED_COLOR = Color4.White;
        #endregion

        public event ButtonClicked Click;
        public event ButtonHovered Hover;

        public delegate void ButtonClicked(object sender, MouseButtons button);
        public delegate void ButtonHovered(object sender);
    }
}
