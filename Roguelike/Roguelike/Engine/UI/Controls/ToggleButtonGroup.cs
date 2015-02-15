using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Roguelike.Engine.UI.Controls
{
    public class ToggleButtonGroup : Control
    {
        private List<ToggleButton> buttons;
        public ToggleButtonGroup(Control parent, int x, int y)
            : base(parent)
        {
            this.position = new Point(x, y);
            this.buttons = new List<ToggleButton>();
        }
        public ToggleButtonGroup(Control parent)
            : base(parent)
        {
            this.position = new Point(0, 0);
            this.buttons = new List<ToggleButton>();
        }

        public void AddButton(ToggleButton button)
        {
            button.Click += button_Click;
            this.buttons.Add(button);
        }

        void button_Click(object sender)
        {
            if (((ToggleButton)sender).Enabled)
            {
                for (int i = 0; i < this.buttons.Count; i++)
                {
                    if (this.buttons[i] != sender)
                    {
                        this.buttons[i].Enabled = false;
                        this.buttons[i].UpdateStep();
                        this.buttons[i].DrawStep();
                    }
                }
            }
        }
    }
}
