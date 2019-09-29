using System;
using Roguelike.Engine.Console;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Engine.UI
{
    public class Interface : Control
    {
        public Interface()
        {
            position = new Point(0, 0);
            size = new Point(GraphicConsole.Instance.BufferWidth, GraphicConsole.Instance.BufferHeight);
        }

        public virtual void OnCall()
        {
            GraphicConsole.Instance.Clear();

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
