using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Engine.UI.Controls
{
    public class ToggleButtonGroup : Control
    {
        private List<ToggleButton> buttons;
        public ToggleButtonGroup(Control parent, int x, int y)
            : base(parent)
        {
            position = new Point(x, y);
            buttons = new List<ToggleButton>();
        }
        public ToggleButtonGroup(Control parent)
            : base(parent)
        {
            position = new Point(0, 0);
            buttons = new List<ToggleButton>();
        }

        public void AddButton(ToggleButton button)
        {
            button.Click += button_Click;
            buttons.Add(button);
        }

        void button_Click(object sender)
        {
            if (((ToggleButton)sender).Enabled)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    if (buttons[i] != sender)
                    {
                        buttons[i].Enabled = false;
                        buttons[i].UpdateStep();
                        buttons[i].DrawStep();
                    }
                }
            }
        }
    }
}
