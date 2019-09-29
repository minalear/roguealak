using System;
using OpenTK.Graphics;
using Roguelike.Engine;

namespace Roguelike.Core.Entities.Static
{
    public class WinShrine : Entity
    {
        public WinShrine(Level parent)
            : base(parent)
        {
            EntityType = EntityTypes.Other;
            token = 'Φ';

            ForegroundColor = Color4.WhiteSmoke;
            isSolid = true;
            statsPackage = new Stats.StatsPackage(this) { IsImmune = true };
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
