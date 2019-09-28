using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Core.Combat;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Items
{
    public class Potion : Item
    {
        public Potion(Effect onUseEffect)
            : base(ItemTypes.Potion)
        {
            this.RemoveOnUse = true;
            this.onUseEffect = onUseEffect;
        }

        public override void OnUse(Entities.Entity entity)
        {
            if (this.onUseEffect != null)
                entity.StatsPackage.ApplyEffect(this.onUseEffect);
        }

        public override string GetDescription()
        {
            return this.Name + " - " + this.ItemType.ToString() + "\n" + "Gold: " + this.Value + "\n" + this.Description;
        }

        private Effect onUseEffect;
        public Effect OnUseEffect { get { return this.onUseEffect; } set { this.onUseEffect = value; } }
    }
}
