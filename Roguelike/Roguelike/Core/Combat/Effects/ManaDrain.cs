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
            EffectName = "Mana Shield";
            IsHarmful = false;
        }

        public override int OnHealthLoss(int amount)
        {
            if (parent.Mana > 0)
            {
                parent.DrainMana(amount);
                amount = 0;
            }

            return base.OnHealthLoss(amount);
        }
    }
}
