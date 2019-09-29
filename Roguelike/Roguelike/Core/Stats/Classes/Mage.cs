using System;
using System.Collections.Generic;
using Roguelike.Core.Combat;
using Roguelike.Engine;

namespace Roguelike.Core.Stats.Classes
{
    public class Mage : Class
    {
        public Mage()
            : base("Magician")
        {
            Description = "Students gifted with a keen intellect and unwavering discipline may walk the path of the mage. The arcane magic available to magi is both great and dangerous, and thus is revealed only to the most devoted practitioners. To avoid interference with their spellcasting, magi wear only cloth armor, but arcane shields and enchantments give them additional protection. To keep enemies at bay, magi can summon bursts of fire to incinerate distant targets and cause entire areas to erupt, setting groups of foes ablaze.";
            InheritAbilities = new List<Ability>() { new Ability_Fireball(), new Ability_Frostbolt(), new Ability_IceBlock() };
            ClassTraits = new List<Effect>() { new Trait_IncreasedFortitude(), new Trait_SpellSword() };
        }
        public override PlayerStats CalculateStats(PlayerStats stats)
        {
            stats.Strength -= 1;
            stats.Agility -= 1;
            stats.Dexterity -= 1;

            stats.Intelligence += 3;
            stats.Willpower += 2;
            stats.Wisdom += 1;

            stats.Constitution += 0;
            stats.Endurance += 0;
            stats.Fortitude += 1;

            return base.CalculateStats(stats);
        }

        public class Ability_IceBlock : Ability
        {
            public Ability_IceBlock()
                : base()
            {
                AbilityName = "Ice Block";
                AbilityNameShort = "Ice Blok";

                abilityType = AbilityTypes.Magical;
                TargetingType = TargetingTypes.Self;
                abilityCost = 35;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                for (int i = 0; i < target.AppliedEffects.Count; i++)
                {
                    if (target.AppliedEffects[i].IsHarmful)
                    {
                        target.PurgeEffect(target.AppliedEffects[i]);
                    }
                }

                target.AddHealth((int)(target.MaxHealth * 0.25));
                target.ApplyEffect(new Effect_IceBlock(target));
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }
        }
        public class Ability_Frostbolt : Ability
        {
            public Ability_Frostbolt()
                : base()
            {
                AbilityName = "Frostbolt";
                AbilityNameShort = "Frstbolt";

                abilityType = AbilityTypes.Magical;
                TargetingType = TargetingTypes.EntityTarget;
                abilityCost = 25;
                Range = 15;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                CombatResults results = DoesAttackHit(caster, target);

                if (!results.DidMiss && !results.DidAvoid)
                {
                    int damage = (int)(caster.SpellPower.EffectiveValue * 0.75);
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

                if (!target.HasEffect("Chilled"))
                {
                    target.ApplyEffect(new Effect_Frostbolt(target));
                }

                return results;
            }
        }
        public class Ability_Fireball : Ability
        {
            public Ability_Fireball()
                : base()
            {
                AbilityName = "Fireball";
                AbilityNameShort = "Freball";

                TargetingType = TargetingTypes.EntityTarget;
                abilityType = AbilityTypes.Magical;
                abilityCost = 55;
                Range = 15;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                CombatResults results = DoesAttackHit(caster, target);

                if (!results.DidMiss && !results.DidAvoid)
                {
                    int damage = (int)(caster.SpellPower.EffectiveValue * 1.5);
                    if (DoesAttackCrit(caster))
                    {
                        damage = ApplyCriticalDamage(damage, caster);
                        results.DidCrit = true;
                    }

                    results.PureDamage = damage;
                    results.AbsorbedDamage = CalculateAbsorption(damage, target);
                    results.AppliedDamage = results.PureDamage - results.AbsorbedDamage;
                    results.ReflectedDamage = CalculateReflectedDamage(results.AppliedDamage, target);

                    if (!target.HasEffect("Ignite"))
                    {
                        int result = RNG.Next(0, 100);
                        if (result <= 20)
                            target.ApplyEffect(new Effect_FireballDOT(target));
                    }
                }

                return results;
            }
        }

        public class Effect_FireballDOT : Effect
        {
            int damage = 5;

            public Effect_FireballDOT(StatsPackage package)
                : base(package, 15)
            {
                EffectName = "Ignite";
                IsHarmful = true;

                EffectType = EffectTypes.Magical;
                EffectDescription = "You have been set aflame!!!";
            }

            public override void UpdateStep()
            {
                //parent.DrainHealth(damage);
                parent.DealDOTDamage(damage, this);

                base.UpdateStep();
            }
        }
        public class Effect_Frostbolt : Effect
        {
            public Effect_Frostbolt(StatsPackage package)
                : base(package, 5)
            {
                EffectName = "Chilled";
                IsHarmful = true;

                EffectDescription = "You feel extremely cold, making it harder to resist damage.";
            }

            public override void CalculateStats()
            {
                parent.SpellReduction.ModValue -= parent.SpellReduction.EffectiveValue * 0.2;

                base.CalculateStats();
            }
        }
        public class Effect_IceBlock : Effect
        {
            public Effect_IceBlock(StatsPackage parent)
                : base(parent, 5)
            {
                EffectName = "Ice Block";
                IsHarmful = false;

                EffectDescription = "A thin Ice barrier surrounds you preventing damage being dealt to you for a small amount of time.";
            }

            public override int OnHealthLoss(int amount)
            {
                return 0;
            }
        }

        public class Trait_IncreasedFortitude : Effect
        {
            public Trait_IncreasedFortitude()
                : base(0)
            {
                EffectName = "Increased Fortitude";
                IsHarmful = false;

                IsImmuneToPurge = true;
                EffectType = EffectTypes.Trait;
                EffectDescription = "Your mind has been deepend and your magic is more powerful.";
            }

            public override void CalculateStats()
            {
                parent.SpellPower.ModValue += parent.SpellPower.BaseValue * 0.10;

                base.CalculateStats();
            }
        }
        public class Trait_SpellSword : Effect
        {
            public Trait_SpellSword()
                : base(0)
            {
                EffectName = "Sword of Azzinoth";
                IsHarmful = false;

                IsImmuneToPurge = true;
                EffectType = EffectTypes.Trait;
                EffectDescription = "You have dedicated your life to the teachings of Azzinoth and became a spellsword.  Your attack power is boosted by your spell power.";
            }

            public override void CalculateStats()
            {
                parent.AttackPower.ModValue += parent.SpellPower.BaseValue * 0.50;

                base.CalculateStats();
            }
        }
    }
}
