using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.Game.Entities.Dynamic;
using Roguelike.Engine.Game.Combat;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Engine.Game.Stats.Classes
{
    public class Warlock : Class
    {
        public Warlock()
            : base("Warlock")
        {
            this.Description = "In the face of demonic power, most heroes see death. Warlocks see only opportunity. Dominance is their aim, and they have found a path to it in the dark arts. These voracious spellcasters summon demonic minions to fight beside them. At first, they command only the service of imps, but as a warlock’s knowledge grows, seductive succubi, loyal voidwalkers, and horrific felhunters join the dark sorcerer’s ranks to wreak havoc on anyone who stands in their master’s way.";
            this.InheritAbilities = new List<Ability>() { new Ability_ChaosBolt(), new Ability_DemonBreath(), new Ability_Havoc(), new Ability_WeakenSoul() };
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
                this.AbilityName = "Chaos Bolt";
                this.AbilityNameShort = "Chs Blt";

                this.TargetingType = TargetingTypes.EntityTarget;
                this.abilityType = AbilityTypes.Magical;
                this.abilityCost = 35;
                this.Range = 15;

                this.cooldown = 5;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                CombatResults results = this.DoesAttackHit(caster, target);

                if (!results.DidMiss && !results.DidAvoid)
                {
                    int damage = (int)(caster.SpellPower.EffectiveValue * 0.9);
                    if (this.DoesAttackCrit(caster))
                    {
                        damage = this.ApplyCriticalDamage(damage, caster);
                        results.DidCrit = true;
                    }

                    results.PureDamage = damage;
                    results.AbsorbedDamage = this.CalculateAbsorption(damage, target);
                    results.AppliedDamage = results.PureDamage - results.AbsorbedDamage;
                    results.ReflectedDamage = this.CalculateReflectedDamage(results.AppliedDamage, target);

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
                this.AbilityName = "Demon's Breath";
                this.AbilityNameShort = "Demn Brth";

                this.TargetingType = TargetingTypes.GroundTarget;
                this.abilityType = AbilityTypes.Magical;
                this.abilityCost = 15;
                this.range = 5;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                target.ApplyEffect(new Effect_HavocDOT(target));
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }

            public override void CastAbilityGround(StatsPackage caster, int x0, int y0, int radius, Level level)
            {
                if (this.CanCastAbility(caster, x0, y0))
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
                this.AbilityName = "Havoc";
                this.AbilityNameShort = "Havoc";

                this.TargetingType = TargetingTypes.EntityTarget;
                this.AbilityType = AbilityTypes.Magical;
                this.abilityCost = 15;
                this.Range = 15;
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
                this.AbilityName = "Weaken Soul";
                this.AbilityNameShort = "Wkn Soul";

                this.TargetingType = TargetingTypes.EntityTarget;
                this.AbilityType = AbilityTypes.Magical;
                this.abilityCost = 10;
                this.Range = 15;
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
                this.EffectName = "Chaos Burning";
                this.IsHarmful = true;

                this.EffectType = EffectTypes.Magical;
                this.EffectDescription = "Chaos is warping your mind, causing you take periodical damage.";
            }

            public override void UpdateStep()
            {
                //this.parent.DrainHealth(this.damage);
                this.parent.DealDOTDamage(this.damage, this);

                base.UpdateStep();
            }
        }
        public class Effect_HavocDOT : Effect
        {
            int damage = 10;

            public Effect_HavocDOT(StatsPackage package)
                : base(package, 10)
            {
                this.EffectName = "Havoc";
                this.IsHarmful = true;

                this.EffectType = EffectTypes.Magical;
                this.EffectDescription = "The shadows assault you from all sides.";
            }

            public override void UpdateStep()
            {
                //this.parent.DrainHealth(this.damage);
                this.parent.DealDOTDamage(this.damage, this);

                base.UpdateStep();
            }
        }
        public class Effect_Weaken : Effect
        {
            public Effect_Weaken(StatsPackage package)
                : base(package, 0)
            {
                this.EffectName = "Weakened Soul";
                this.IsHarmful = true;

                this.EffectDescription = "Shadows have placed a grip upon your soul!  You feel much weaker.";
            }

            public override void OnApplication()
            {
                for (int i = 0; i < this.parent.AppliedEffects.Count; i++)
                {
                    if (!this.parent.AppliedEffects[i].IsHarmful)
                        this.parent.AppliedEffects[i].OnRemoval();
                }

                base.OnApplication();
            }

            public override void CalculateStats()
            {
                this.parent.SpellReduction.ModValue -= this.parent.SpellPower.BaseValue * 0.25;

                base.CalculateStats();
            }
        }
    }
}
