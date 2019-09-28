using System;
using Roguelike.Core.Entities;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Combat.Effects
{
    public class SimpleDot : Effect
    {
        public SimpleDot(StatsPackage package)
            : base(package, 100)
        {
            this.EffectName = "Basic DoT";
            this.IsHarmful = true;
        }

        public override void UpdateStep()
        {
            this.parent.DrainHealth(10);
            
            base.UpdateStep();
        }
    }
}
