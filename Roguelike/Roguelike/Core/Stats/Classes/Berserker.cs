using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Core.Combat;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Core.Stats.Classes
{
    public class Berserker : Class
    {
        public Berserker()
            : base("Berserker")
        {
            this.Description = "Berserkers were Norse warriors who are primarily reported in the Old Norse literature to have fought in a nearly uncontrollable, trance-like fury, a characteristic which later gave rise to the English word berserk. Berserkers are attested to in numerous Old Norse sources. Most historians believe that berserkers worked themselves into a rage before battle, while the idea that they consumed drugged foods has also been suggested.";
            this.InheritEffects = new List<Effect>() { new Effect_WildStrikes(), new Effect_UnendingRage() };
            this.InheritAbilities = new List<Ability>() { new Ability_DoubleDamage() };
        }

        public override PlayerStats CalculateStats(PlayerStats stats)
        {
            stats.Strength += 4;
            stats.Agility -= 1;
            stats.Dexterity += 0;

            stats.Intelligence -= 2;
            stats.Willpower += 0;
            stats.Wisdom += 0;

            stats.Constitution += 2;
            stats.Endurance -= 1;
            stats.Fortitude += 0;
            
            return base.CalculateStats(stats);
        }

        public class Ability_DoubleDamage : Ability
        {
            public Ability_DoubleDamage()
                : base(25)
            {
                this.AbilityName = "Inject Steroids";
                this.AbilityNameShort = "Injct Strds";

                this.abilityType = AbilityTypes.Physical;
                this.TargetingType = TargetingTypes.Self;
                this.abilityCost = 25;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                caster.ApplyEffect(new Effect_DoubleDamage());
                return new CombatResults() { Caster = caster, Target = target, DidMiss = true, UsedAbility = this };
            }
        }

        public class Effect_DoubleDamage : Effect
        {
            public Effect_DoubleDamage()
                : base(1)
            {
                this.EffectName = "Steroid";
                this.EffectDescription = "YOU DO MORE DAMAGE RAWWRR";

                this.IsHarmful = false;
                this.IsImmuneToPurge = false;
                this.EffectType = EffectTypes.Physical;
            }

            public override void OnAttack(CombatResults results)
            {
                results.AppliedDamage *= 2;
            }
        }
        public class Effect_WildStrikes : Effect
        {
            public Effect_WildStrikes()
                : base(0)
            {
                this.EffectName = "Wild Strikes";
                this.EffectDescription = "Due to the Berserker's unruly nature, his ability to hit targets is reduced by 15%.";

                this.IsHarmful = false;
                this.IsImmuneToPurge = true;
                this.EffectType = EffectTypes.Physical;
            }

            public override void CalculateStats()
            {
                this.parent.PhysicalHitChance.ModValue -= 15;
            }
        }
        public class Effect_UnendingRage : Effect
        {
            private double bonusCrit = 0.0;
            private double bonusHit = 0.0;

            public Effect_UnendingRage()
                : base(0)
            {
                this.EffectName = "Unending Rage";
                this.EffectDescription = "The Berserker's blinding rage allows him to build up critical strike rating everytime he misses his attack or is critically struck.";

                this.IsHarmful = false;
                this.IsImmuneToPurge = true;
                this.EffectType = EffectTypes.Physical;
            }

            public override void OnAttack(CombatResults results)
            {
                if (results.DidMiss || results.DidAvoid)
                {
                    this.addCritBonus(5);
                }
                else if (results.DidCrit)
                {
                    this.bonusCrit = 0.0;
                    this.bonusHit = 0.0;
                }

                base.OnAttack(results);
            }

            public override void OnDefend(CombatResults results)
            {
                if (results.DidCrit)
                {
                    this.addCritBonus(5);
                }

                base.OnDefend(results);
            }

            public override void CalculateStats()
            {
                parent.PhysicalCritChance.ModValue += bonusCrit;
                parent.PhysicalHitChance.ModValue += bonusHit;

                base.CalculateStats();
            }

            private void addCritBonus(double amount)
            {
                if (parent.PhysicalCritChance.EffectiveValue >= 100.0)
                    bonusHit += amount;
                else
                    bonusCrit += amount;
            }
        }
    }
}
