using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Roguelike.Engine
{
    public class Game : IDisposable
    {
        private GameTime gameTime;

        public Game() : this(640, 360) { }
        public Game(int width, int height)
        {
            Window = new GameWindow(width, height, GraphicsMode.Default, "Roguelike Thing");
            Window.RenderFrame += (sender, e) => renderFrame(e);
            Window.UpdateFrame += (sender, e) => updateFrame(e);
            Window.Resize += (sender, e) => Reshape(Window.Width, Window.Height);

            gameTime = new GameTime();

            Initialize();
            LoadContent();
        }

        public virtual void Run()
        {
            Window.Run();
        }
        public virtual void Run(double updateRate)
        {
            Window.Run(updateRate);
        }
        public virtual void Run(double updateRate, double drawRate)
        {
            Window.Run(updateRate, drawRate);
        }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void Dispose()
        {
            UnloadContent();
        }

        public virtual void BeginRenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }
        public virtual void RenderFrame(GameTime gameTime) { }
        public virtual void EndRenderFrame()
        {
            Window.SwapBuffers();
        }

        public virtual void UpdateFrame(GameTime gameTime) { }

        public virtual void Reshape(int width, int height) { }

        public GameWindow Window { get; protected set; }
        public ContentManager Content { get; protected set; }

        // Private Methods
        private void renderFrame(FrameEventArgs e)
        {
            BeginRenderFrame();
            RenderFrame(gameTime);
            EndRenderFrame();
        }
        private void updateFrame(FrameEventArgs e)
        {
            gameTime.ElapsedTime = TimeSpan.FromSeconds(e.Time);
            gameTime.TotalTime.Add(gameTime.ElapsedTime);

            if (Window.Focused)
            {
                UpdateFrame(gameTime);
            }
        }
    }
}
