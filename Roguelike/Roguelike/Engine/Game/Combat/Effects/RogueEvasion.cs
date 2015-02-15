using System;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.Game.Stats;

namespace Roguelike.Engine.Game.Combat.Effects
{
    public class RogueEvasion : Effect
    {
        double evasionBonus = 25.0;

        public RogueEvasion(StatsPackage package)
            : base(package, 0)
        {
            this.EffectName = "Rogue Evasion";
            this.IsHarmful = false;
        }

        public override void OnAttack(CombatResults results)
        {
            if (this.parent == results.Target && results.DidCrit) //IF WE GOT CRITTED
            {

            }
            
            base.OnAttack(results);
        }

        public override void CalculateStats()
        {
            this.parent.PhysicalAvoidance.ModValue += 10.0;
            this.parent.SpellAvoidance.ModValue += 7.5;

            base.CalculateStats();
        }
    }
}
