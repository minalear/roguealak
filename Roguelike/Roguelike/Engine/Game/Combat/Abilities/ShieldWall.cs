using System;
using Roguelike.Engine.Game.Entities;

namespace Roguelike.Engine.Game.Combat.Abilities
{
    public class ShieldWall : Ability
    {
        public ShieldWall()
            : base()
        {
            this.AbilityName = "Shield Wall";
            this.AbilityNameShort = "Shld Wll";

            this.abilityCost = 50;
            this.TargetingType = TargetingTypes.Self;
        }

        public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
        {
            results = new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            caster.ApplyEffect(new Effects.ShieldWall(caster));
            return results;
        }
    }
}
