using System;
using System.Text;
using System.Collections.Generic;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Engine.UI
{
    public class Interface : Control
    {
        public Interface()
        {
            position = new Point(0, 0);
            size = new Point(GraphicConsole.BufferWidth, GraphicConsole.BufferHeight);
        }

        public virtual void OnCall()
        {
            GraphicConsole.Clear();

            InterfaceManager.UpdateStep();
            InterfaceManager.DrawStep();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].IsVisible)
                    Children[i].Update(gameTime);
            }
        }

        public override void DrawStep()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].IsVisible)
                    Children[i].DrawStep();
            }
        }
        public override void UpdateStep()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].IsVisible)
                    Children[i].UpdateStep();
            }
        }
    }
}
