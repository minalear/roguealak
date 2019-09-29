using System;
using System.Collections.Generic;
using Roguelike.Core.Combat;

namespace Roguelike.Core.Stats.Classes
{
    public class Cleric : Class
    {
        public Cleric()
            : base("Cleric")
        {
            Description = "Priests are devoted to the spiritual, and express their unwavering faith by serving the people. For millennia they have left behind the confines of their temples and the comfort of their shrines so they can support their allies in war-torn lands. In the midst of terrible conflict, no hero questions the value of the priestly orders.";
            InheritAbilities = new List<Ability>() { new Ability_Heal(), new Ability_Smite() };
            InheritEffects = new List<Effect>() { new Effect_Glory() };
        }
        public override PlayerStats CalculateStats(PlayerStats stats)
        {
            stats.Strength -= 1;
            stats.Agility -= 1;
            stats.Dexterity += 0;

            stats.Intelligence += 2;
            stats.Willpower += 0;
            stats.Wisdom += 1;

            stats.Constitution += 1;
            stats.Endurance += 1;
            stats.Fortitude += 1;
            
            return base.CalculateStats(stats);
        }

        public class Ability_Heal : Ability
        {
            public Ability_Heal()
                : base()
            {
                AbilityName = "Heal";
                AbilityNameShort = "Heal";

                abilityType = AbilityTypes.Magical;
                TargetingType = TargetingTypes.Self;
                abilityCost = 25;
                Range = 15;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                int amount = (int)(target.Health * 0.2);

                if (DoesAttackCrit(caster))
                    amount = ApplyCriticalDamage(amount, caster);

                target.AddHealth(amount);

                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }
        }
        public class Ability_Smite : Ability
        {
            public Ability_Smite()
                : base()
            {
                AbilityName = "Holy Smite";
                AbilityNameShort = "Hly Smt";

                AbilityType = AbilityTypes.Magical;
                TargetingType = TargetingTypes.EntityTarget;
                abilityCost = 50;
                Range = 15;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                CombatResults results = DoesAttackHit(caster, target);

                if (!results.DidMiss && !results.DidAvoid)
                {
                    int damage = (int)(caster.SpellPower.EffectiveValue * 2.15);
                    if (DoesAttackCrit(caster))
                    {
                        damage = ApplyCriticalDamage(damage, caster);
                        results.DidCrit = true;
                    }

                    results.PureDamage = damage;
                    results.AbsorbedDamage = CalculateAbsorption(damage, target);
                    results.AppliedDamage = results.PureDamage - results.AbsorbedDamage;
                    results.ReflectedDamage = CalculateReflectedDamage(results.AppliedDamage, target);
                }

                if (!target.HasEffect("Smitten"))
                {
                    target.ApplyEffect(new Effect_Smite());
                }

                return results;
            }
        }
        public class Effect_Glory : Effect
        {
            public Effect_Glory()
                : base(0)
            {
                EffectName = "Glory to the Gods";
                IsHarmful = false;
                IsImmuneToPurge = true;

                EffectDescription = "The Gods look favorably upon you and grant you a small boon.";
            }

            public override void CalculateStats()
            {
                parent.SpellPower.ModValue += parent.SpellPower.BaseValue * 0.1;

                base.CalculateStats();
            }
        }
        public class Effect_Smite : Effect
        {
            public Effect_Smite()
                : base(10)
            {
                EffectName = "Smitten";
                IsHarmful = true;

                EffectDescription = "You have been smitten by a powerful holy attack reducing your combat prowess for awhile.";
            }

            public override void CalculateStats()
            {
                parent.AttackPower.ModValue -= 25;

                base.CalculateStats();
            }
        }
    }
}
