using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Roguelike.Engine.UI.Controls
{
    public class CheckBoxGroup : Control
    {
        private List<CheckBox> checkboxes;
        public CheckBoxGroup(Control parent)
        {
            this.position = new Point(0, 0);
            this.checkboxes = new List<CheckBox>();
        }
        public CheckBoxGroup(Control parent, int x, int y)
            : base(parent)
        {
            this.position = new Point(x, y);
            this.checkboxes = new List<CheckBox>();
        }

        public void AddCheckbox(CheckBox checkBox)
        {
            checkBox.Toggled += checkBox_Toggled;
            this.checkboxes.Add(checkBox);
        }

        void checkBox_Toggled(object sender)
        {
            if (((CheckBox)sender).Enabled)
            {
                for (int i = 0; i < this.checkboxes.Count; i++)
                {
                    if (this.checkboxes[i] != sender)
                    {
                        this.checkboxes[i].Enabled = false;
                        this.checkboxes[i].DrawStep();
                    }
                }
            }
        }
    }
}
