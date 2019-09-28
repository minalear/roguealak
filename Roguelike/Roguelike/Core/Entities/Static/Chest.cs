using System;
using Roguelike.Core.Items;

namespace Roguelike.Core.Entities.Static
{
    public class Chest : Entity
    {
        public Chest(Level level)
            : base(level)
        {
            this.EntityType = EntityTypes.Chest;
            this.Token = TokenReference.CHEST_CLOSED;

            this.ForegroundColor = Color.SaddleBrown;
            this.IsSolid = true;
        }

        public override void UpdateStep()
        {
            base.UpdateStep();
        }

        public override void OnInteract(Entity entity)
        {
            if (entity.EntityType == EntityTypes.Player)
                this.spewOutContents();

            base.OnInteract(entity);
        }
        private void spewOutContents()
        {
            int numItems = RNG.Next(1, 4);
            for (int i = 0; i < numItems; i++)
            {
                Item item = Factories.ItemGenerator.GenerateRandomItem();
                item.ParentLevel = this.parentLevel;
                item.Position = new Point(this.X, this.Y);

                this.parentLevel.FloorItems.Add(item);
            }

            this.DoPurge = true;
        }
    }
}
