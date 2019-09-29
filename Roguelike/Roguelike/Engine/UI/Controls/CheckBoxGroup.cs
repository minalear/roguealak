using System;
using System.Collections.Generic;
using Roguelike.Engine.Console;

namespace Roguelike.Engine.UI.Controls
{
    public class CheckBoxGroup : Control
    {
        private List<CheckBox> checkboxes;
        public CheckBoxGroup(Control parent)
        {
            position = new Point(0, 0);
            checkboxes = new List<CheckBox>();
        }
        public CheckBoxGroup(Control parent, int x, int y)
            : base(parent)
        {
            position = new Point(x, y);
            checkboxes = new List<CheckBox>();
        }

        public void AddCheckbox(CheckBox checkBox)
        {
            checkBox.Toggled += checkBox_Toggled;
            checkboxes.Add(checkBox);
        }

        void checkBox_Toggled(object sender)
        {
            if (((CheckBox)sender).Enabled)
            {
                for (int i = 0; i < checkboxes.Count; i++)
                {
                    if (checkboxes[i] != sender)
                    {
                        checkboxes[i].Enabled = false;
                        checkboxes[i].DrawStep();
                    }
                }
            }
        }
    }
}
