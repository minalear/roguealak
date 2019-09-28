using System;

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

            this.token = TokenReference.LADDER_UP;
            this.ForegroundColor = Color.Tan;
            this.isSolid = true;

            this.EntityType = EntityTypes.Ladder;
            this.statsPackage = new Stats.StatsPackage(this) { IsImmune = true };
        }

        public override void OnInteract(Entity entity)
        {
            if (entity.EntityType == EntityTypes.Player)
            {
                Pathing.PathCalculator.CacheLevel(this.targetLevel);
                GameManager.Player.TeleportPlayer(this.targetLevel, this.targetDestination);
            }
        }
    }
}
