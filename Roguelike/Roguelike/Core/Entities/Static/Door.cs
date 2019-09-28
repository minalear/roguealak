using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Roguelike.Core.Entities
{
    public class Door : Entity
    {
        public Door(Level parent) 
            : base(parent)
        {
            this.EntityType = EntityTypes.Door;
            this.token = TokenReference.DOOR_CLOSED;

            this.ForegroundColor = Color.BurlyWood;
            this.isSolid = true;
        }

        public override void OnInteract(Entity entity)
        {
            if (/*(entity.EntityType == EntityTypes.Player || entity.EntityType == EntityTypes.NPC) && */this.isClosed)
            {
                this.isClosed = false;
                this.isSolid = false;

                this.token = TokenReference.DOOR_OPEN;
            }
            
            base.OnInteract(entity);
        }
        
        private bool isClosed = true;
    }
}
