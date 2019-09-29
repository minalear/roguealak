using System;
using Roguelike.Engine;

namespace Roguelike.Core.Combat.Abilities
{
    public class BasicPoisonAttack : Ability
    {
        public BasicPoisonAttack()
            : base()
        {
            abilityType = AbilityTypes.Physical;
        }

        public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
        {
            CombatResults results = DoesAttackHit(caster, target);

            if (!results.DidMiss && !results.DidAvoid)
            {
                int damage = (int)(caster.AttackPower.EffectiveValue * 0.8);
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

            int result = Engine.RNG.Next(0, 100);
            if (result <= 100)
            {
                if (!target.HasEffect("Basic DoT"))
                    target.ApplyEffect(new Effects.SimpleDot(target));
            }

            return results;
        }
    }
}
