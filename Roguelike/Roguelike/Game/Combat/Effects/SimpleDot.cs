using System;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.Game.Stats;

namespace Roguelike.Engine.Game.Combat.Effects
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
