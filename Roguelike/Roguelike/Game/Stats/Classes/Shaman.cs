using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Engine.Game.Combat;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Engine.Game.Stats.Classes
{
    public class Shaman : Class
    {
        public Shaman()
            : base("Shaman")
        {
            this.Description = "Shaman are spiritual guides and practitioners, not of the divine, but of the very elements. Unlike some other mystics, shaman commune with forces that are not strictly benevolent. The elements are chaotic, and left to their own devices, they rage against one another in unending primal fury. It is the call of the shaman to bring balance to this chaos. Acting as moderators among earth, fire, water, and air, shaman summon totems that focus the elements to support the shaman’s allies or punish those who threaten them.";
            this.InheritAbilities = new List<Ability>() { new Ability_WindShear(), new Ability_FireLash() };
            this.InheritEffects = new List<Effect>() { new Effect_SpiritUprising() };
        }
        public override PlayerStats CalculateStats(PlayerStats stats)
        {
            stats.Strength -= 1;
            stats.Agility += 2;
            stats.Dexterity += 0;

            stats.Intelligence += 2;
            stats.Willpower += 0;
            stats.Wisdom += 1;

            stats.Constitution += 1;
            stats.Endurance -= 1;
            stats.Fortitude += 0;

            return base.CalculateStats(stats);
        }

        public class Ability_WindShear : Ability
        {
            public Ability_WindShear()
                : base()
            {
                this.AbilityName = "Wind Shear";
                this.AbilityNameShort = "Wnd Shr";

                this.TargetingType = TargetingTypes.EntityTarget;
                this.AbilityType = AbilityTypes.Magical;
                this.abilityCost = 15;
                this.Range = 15;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                if (!target.HasEffect(typeof(Effect_WindShear)))
                    target.ApplyEffect(new Effect_WindShear());

                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }
        }
        public class Ability_FireLash : Ability
        {
            public Ability_FireLash()
                : base()
            {
                this.AbilityName = "Fire Lash";
                this.AbilityNameShort = "Fre Lsh";

                this.TargetingType = TargetingTypes.EntityTarget;
                this.AbilityType = AbilityTypes.Physical;
                this.abilityCost = 15;
                this.Range = 15;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                CombatResults results = this.DoesAttackHit(caster, target);

                if (!results.DidMiss && !results.DidAvoid)
                {
                    int damage = (int)((caster.SpellPower.EffectiveValue * 0.5) + (caster.AttackPower.EffectiveValue * 0.5));
                    if (this.DoesAttackCrit(caster))
                    {
                        damage = this.ApplyCriticalDamage(damage, caster);
                        results.DidCrit = true;
                    }

                    results.PureDamage = damage;
                    results.AbsorbedDamage = this.CalculateAbsorption(damage, target);
                    results.AppliedDamage = results.PureDamage - results.AbsorbedDamage;
                    results.ReflectedDamage = this.CalculateReflectedDamage(results.AppliedDamage, target);

                    if (!target.HasEffect(typeof(Effect_FireLashDOT)))
                    {
                        target.ApplyEffect(new Effect_FireLashDOT());
                    }
                }

                return results;
            }
        }

        public class Effect_FireLashDOT : Effect
        {
            int damage = 5;

            public Effect_FireLashDOT()
                : base(10)
            {
                this.EffectName = "Fire Lash";
                this.IsHarmful = true;

                this.EffectType = EffectTypes.Hybrid;
                this.EffectDescription = "The Fire elemental's spirit is lashing at you from the elemental plane.";
            }

            public override void UpdateStep()
            {
                //this.parent.DrainHealth(this.damage);
                this.parent.DealDOTDamage(this.damage, this);

                base.UpdateStep();
            }
        }
        public class Effect_SpiritUprising : Effect
        {
            public Effect_SpiritUprising()
                : base(0)
            {
                this.EffectName = "Spiritual Uprising";
                this.IsHarmful = false;
                this.IsImmuneToPurge = true;

                this.EffectDescription = "The Shaman's natural communion with the spirits helps during combat.";
            }

            public override void CalculateStats()
            {
                this.parent.SpellPower.ModValue += this.parent.SpellPower.BaseValue * 0.15;
                this.parent.AttackPower.ModValue += this.parent.AttackPower.BaseValue * 0.15;

                base.CalculateStats();
            }
        }
        public class Effect_WindShear : Effect
        {
            public Effect_WindShear()
                : base(0)
            {
                this.EffectName = "Wind Shear";
                this.IsHarmful = true;

                this.EffectDescription = "Your flesh is torn from intense winds and you feel weakened as a result.";
            }

            public override void CalculateStats()
            {
                this.parent.SpellPower.ModValue -= this.parent.SpellPower.EffectiveValue * 0.5;

                base.CalculateStats();
            }
        }
    }
}
