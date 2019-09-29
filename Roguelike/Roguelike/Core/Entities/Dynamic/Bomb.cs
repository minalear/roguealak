﻿using System;
using OpenTK;
using OpenTK.Graphics;
using Roguelike.Engine;

namespace Roguelike.Core.Entities
{
    public class Bomb : Entity
    {
        private int timer;
        private int radius = 10;
        private int intensity = 1;
        private bool drawExplosionRadius = false;

        private static char[] explosionTokens = new char[] { ' ', ' ', ' ', ' ', '☼', '≈', '!', '*', '¿', '‼' };
        private static Color4[] explosionColors = new Color4[] { Color4.Red, Color4.Yellow, Color4.Orange, Color4.DarkOrange, Color4.Maroon };

        public Bomb(Level parent, int timer)
            : base(parent)
        {
            this.timer = timer;

            ForegroundColor = Color4.Yellow;
            token = TokenReference.FLOOR_OBJECT;
            
            isSolid = true;
            EntityType = EntityTypes.Other;
        }

        public override void DrawStep(Box2 viewport)
        {
            if (drawExplosionRadius)
                drawExplosion(viewport);
            else
                base.DrawStep(viewport);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void UpdateStep()
        {
            timer--;

            if (timer == 0)
                Explode();
            else if (timer == -1)
                DoPurge = true;
            else if (timer % 2 == 0)
                ForegroundColor = Color4.Yellow;
            else
                ForegroundColor = Color4.Red;

            base.UpdateStep();
        }

        public void Explode()
        {
            drawExplosionRadius = true;
            //parentLevel.Detonate(x, y, intensity, radius);

            GameManager.Player.UpdateStep();
        }

        private void drawExplosion(Box2 viewport)
        {
            for (int angle = 0; angle < 360; angle += 1)
            {
                for (int r = 0; r < radius; r++)
                {
                    Point point = new Point();
                    point.X = (int)(x + 0.5 + r * Math.Cos(angle));
                    point.Y = (int)(y + 0.5 + r * Math.Sin(angle));

                    //GraphicConsole.Put(getExplosionToken(), point.X, point.Y);
                    putChar(getExplosionToken(), point.X, point.Y, viewport);
                }
            }
        }
        private char getExplosionToken()
        {
            return explosionTokens[RNG.Next(0, explosionTokens.Length)];
        }
        private Color4 getExplosionColor()
        {
            return explosionColors[RNG.Next(0, explosionColors.Length)];
        }
        private void putChar(char ch, int x, int y, Box2 viewport)
        {
            int pointX = x - GameManager.CameraOffset.X + viewport.X;
            int pointY = y - GameManager.CameraOffset.Y + viewport.Y;

            if (pointX >= viewport.Left && pointX < viewport.Right && pointY >= viewport.Top && pointY < viewport.Bottom)
            {
                if (!parentLevel.IsOutOfBounds(x, y) && parentLevel.Matrix.TerrainMatrix[x, y].IsVisible)
                {
                    GraphicConsole.SetColors(getExplosionColor(), BackgroundColor);
                    GraphicConsole.Put(ch, pointX, pointY);
                }
            }
        }
    }
}
