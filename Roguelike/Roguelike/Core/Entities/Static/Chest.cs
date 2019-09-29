using System;
using OpenTK.Graphics;
using Roguelike.Core.Items;
using Roguelike.Engine;
using Roguelike.Engine.Factories;

namespace Roguelike.Core.Entities.Static
{
    public class Chest : Entity
    {
        public Chest(Level level)
            : base(level)
        {
            EntityType = EntityTypes.Chest;
            Token = TokenReference.CHEST_CLOSED;

            ForegroundColor = Color4.SaddleBrown;
            IsSolid = true;
        }

        public override void UpdateStep()
        {
            base.UpdateStep();
        }

        public override void OnInteract(Entity entity)
        {
            if (entity.EntityType == EntityTypes.Player)
                spewOutContents();

            base.OnInteract(entity);
        }
        private void spewOutContents()
        {
            int numItems = Engine.RNG.Next(1, 4);
            for (int i = 0; i < numItems; i++)
            {
                Item item = ItemGenerator.GenerateRandomItem();
                item.ParentLevel = parentLevel;
                item.Position = new Point(X, Y);

                parentLevel.FloorItems.Add(item);
            }

            DoPurge = true;
        }
    }
}
