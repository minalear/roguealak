using System;
using Roguelike.Core.Entities;

namespace Roguelike.Core.Combat.Abilities
{
    public class ShieldWall : Ability
    {
        public ShieldWall()
            : base()
        {
            AbilityName = "Shield Wall";
            AbilityNameShort = "Shld Wll";

            abilityCost = 50;
            TargetingType = TargetingTypes.Self;
        }

        public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
        {
            results = new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            caster.ApplyEffect(new Effects.ShieldWall(caster));
            return results;
        }
    }
}
