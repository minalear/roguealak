using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using Roguelike.Engine;

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
            X = position.X;
            Y = position.Y;

            this.direction = direction;

            token = ' ';
            isSolid = false;
        }

        public override void DrawStep(Box2 viewport)
        {
            double angle = Math.Atan2(direction.Y, direction.X);

            double angleLeft = angle - 0.285;
            double angleRight = angle + 0.285;

            for (double a = angleLeft; a < angleRight; a += 0.01)
            {
                for (int r = 0; r < distance; r++)
                {
                    Point position = new Point();
                    position.X = (int)(X + 0.5 + r * Math.Cos(a));
                    position.Y = (int)(Y + 0.5 + r * Math.Sin(a));

                    if (!affectedTiles.Contains(position))
                    {
                        parentLevel.SetToken(MatrixLevels.Effect, position.X, position.Y, getFlameToken(), getFlameColor(), Color.Black);
                        affectedTiles.Add(position);
                    }

                    if (parentLevel.IsTileSolid(position.X, position.Y))
                        break;
                }
            }

            base.DrawStep(viewport);
        }
        public override void UpdateStep()
        {
            duration--;
            if (duration <= 0)
                DoPurge = true;
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
        private Color4 getFlameColor()
        {
            var explosionColors = new Color4[] { Color4.Red, Color4.DarkGoldenrod, Color4.Orange, Color4.DarkOrange, Color4.Maroon };
            return explosionColors[RNG.Next(0, explosionColors.Length)];
        }
        private Color4 getScorchColor()
        {
            var explosionColors = new Color4[] { new Color4(25, 25, 25, 255), new Color4(35, 35, 35, 255), new Color4(5, 5, 5, 255) };
            return explosionColors[RNG.Next(0, explosionColors.Length)];
        }
    }
}
