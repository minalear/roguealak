using System;
using System.Timers;
using Roguelike.Engine.Console;

namespace Roguelike.Engine.UI.Controls
{
    [Obsolete("I don't know if this works or not.  No guarantees.")]
    public class AnimBox : Control
    {
        private string[] animationFrames;
        private int frameCount;

        private int currentFrame;
        private double elapsedTime;
        private double timePerFrame;

        private bool isPaused = false;

        public int CurrentFrame { get { return currentFrame; } set { currentFrame = value; } }
        public bool Paused { get { return isPaused; } set { isPaused = value; } }

        public AnimBox(Control parent, int frameCount)
            : base(parent)
        {
            this.frameCount = frameCount;
            animationFrames = new string[frameCount];

            currentFrame = 0;
            elapsedTime = 0.0;
            timePerFrame = 75.0;
        }

        public override void DrawStep()
        {
            clearArea();
            GraphicConsole.Instance.SetCursor(Position.X, Position.Y);
            GraphicConsole.Instance.Write(animationFrames[currentFrame]);

            base.DrawStep();
        }

        public override void Update(GameTime gameTime)
        {
            if (!isPaused)
            {
                elapsedTime += gameTime.ElapsedTime.Milliseconds;

                if (elapsedTime >= timePerFrame)
                    UpdateStep();
            }
        }

        public override void UpdateStep()
        {
            DrawStep();

            elapsedTime = 0.0;
            currentFrame++;

            if (currentFrame >= frameCount)
                currentFrame = 0;
        }

        public void Initialize(int x, int y, int width, int height, double frameDelay)
        {
            Position = new Point(x, y);
            Size = new Point(width, height);

            timePerFrame = frameDelay;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            currentFrame++;
            if (currentFrame >= frameCount)
                currentFrame = 0;
        }

        public void LoadFrame(int frame, string frameData)
        {
            if (frame >= 0 && frame < frameCount)
            {
                frameData = frameData.Replace("\r", string.Empty);
                animationFrames[frame] = frameData;
            }
        }

        public void PlayPause()
        {
            if (isPaused)
                isPaused = false;
            else
                isPaused = true;
        }
    }
}
