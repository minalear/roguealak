using System;
using System.Collections.Generic;
using Roguelike.Core.Entities;
using Roguelike.Core.Entities.Dynamic;
using Roguelike.Core.Combat;
using Roguelike.Engine;

namespace Roguelike.Core.Stats.Classes
{
    public class Warlock : Class
    {
        public Warlock()
            : base("Warlock")
        {
            Description = "In the face of demonic power, most heroes see death. Warlocks see only opportunity. Dominance is their aim, and they have found a path to it in the dark arts. These voracious spellcasters summon demonic minions to fight beside them. At first, they command only the service of imps, but as a warlock’s knowledge grows, seductive succubi, loyal voidwalkers, and horrific felhunters join the dark sorcerer’s ranks to wreak havoc on anyone who stands in their master’s way.";
            InheritAbilities = new List<Ability>() { new Ability_ChaosBolt(), new Ability_DemonBreath(), new Ability_Havoc(), new Ability_WeakenSoul() };
        }
        public override PlayerStats CalculateStats(PlayerStats stats)
        {
            stats.Strength += 0;
            stats.Agility += 0;
            stats.Dexterity += 0;

            stats.Intelligence += 4;
            stats.Willpower += 1;
            stats.Wisdom += 0;

            stats.Constitution += -1;
            stats.Endurance += -1;
            stats.Fortitude += 1;
            
            return base.CalculateStats(stats);
        }

        public class Ability_ChaosBolt : Ability
        {
            public Ability_ChaosBolt()
                : base()
            {
                AbilityName = "Chaos Bolt";
                AbilityNameShort = "Chs Blt";

                TargetingType = TargetingTypes.EntityTarget;
                abilityType = AbilityTypes.Magical;
                abilityCost = 35;
                Range = 15;

                cooldown = 5;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                CombatResults results = DoesAttackHit(caster, target);

                if (!results.DidMiss && !results.DidAvoid)
                {
                    int damage = (int)(caster.SpellPower.EffectiveValue * 0.9);
                    if (DoesAttackCrit(caster))
                    {
                        damage = ApplyCriticalDamage(damage, caster);
                        results.DidCrit = true;
                    }

                    results.PureDamage = damage;
                    results.AbsorbedDamage = CalculateAbsorption(damage, target);
                    results.AppliedDamage = results.PureDamage - results.AbsorbedDamage;
                    results.ReflectedDamage = CalculateReflectedDamage(results.AppliedDamage, target);

                    target.ApplyEffect(new Effect_ChaosDOT(target));
                }

                return results;
            }
        }
        public class Ability_DemonBreath : Ability
        {
            public Ability_DemonBreath()
                : base()
            {
                AbilityName = "Demon's Breath";
                AbilityNameShort = "Demn Brth";

                TargetingType = TargetingTypes.GroundTarget;
                abilityType = AbilityTypes.Magical;
                abilityCost = 15;
                range = 5;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                target.ApplyEffect(new Effect_HavocDOT(target));
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }

            public override void CastAbilityGround(StatsPackage caster, int x0, int y0, int radius, Level level)
            {
                if (CanCastAbility(caster, x0, y0))
                {
                    Point delta = new Point(x0 - caster.ParentEntity.X, y0 - caster.ParentEntity.Y);
                    FlameBreath breath = new FlameBreath(level, new Point(caster.ParentEntity.X + delta.X, caster.ParentEntity.Y + delta.Y), delta);

                    level.Entities.Add(breath);
                }
            }

            public override CombatResults CastAbilityTarget(StatsPackage caster, StatsPackage target)
            {
                target.ApplyEffect(new Effect_HavocDOT(target));
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }
        }
        public class Ability_Havoc : Ability
        {
            public Ability_Havoc()
                : base()
            {
                AbilityName = "Havoc";
                AbilityNameShort = "Havoc";

                TargetingType = TargetingTypes.EntityTarget;
                AbilityType = AbilityTypes.Magical;
                abilityCost = 15;
                Range = 15;
            }

            public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
            {
                if (!target.HasEffect(typeof(Effect_HavocDOT)))
                    target.ApplyEffect(new Effect_HavocDOT(target));

                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }
        }
        public class Ability_WeakenSoul : Ability
        {
            public Ability_WeakenSoul()
                : base()
            {
                AbilityName = "Weaken Soul";
                AbilityNameShort = "Wkn Soul";

                TargetingType = TargetingTypes.EntityTarget;
                AbilityType = AbilityTypes.Magical;
                abilityCost = 10;
                Range = 15;
            }

            public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
            {
                if (!target.HasEffect(typeof(Effect_Weaken)))
                    target.ApplyEffect(new Effect_Weaken(target));

                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }
        }

        public class Effect_ChaosDOT : Effect
        {
            int damage = 2;

            public Effect_ChaosDOT(StatsPackage package)
                : base(package, 10)
            {
                EffectName = "Chaos Burning";
                IsHarmful = true;

                EffectType = EffectTypes.Magical;
                EffectDescription = "Chaos is warping your mind, causing you take periodical damage.";
            }

            public override void UpdateStep()
            {
                //parent.DrainHealth(damage);
                parent.DealDOTDamage(damage, this);

                base.UpdateStep();
            }
        }
        public class Effect_HavocDOT : Effect
        {
            int damage = 10;

            public Effect_HavocDOT(StatsPackage package)
                : base(package, 10)
            {
                EffectName = "Havoc";
                IsHarmful = true;

                EffectType = EffectTypes.Magical;
                EffectDescription = "The shadows assault you from all sides.";
            }

            public override void UpdateStep()
            {
                //parent.DrainHealth(damage);
                parent.DealDOTDamage(damage, this);

                base.UpdateStep();
            }
        }
        public class Effect_Weaken : Effect
        {
            public Effect_Weaken(StatsPackage package)
                : base(package, 0)
            {
                EffectName = "Weakened Soul";
                IsHarmful = true;

                EffectDescription = "Shadows have placed a grip upon your soul!  You feel much weaker.";
            }

            public override void OnApplication(Entity entity)
            {
                for (int i = 0; i < parent.AppliedEffects.Count; i++)
                {
                    if (!parent.AppliedEffects[i].IsHarmful)
                        parent.AppliedEffects[i].OnRemoval();
                }

                base.OnApplication(entity);
            }

            public override void CalculateStats()
            {
                parent.SpellReduction.ModValue -= parent.SpellPower.BaseValue * 0.25;

                base.CalculateStats();
            }
        }
    }
}
