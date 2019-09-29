using System;
using Roguelike.Core.Entities;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Combat.Effects
{
    public class RogueEvasion : Effect
    {
        // double evasionBonus = 25.0;

        public RogueEvasion(StatsPackage package)
            : base(package, 0)
        {
            EffectName = "Rogue Evasion";
            IsHarmful = false;
        }

        public override void OnAttack(CombatResults results)
        {
            if (parent == results.Target && results.DidCrit) //IF WE GOT CRITTED
            {

            }
            
            base.OnAttack(results);
        }

        public override void CalculateStats()
        {
            parent.PhysicalAvoidance.ModValue += 10.0;
            parent.SpellAvoidance.ModValue += 7.5;

            base.CalculateStats();
        }
    }
}
