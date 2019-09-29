using System;
using Roguelike.Core.Entities;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Combat.Effects
{
    public class ShieldWall : Effect
    {
        private double physicalReduction = 80.0;
        private double magicalReduction = 25.0;

        public ShieldWall(StatsPackage package)
            : base(package, 3)
        {
            EffectName = "Shield Wall";
            IsHarmful = false;
        }

        public override void OnAttack(CombatResults results)
        {
            base.OnAttack(results);
        }

        public override int OnHealthLoss(int amount)
        {
            return base.OnHealthLoss(amount);
        }

        public override void CalculateStats()
        {
            parent.PhysicalReduction += physicalReduction;
            parent.SpellReduction += magicalReduction;
        }
    }
}
