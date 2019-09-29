using System;
using OpenTK.Graphics;

namespace Roguelike.Engine.UI.Controls
{
    public class CheckBox : Control
    {
        public CheckBox(Control parent, int x, int y) 
            : base(parent)
        {
            ForegroundColor = Color4.White;
            BackgroundColor = Color4.Black;

            Size = new Point(1, 1);
            Position = new Point(x, y);
        }

        public override void DrawStep()
        {
            GraphicConsole.SetCursor(Position.X, Position.Y);

            if (isHover) //Mouse is hovering
                GraphicConsole.SetColors(foregroundColorHover, backgroundColorHover);
            else if (enabled) //Check box is checked
                GraphicConsole.SetColors(foregroundColorEnabled, backgroundColorEnabled);
            else //Check box isn't checked
                GraphicConsole.SetColors(foregroundColorDisabled, backgroundColorDisabled);


            if (enabled)
                GraphicConsole.Write(enabledToken);
            else
                GraphicConsole.Write(disabledToken);

            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (isMouseHover())
            {
                isHover = true;

                if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                {
                    onToggle();

                    InterfaceManager.UpdateStep();
                    InterfaceManager.DrawStep();

                    isHover = false;
                }
                else if (InputManager.MouseButtonIsDown(MouseButtons.Left))
                {
                    isHover = false;
                    DrawStep();
                }
                else if (!wasHover())
                {
                    DrawStep();
                }
            }
            else if (wasHover())
            {
                isHover = false;
                DrawStep();
            }
        }

        protected void onToggle()
        {
            enabled = !enabled;

            Toggled?.Invoke(this);
        }

        private char enabledToken = '⌂';
        private char disabledToken = '⌂';

        private bool enabled = false;
        private bool isHover = false;

        private Color4 foregroundColorEnabled = Color4.Black;
        private Color4 foregroundColorDisabled = Color4.White;
        private Color4 foregroundColorHover = Color4.White;

        private Color4 backgroundColorEnabled = Color4.White;
        private Color4 backgroundColorDisabled = Color4.Black;
        private Color4 backgroundColorHover = new Color4(170, 181, 187, 255);

        #region Properties
        public Color4 ForegroundColor { get; set; }
        public Color4 BackgroundColor { get; set; }
        public char EnabledToken { get { return enabledToken; } set { enabledToken = value; } }
        public char DisabledToken { get { return disabledToken; } set { disabledToken = value; } }
        public bool Enabled { get { return enabled; } set { enabled = value; } }
        #endregion

        public event CheckBoxPressed Toggled;
        public delegate void CheckBoxPressed(object sender);
    }
}
