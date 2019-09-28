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
            this.setDefaults();

            this.text = text;
            this.position = new Point(x, y);
            this.size = new Point(text.Length + 2, 3);

            this.setTextPosition();
        }
        public Button(Control parent, string text, int x, int y, int width, int height)
            : base(parent)
        {
            this.setDefaults();

            this.text = text;
            this.position = new Point(x, y);
            this.size = new Point(width, height);

            this.setTextPosition();
        }
        
        public override void DrawStep()
        {
            this.clearArea();

            if (this.mode == ButtonModes.Normal)
            {
                //Fill Area
                GraphicConsole.SetColors(Color.Transparent, this.fillColor);
                DrawingUtilities.DrawRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

                //Write Text
                GraphicConsole.SetColors(this.textColor, this.fillColor);
                GraphicConsole.SetCursor(this.textPosition);
                GraphicConsole.Write(this.text);
            }
            else if (this.mode == ButtonModes.Hover)
            {
                //Fill Area
                GraphicConsole.SetColors(Color.Transparent, this.fillColorHover);
                DrawingUtilities.DrawRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

                //Write Text
                GraphicConsole.SetColors(this.textColorHover, this.fillColorHover);
                GraphicConsole.SetCursor(this.textPosition);
                GraphicConsole.Write(this.text);
            }
            else if (this.mode == ButtonModes.Pressed)
            {
                //Fill Area
                GraphicConsole.SetColors(Color.Transparent, this.fillColorPressed);
                DrawingUtilities.DrawRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

                //Write Text
                GraphicConsole.SetColors(this.textColorPressed, this.fillColorPressed);
                GraphicConsole.SetCursor(this.textPosition);
                GraphicConsole.Write(this.text);
            }

            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (this.isMouseHover())
            {
                #region IsMouseHover Branch
                this.mode = ButtonModes.Hover;

                if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                {
                    this.onButtonPress(MouseButtons.Left);

                    InterfaceManager.UpdateStep();
                    InterfaceManager.DrawStep();
                }
                else if (InputManager.MouseButtonWasClicked(MouseButtons.Middle))
                {
                    this.onButtonPress(MouseButtons.Middle);

                    InterfaceManager.UpdateStep();
                    InterfaceManager.DrawStep();
                }
                else if (InputManager.MouseButtonWasClicked(MouseButtons.Right))
                {
                    this.onButtonPress(MouseButtons.Right);

                    InterfaceManager.UpdateStep();
                    InterfaceManager.DrawStep();
                }
                else if (InputManager.MouseButtonIsDown(MouseButtons.Left))
                {
                    this.mode = ButtonModes.Pressed;
                    this.DrawStep();
                }
                else if (InputManager.MouseButtonIsDown(MouseButtons.Middle))
                {
                    this.mode = ButtonModes.Pressed;
                    this.DrawStep();
                }
                else if (InputManager.MouseButtonIsDown(MouseButtons.Right))
                {
                    this.mode = ButtonModes.Pressed;
                    this.DrawStep();
                }
                else if (!this.wasHover())
                {
                    this.onButtonHover();
                    this.DrawStep();
                }
                #endregion
            }
            else if (this.wasHover())
            {
                this.mode = ButtonModes.Normal;
                this.DrawStep();
            }

            if (InputManager.KeyWasReleased(this.KeyShortcut))
                onButtonPress(MouseButtons.Left);
        }

        //Event Methods
        protected void onButtonPress(MouseButtons button)
        {
            if (this.Click != null)
                this.Click(this, button);
            this.mode = ButtonModes.Normal;
        }
        protected void onButtonHover()
        {
            if (this.Hover != null)
                this.Hover(this);
        }

        public void Press()
        {
            if (this.Click != null)
                this.Click(this, MouseButtons.Left);
        }

        private void setDefaults()
        {
            this.textColor = DEFAULT_TEXT_COLOR;
            this.fillColor = DEFAULT_FILL_COLOR;

            this.textColorHover = DEFAULT_TEXT_HOVER_COLOR;
            this.fillColorHover = DEFAULT_FILL_HOVER_COLOR;

            this.textColorPressed = DEFAULT_TEXT_PRESSED_COLOR;
            this.fillColorPressed = DEFAULT_FILL_PRESSED_COLOR;
        }
        private void setTextPosition()
        {
            this.textPosition.X = this.Position.X + (this.Size.X / 2 - this.text.Length / 2);
            this.textPosition.Y = (this.Size.Y / 2) + this.Position.Y;
        }

        private string text;
        private Color textColor, fillColor;
        private Color textColorHover, fillColorHover;
        private Color textColorPressed, fillColorPressed;
        private ButtonModes mode;
        private Point textPosition;

        private enum ButtonModes { Normal, Hover, Pressed }

        #region Properties
        public string Text { get { return this.text; } set { this.text = value; this.setTextPosition(); } }
        public Color TextColor { get { return this.textColor; } set { this.textColor = value; } }
        public Color FillColor { get { return this.fillColor; } set { this.fillColor = value; } }
        public Color TextColorHover { get { return this.textColorHover; } set { this.textColorHover = value; } }
        public Color FillColorHover { get { return this.fillColorHover; } set { this.fillColorHover = value; } }
        public Color TextColorPressed { get { return this.textColorPressed; } set { this.textColorPressed = value; } }
        public Color FillColorPressed { get { return this.fillColorPressed; } set { this.fillColorPressed = value; } }
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
