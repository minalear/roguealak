using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Roguelike.Engine.UI.Controls
{
    public class ButtonGroup : Control
    {
        private List<Button> buttons;
        public ButtonGroup(Control parent)
            : base(parent)
        {
            this.position = new Point(0, 0);
            this.buttons = new List<Button>();
        }
        public ButtonGroup(Control parent, int x, int y)
            : base(parent)
        {
            this.position = new Point(x, y);
            this.buttons = new List<Button>();
        }

        public void AddButton(Button button)
        {
            button.Click += button_Click;
            this.buttons.Add(button);
        }

        void button_Click(object sender, MouseButtons button)
        {
            if (Click != null)
                Click((Button)sender);
        }

        public event ButtonClicked Click;
        public delegate void ButtonClicked(Button button);
    }
}
