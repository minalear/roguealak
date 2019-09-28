using System;
using System.Timers;

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

        public int CurrentFrame { get { return this.currentFrame; } set { this.currentFrame = value; } }
        public bool Paused { get { return this.isPaused; } set { this.isPaused = value; } }

        public AnimBox(Control parent, int frameCount)
            : base(parent)
        {
            this.frameCount = frameCount;
            this.animationFrames = new string[this.frameCount];

            this.currentFrame = 0;
            this.elapsedTime = 0.0;
            this.timePerFrame = 75.0;
        }

        public override void DrawStep()
        {
            this.clearArea();
            GraphicConsole.SetCursor(this.Position.X, this.Position.Y);
            GraphicConsole.Write(this.animationFrames[this.currentFrame]);

            base.DrawStep();
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.isPaused)
            {
                this.elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (this.elapsedTime >= this.timePerFrame)
                    this.UpdateStep();
            }
        }

        public override void UpdateStep()
        {
            this.DrawStep();

            this.elapsedTime = 0.0;
            this.currentFrame++;

            if (this.currentFrame >= this.frameCount)
                this.currentFrame = 0;
        }

        public void Initialize(int x, int y, int width, int height, double frameDelay)
        {
            this.Position = new Point(x, y);
            this.Size = new Point(width, height);

            this.timePerFrame = frameDelay;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.currentFrame++;
            if (this.currentFrame >= this.frameCount)
                this.currentFrame = 0;
        }

        public void LoadFrame(int frame, string frameData)
        {
            if (frame >= 0 && frame < this.frameCount)
            {
                frameData = frameData.Replace("\r", string.Empty);
                this.animationFrames[frame] = frameData;
            }
        }

        public void PlayPause()
        {
            if (this.isPaused)
                this.isPaused = false;
            else
                this.isPaused = true;
        }
    }
}
