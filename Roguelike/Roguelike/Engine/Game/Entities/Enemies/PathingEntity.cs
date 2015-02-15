using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Roguelike.Engine.Game.Entities
{
    public class PathingEntity : Entity
    {
        Point playerPosition = new Point(1, 1);
        List<Point> path = new List<Point>();

        public static bool AllowedToMove = true;
        private int health = 10;

        public PathingEntity(Level parent)
            : base(parent)
        {
            this.ForegroundColor = Color.Black;
            this.BackgroundColor = Color.Pink;

            this.token = '♥';
            this.EntityType = EntityTypes.NPC;
            this.isSolid = true;
        }

        public override void DrawStep(Rectangle viewport)
        {
            base.DrawStep(viewport);
        }

        public override void UpdateStep()
        {
            if (this.health <= 0)
                this.DoPurge = true;

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

        public override void OnInteract(Entity entity)
        {
            if (entity.EntityType == EntityTypes.Player)
                this.health = 0;

            base.OnInteract(entity);
        }

        public override void MoveToTile(int x, int y)
        {
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
