using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Core.Stats;

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
            this.ForegroundColor = Color.Black;
            this.BackgroundColor = Color.Pink;

            this.token = '♥';
            this.EntityType = EntityTypes.Enemy;
            this.isSolid = true;

            this.statsPackage = new StatsPackage(this)
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
            this.getPlayerPosition();
            this.calculatePath();
            //this.paintPath();

            if (this.path.Count > 1)
            {
                this.MoveToTile(this.path[1].X, this.path[1].Y);
            }

            base.UpdateStep();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void MoveToTile(int x, int y)
        {
            if (this.parentLevel.IsBlockedByEntity(x, y))
            {
                Entity entity = this.parentLevel.GetEntity(x, y);
                if (entity.EntityType == EntityTypes.Player)
                    entity.Attack(this, this.statsPackage.AbilityList[0]);
                else
                    entity.OnInteract(this);
            }
            else
                base.MoveToTile(x, y);
        }

        private void calculatePath()
        {
            this.path = Pathing.PathCalculator.CalculatePath(new Point(this.X, this.Y), this.playerPosition, parentLevel);
        }
        private void getPlayerPosition()
        {
            this.playerPosition = new Point(GameManager.TestPlayer.X, GameManager.TestPlayer.Y);
        }
        private void paintPath()
        {
            for (int y = 0; y < this.parentLevel.Matrix.Height; y++)
            {
                for (int x = 0; x < this.parentLevel.Matrix.Width; x++)
                {
                    this.parentLevel.StainTile(x, y, Color.White);
                }
            }

            for (int i = 0; i < this.path.Count; i++)
                this.parentLevel.StainTile(this.path[i].X, this.path[i].Y, new Color(255, 0, 0));
        }
    }
}
