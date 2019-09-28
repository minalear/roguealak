using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Engine.Game.Combat.Abilities
{
    public class BasicPoisonAttack : Ability
    {
        public BasicPoisonAttack()
            : base()
        {
            this.abilityType = AbilityTypes.Physical;
        }

        public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
        {
            CombatResults results = this.DoesAttackHit(caster, target);

            if (!results.DidMiss && !results.DidAvoid)
            {
                int damage = (int)(caster.AttackPower.EffectiveValue * 0.8);
                if (this.DoesAttackCrit(caster))
                {
                    damage = this.ApplyCriticalDamage(damage, caster);
                    results.DidCrit = true;
                }

                results.PureDamage = damage;
                results.AbsorbedDamage = this.CalculateAbsorption(results.PureDamage, target);
                results.AppliedDamage = results.PureDamage - results.AbsorbedDamage;
                results.ReflectedDamage = this.CalculateReflectedDamage(results.AppliedDamage, target);
            }

            int result = RNG.Next(0, 100);
            if (result <= 100)
            {
                if (!target.HasEffect("Basic DoT"))
                    target.ApplyEffect(new Effects.SimpleDot(target));
            }

            return results;
        }
    }
}
