using System;
using Roguelike.Core.Combat;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Items
{
    public class Food : Item
    {
        public Food()
            : base(ItemTypes.Food)
        {
            this.RemoveOnUse = true;
        }

        public override void OnUse(Entities.Entity entity)
        {
            if (this.onUseEffect != null)
                entity.StatsPackage.ApplyEffect(this.onUseEffect);

            //TODO Remove this when it's not fucking stupid
            if (this.Name == "Sweet Roll")
            {
                GameManager.FakeScore += 10;
                GameManager.SweetRolls++;
            }
        }

        private Effect onUseEffect;
        public Effect OnUseEffect { get { return this.onUseEffect; } set { this.onUseEffect = value; } }
    }
}
