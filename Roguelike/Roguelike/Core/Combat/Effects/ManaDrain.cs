using System;
using Roguelike.Core.Entities;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Combat.Effects
{
    public class ManaDrain : Effect
    {
        public ManaDrain(StatsPackage package)
            : base(package, 0)
        {
            this.EffectName = "Mana Shield";
            this.IsHarmful = false;
        }

        public override int OnHealthLoss(int amount)
        {
            if (this.parent.Mana > 0)
            {
                this.parent.DrainMana(amount);
                amount = 0;
            }

            return base.OnHealthLoss(amount);
        }
    }
}
