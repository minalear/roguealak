using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Engine.UI.Controls
{
    public class CheckBox : Control
    {
        public CheckBox(Control parent, int x, int y) 
            : base(parent)
        {
            this.ForegroundColor = Color.White;
            this.BackgroundColor = Color.Black;

            this.Size = new Point(1, 1);
            this.Position = new Point(x, y);
        }

        public override void DrawStep()
        {
            GraphicConsole.SetCursor(this.Position.X, this.Position.Y);

            if (this.isHover) //Mouse is hovering
                GraphicConsole.SetColors(this.foregroundColorHover, this.backgroundColorHover);
            else if (this.enabled) //Check box is checked
                GraphicConsole.SetColors(this.foregroundColorEnabled, this.backgroundColorEnabled);
            else //Check box isn't checked
                GraphicConsole.SetColors(this.foregroundColorDisabled, this.backgroundColorDisabled);


            if (this.enabled)
                GraphicConsole.Write(this.enabledToken);
            else
                GraphicConsole.Write(this.disabledToken);

            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (this.isMouseHover())
            {
                this.isHover = true;

                if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                {
                    this.onToggle();

                    InterfaceManager.UpdateStep();
                    InterfaceManager.DrawStep();

                    this.isHover = false;
                }
                else if (InputManager.MouseButtonIsDown(MouseButtons.Left))
                {
                    this.isHover = false;
                    this.DrawStep();
                }
                else if (!this.wasHover())
                {
                    this.DrawStep();
                }
            }
            else if (this.wasHover())
            {
                this.isHover = false;
                this.DrawStep();
            }
        }

        protected void onToggle()
        {
            this.enabled = !this.enabled;

            if (this.Toggled != null)
                this.Toggled(this);
        }

        private char enabledToken = '⌂';
        private char disabledToken = '⌂';

        private bool enabled = false;
        private bool isHover = false;

        private Color foregroundColorEnabled = Color.Black;
        private Color foregroundColorDisabled = Color.White;
        private Color foregroundColorHover = Color.White;

        private Color backgroundColorEnabled = Color.White;
        private Color backgroundColorDisabled = Color.Black;
        private Color backgroundColorHover = new Color(170, 181, 187);

        #region Properties
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public char EnabledToken { get { return this.enabledToken; } set { this.enabledToken = value; } }
        public char DisabledToken { get { return this.disabledToken; } set { this.disabledToken = value; } }
        public bool Enabled { get { return this.enabled; } set { this.enabled = value; } }
        #endregion

        public event CheckBoxPressed Toggled;
        public delegate void CheckBoxPressed(object sender);
    }
}
