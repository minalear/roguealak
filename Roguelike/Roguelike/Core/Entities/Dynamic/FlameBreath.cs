using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Entities.Dynamic
{
    public class FlameBreath : Entity
    {
        int duration = 5;
        int distance = 3;
        Point direction;
        List<Point> affectedTiles = new List<Point>();

        public FlameBreath(Level parent, Point position, Point direction)
            : base(parent)
        {
            this.X = position.X;
            this.Y = position.Y;

            this.direction = direction;

            this.token = ' ';
            this.isSolid = false;
        }

        public override void DrawStep(Rectangle viewport)
        {
            double angle = Math.Atan2(direction.Y, direction.X);

            double angleLeft = angle - 0.285;
            double angleRight = angle + 0.285;

            for (double a = angleLeft; a < angleRight; a += 0.01)
            {
                for (int r = 0; r < distance; r++)
                {
                    Point position = new Point();
                    position.X = (int)(this.X + 0.5 + r * Math.Cos(a));
                    position.Y = (int)(this.Y + 0.5 + r * Math.Sin(a));

                    if (!affectedTiles.Contains(position))
                    {
                        this.parentLevel.SetToken(MatrixLevels.Effect, position.X, position.Y, getFlameToken(), getFlameColor(), Color.Black);
                        affectedTiles.Add(position);
                    }

                    if (this.parentLevel.IsTileSolid(position.X, position.Y))
                        break;
                }
            }

            base.DrawStep(viewport);
        }
        public override void UpdateStep()
        {
            duration--;
            if (duration <= 0)
                this.DoPurge = true;
            distance += 3;

            for (int i = 0; i < affectedTiles.Count; i++)
            {
                Entity entity = GameManager.CurrentLevel.GetEntity(affectedTiles[i].X, affectedTiles[i].Y);
                if (entity != null && entity.StatsPackage != null && entity.GetType() != typeof(FlameBreath))
                {
                    if (!entity.StatsPackage.HasEffect("Ignite"))
                        entity.StatsPackage.ApplyEffect(new Stats.Classes.Mage.Effect_FireballDOT(entity.StatsPackage));
                }
                GameManager.CurrentLevel.DamageTile(affectedTiles[i].X, affectedTiles[i].Y);
                GameManager.CurrentLevel.StainTile(affectedTiles[i].X, affectedTiles[i].Y, getScorchColor());
            }

            affectedTiles.Clear();
            base.UpdateStep();
        }

        private char getFlameToken()
        {
            char[] token = new char[] { '*', '#', '☼', '≈', '░' };

            return token[RNG.Next(0, token.Length)];
        }
        private Color getFlameColor()
        {
            Color[] explosionColors = new Color[] { Color.Red, Color.DarkGoldenrod, Color.Orange, Color.DarkOrange, Color.Maroon };

            return explosionColors[RNG.Next(0, explosionColors.Length)];
        }
        private Color getScorchColor()
        {
            Color[] explosionColors = new Color[] { new Color(25, 25, 25), new Color(35, 35, 35), new Color(5, 5, 5) };

            return explosionColors[RNG.Next(0, explosionColors.Length)];
        }
    }
}
