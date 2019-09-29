using System;
using OpenTK.Graphics;
using Roguelike.Engine;

namespace Roguelike.Core.Entities
{
    public class Door : Entity
    {
        public Door(Level parent) 
            : base(parent)
        {
            EntityType = EntityTypes.Door;
            token = TokenReference.DOOR_CLOSED;

            ForegroundColor = Color4.BurlyWood;
            isSolid = true;
        }

        public override void OnInteract(Entity entity)
        {
            if (/*(entity.EntityType == EntityTypes.Player || entity.EntityType == EntityTypes.NPC) && */isClosed)
            {
                isClosed = false;
                isSolid = false;

                token = TokenReference.DOOR_OPEN;
            }
            
            base.OnInteract(entity);
        }
        
        private bool isClosed = true;
    }
}
