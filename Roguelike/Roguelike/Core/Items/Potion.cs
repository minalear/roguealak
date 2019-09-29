using System;
using Roguelike.Core.Combat;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Items
{
    public class Potion : Item
    {
        public Potion(Effect onUseEffect)
            : base(ItemTypes.Potion)
        {
            RemoveOnUse = true;
            this.onUseEffect = onUseEffect;
        }

        public override void OnUse(Entities.Entity entity)
        {
            if (onUseEffect != null)
                entity.StatsPackage.ApplyEffect(onUseEffect);
        }

        public override string GetDescription()
        {
            return Name + " - " + ItemType.ToString() + "\n" + "Gold: " + Value + "\n" + Description;
        }

        private Effect onUseEffect;
        public Effect OnUseEffect { get { return onUseEffect; } set { onUseEffect = value; } }
    }
}
