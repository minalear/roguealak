using System;

namespace Roguelike.Core.Combat.Abilities
{
    //Every combat-ready entity has the basic attack
    public class BasicAttack : Ability
    {
        public BasicAttack()
            : base()
        {
            AbilityName = "Basic Attack";
            AbilityNameShort = "Bsc Attk";

            abilityType = AbilityTypes.Physical;
            TargetingType = TargetingTypes.EntityTarget;
            Range = 1;
        }

        public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
        {
            CombatResults results = DoesAttackHit(caster, target);

            if (!results.DidMiss && !results.DidAvoid)
            {
                int damage = (int)caster.AttackPower.EffectiveValue;
                if (DoesAttackCrit(caster))
                {
                    damage = ApplyCriticalDamage(damage, caster);
                    results.DidCrit = true;
                }

                results.PureDamage = damage;
                results.AbsorbedDamage = CalculateAbsorption(results.PureDamage, target);
                results.AppliedDamage = results.PureDamage - results.AbsorbedDamage;
                results.ReflectedDamage = CalculateReflectedDamage(results.AppliedDamage, target);
            }

            return results;
        }
    }
}
