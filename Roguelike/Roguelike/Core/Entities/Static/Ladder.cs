using System;
using OpenTK.Graphics;
using Roguelike.Engine;

namespace Roguelike.Core.Entities
{
    public class Ladder : Entity
    {
        private Level targetLevel;
        private Point targetDestination;

        public Ladder(Level parent, Level targetLevel, Point targetDestination)
            : base(parent)
        {
            this.targetLevel = targetLevel;
            this.targetDestination = targetDestination;

            token = TokenReference.LADDER_UP;
            ForegroundColor = Color4.Tan;
            isSolid = true;

            EntityType = EntityTypes.Ladder;
            statsPackage = new Stats.StatsPackage(this) { IsImmune = true };
        }

        public override void OnInteract(Entity entity)
        {
            if (entity.EntityType == EntityTypes.Player)
            {
                //Pathing.PathCalculator.CacheLevel(targetLevel);
                GameManager.Player.TeleportPlayer(targetLevel, targetDestination);
            }
        }
    }
}
