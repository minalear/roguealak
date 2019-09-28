using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Engine.Game.Stats;

namespace Roguelike.Engine.Game.Entities
{
    public class Bomb : Entity
    {
        private int timer;
        private int radius = 10;
        private int intensity = 1;
        private bool drawExplosionRadius = false;

        private static char[] explosionTokens = new char[] { ' ', ' ', ' ', ' ', '☼', '≈', '!', '*', '¿', '‼' };
        private static Color[] explosionColors = new Color[] { Color.Red, Color.Yellow, Color.Orange, Color.DarkOrange, Color.Maroon };

        public Bomb(Level parent, int timer)
            : base(parent)
        {
            this.timer = timer;

            this.ForegroundColor = Color.Yellow;
            this.token = TokenReference.FLOOR_OBJECT;
            
            this.isSolid = true;
            this.EntityType = EntityTypes.Other;
        }

        public override void DrawStep(Rectangle viewport)
        {
            if (this.drawExplosionRadius)
                this.drawExplosion(viewport);
            else
                base.DrawStep(viewport);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void UpdateStep()
        {
            this.timer--;

            if (timer == 0)
                this.Explode();
            else if (timer == -1)
                this.DoPurge = true;
            else if (timer % 2 == 0)
                this.ForegroundColor = Color.Yellow;
            else
                this.ForegroundColor = Color.Red;

            base.UpdateStep();
        }

        public void Explode()
        {
            this.drawExplosionRadius = true;
            //this.parentLevel.Detonate(this.x, this.y, this.intensity, this.radius);

            GameManager.Player.UpdateStep();
        }

        private void drawExplosion(Rectangle viewport)
        {
            for (int angle = 0; angle < 360; angle += 1)
            {
                for (int r = 0; r < radius; r++)
                {
                    Point point = new Point();
                    point.X = (int)(this.x + 0.5 + r * Math.Cos(angle));
                    point.Y = (int)(this.y + 0.5 + r * Math.Sin(angle));

                    //GraphicConsole.Put(getExplosionToken(), point.X, point.Y);
                    this.putChar(getExplosionToken(), point.X, point.Y, viewport);
                }
            }
        }
        private char getExplosionToken()
        {
            return explosionTokens[RNG.Next(0, explosionTokens.Length)];
        }
        private Color getExplosionColor()
        {
            return explosionColors[RNG.Next(0, explosionColors.Length)];
        }
        private void putChar(char ch, int x, int y, Rectangle viewport)
        {
            int pointX = x - GameManager.CameraOffset.X + viewport.X;
            int pointY = y - GameManager.CameraOffset.Y + viewport.Y;

            if (pointX >= viewport.Left && pointX < viewport.Right && pointY >= viewport.Top && pointY < viewport.Bottom)
            {
                if (!this.parentLevel.IsOutOfBounds(x, y) && this.parentLevel.Matrix.TerrainMatrix[x, y].IsVisible)
                {
                    GraphicConsole.SetColors(getExplosionColor(), this.BackgroundColor);
                    GraphicConsole.Put(ch, pointX, pointY);
                }
            }
        }
    }
}
