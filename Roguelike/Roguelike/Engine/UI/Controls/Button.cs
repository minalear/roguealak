﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Engine.UI.Controls
{
    public class Button : Control
    {
        public Button(Control parent, string text, int x, int y)
            : base(parent)
        {
            setDefaults();

            text = text;
            position = new Point(x, y);
            size = new Point(text.Length + 2, 3);

            setTextPosition();
        }
        public Button(Control parent, string text, int x, int y, int width, int height)
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

            if (mode == ButtonModes.Normal)
            {
                //Fill Area
                GraphicConsole.SetColors(Color.Transparent, fillColor);
                DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

                //Write Text
                GraphicConsole.SetColors(textColor, fillColor);
                GraphicConsole.SetCursor(textPosition);
                GraphicConsole.Write(text);
            }
            else if (mode == ButtonModes.Hover)
            {
                //Fill Area
                GraphicConsole.SetColors(Color.Transparent, fillColorHover);
                DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

                //Write Text
                GraphicConsole.SetColors(textColorHover, fillColorHover);
                GraphicConsole.SetCursor(textPosition);
                GraphicConsole.Write(text);
            }
            else if (mode == ButtonModes.Pressed)
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
        private Color textColor, fillColor;
        private Color textColorHover, fillColorHover;
        private Color textColorPressed, fillColorPressed;
        private ButtonModes mode;
        private Point textPosition;

        private enum ButtonModes { Normal, Hover, Pressed }

        #region Properties
        public string Text { get { return text; } set { text = value; setTextPosition(); } }
        public Color TextColor { get { return textColor; } set { textColor = value; } }
        public Color FillColor { get { return fillColor; } set { fillColor = value; } }
        public Color TextColorHover { get { return textColorHover; } set { textColorHover = value; } }
        public Color FillColorHover { get { return fillColorHover; } set { fillColorHover = value; } }
        public Color TextColorPressed { get { return textColorPressed; } set { textColorPressed = value; } }
        public Color FillColorPressed { get { return fillColorPressed; } set { fillColorPressed = value; } }
        public Keys KeyShortcut { get; set; }
        #endregion
        #region Constants
        private static Color DEFAULT_TEXT_COLOR = Color.White;
        private static Color DEFAULT_FILL_COLOR = Color.Black;

        private static Color DEFAULT_TEXT_HOVER_COLOR = Color.White;
        private static Color DEFAULT_FILL_HOVER_COLOR = new Color(170, 181, 187);

        private static Color DEFAULT_TEXT_PRESSED_COLOR = Color.Black;
        private static Color DEFAULT_FILL_PRESSED_COLOR = Color.White;
        #endregion

        public event ButtonClicked Click;
        public event ButtonHovered Hover;

        public delegate void ButtonClicked(object sender, MouseButtons button);
        public delegate void ButtonHovered(object sender);
    }
}
