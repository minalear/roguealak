using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using Roguelike.Core.Stats;
using Roguelike.Engine;

namespace Roguelike.Core.Entities
{
    public class PathingEntity : Entity
    {
        Point playerPosition = new Point(1, 1);
        List<Point> path = new List<Point>();

        public static bool AllowedToMove = true;

        public PathingEntity(Level parent)
            : base(parent)
        {
            ForegroundColor = Color4.Black;
            BackgroundColor = Color4.Pink;

            token = '♥';
            EntityType = EntityTypes.Enemy;
            isSolid = true;

            statsPackage = new StatsPackage(this)
            {
                UnitName = "Demon Heart",

                AttackPower = new Stat() { BaseValue = 25.0 },
                PhysicalAvoidance = new Stat() { BaseValue = 15.0 },
                PhysicalReduction = new Stat() { BaseValue = 20.0 },
                PhysicalHitChance = new Stat() { BaseValue = 75.0 },
                Health = 150
            };
        }

        public override void DrawStep(Rectangle viewport)
        {
            base.DrawStep(viewport);
        }

        public override void UpdateStep()
        {
            getPlayerPosition();
            calculatePath();
            //paintPath();

            if (path.Count > 1)
            {
                MoveToTile(path[1].X, path[1].Y);
            }

            base.UpdateStep();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void MoveToTile(int x, int y)
        {
            if (parentLevel.IsBlockedByEntity(x, y))
            {
                Entity entity = parentLevel.GetEntity(x, y);
                if (entity.EntityType == EntityTypes.Player)
                    entity.Attack(this, statsPackage.AbilityList[0]);
                else
                    entity.OnInteract(this);
            }
            else
                base.MoveToTile(x, y);
        }

        private void calculatePath()
        {
            //path = Pathing.PathCalculator.CalculatePath(new Point(X, Y), playerPosition, parentLevel);
        }
        private void getPlayerPosition()
        {
            playerPosition = new Point(GameManager.TestPlayer.X, GameManager.TestPlayer.Y);
        }
        private void paintPath()
        {
            for (int y = 0; y < parentLevel.Matrix.Height; y++)
            {
                for (int x = 0; x < parentLevel.Matrix.Width; x++)
                {
                    parentLevel.StainTile(x, y, Color4.White);
                }
            }

            for (int i = 0; i < path.Count; i++)
                parentLevel.StainTile(path[i].X, path[i].Y, new Color4(255, 0, 0, 255));
        }
    }
}
