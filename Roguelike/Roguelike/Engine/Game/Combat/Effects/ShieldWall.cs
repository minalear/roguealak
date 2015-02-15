using System;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.Game.Stats;

namespace Roguelike.Engine.Game.Combat.Effects
{
    public class ShieldWall : Effect
    {
        private double physicalReduction = 80.0;
        private double magicalReduction = 25.0;

        public ShieldWall(StatsPackage package)
            : base(package, 3)
        {
            this.EffectName = "Shield Wall";
            this.IsHarmful = false;
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
            this.parent.PhysicalReduction += this.physicalReduction;
            this.parent.SpellReduction += this.magicalReduction;
        }
    }
}
