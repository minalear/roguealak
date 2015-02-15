using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Engine.Game.Combat.Abilities
{
    //Every combat-ready entity has the basic attack
    public class BasicAttack : Ability
    {
        public BasicAttack()
            : base()
        {
            this.AbilityName = "Basic Attack";
            this.AbilityNameShort = "Bsc Attk";

            this.abilityType = AbilityTypes.Physical;
            this.TargetingType = TargetingTypes.EntityTarget;
        }

        public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
        {
            CombatResults results = this.DoesAttackHit(caster, target);

            if (!results.DidMiss && !results.DidAvoid)
            {
                int damage = (int)caster.AttackPower.EffectiveValue;
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

            return results;
        }
    }
}
