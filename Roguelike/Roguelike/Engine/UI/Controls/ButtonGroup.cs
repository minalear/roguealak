using System;
using System.Collections.Generic;
using Roguelike.Engine.Console;

namespace Roguelike.Engine.UI.Controls
{
    public class ButtonGroup : Control
    {
        private List<Button> buttons;
        public ButtonGroup(Control parent)
            : base(parent)
        {
            position = new Point(0, 0);
            buttons = new List<Button>();
        }
        public ButtonGroup(Control parent, int x, int y)
            : base(parent)
        {
            position = new Point(x, y);
            buttons = new List<Button>();
        }

        public void AddButton(Button button)
        {
            button.Click += button_Click;
            buttons.Add(button);
        }

        void button_Click(object sender, MouseButtons button)
        {
            Click?.Invoke((Button)sender);
        }

        public event ButtonClicked Click;
        public delegate void ButtonClicked(Button button);
    }
}
