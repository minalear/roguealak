using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Roguelike.Engine
{
    public class Game : IDisposable
    {
        public Game() : this(640, 360) { }
        public Game(int width, int height) { }

        public virtual void Run() { }
        public virtual void Run(double updateRate) { }
        public virtual void Run(double updateRate, double drawRate) { }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void Dispose()
        {
            UnloadContent();
        }

        public virtual void BeginRenderFrame() { }
        public virtual void RenderFrame(GameTime gameTime) { }
        public virtual void EndRenderFrame() { }

        public virtual void UpdateFrame(GameTime gameTime) { }

        public virtual void Reshape() { }

        public GameWindow Window { get; protected set; }
        public ContentManager Content { get; protected set; }
    }
}
