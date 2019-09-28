using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Roguelike.Core.Entities.Static
{
    public class WinShrine : Entity
    {
        public WinShrine(Level parent)
            : base(parent)
        {
            this.EntityType = EntityTypes.Other;
            this.token = 'Φ';

            this.ForegroundColor = Color.WhiteSmoke;
            this.isSolid = true;
            this.statsPackage = new Stats.StatsPackage(this) { IsImmune = true };
        }

        public override void OnInteract(Entity entity)
        {
            if (entity.EntityType == EntityTypes.Player)
            {
                GameManager.ChangeGameState(GameStates.Win);
            }
            
            base.OnInteract(entity);
        }
    }
}
