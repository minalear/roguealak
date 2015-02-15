using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Engine.UI
{
    public class Interface : Control
    {
        public Interface()
        {
            this.position = new Point(0, 0);
            this.size = new Point(GraphicConsole.BufferWidth, GraphicConsole.BufferHeight);
        }

        public virtual void OnCall()
        {
            GraphicConsole.Clear();

            InterfaceManager.UpdateStep();
            InterfaceManager.DrawStep();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.Children.Count; i++)
            {
                if (this.Children[i].IsVisible)
                    this.Children[i].Update(gameTime);
            }
        }

        public override void DrawStep()
        {
            for (int i = 0; i < this.Children.Count; i++)
            {
                if (this.Children[i].IsVisible)
                    this.Children[i].DrawStep();
            }
        }
        public override void UpdateStep()
        {
            for (int i = 0; i < this.Children.Count; i++)
            {
                if (this.Children[i].IsVisible)
                    this.Children[i].UpdateStep();
            }
        }
    }
}
