using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Engine.Game.Combat;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Engine.Game.Stats.Classes
{
    public class Rogue : Class
    {
        public Rogue()
            : base("Rogue")
        {
            this.Description = "For rogues, the only code is the contract, and their honor is purchased in gold. Free from the constraints of a conscience, these mercenaries rely on brutal and efficient tactics. Lethal assassins and masters of stealth, they will approach their marks from behind, piercing a vital organ and vanishing into the shadows before the victim hits the ground.";
            this.BasicAttack = new Ability_PoisonStrike();
            this.InheritAbilities = new List<Ability>() { new Ability_ApplyPoison(), new Ability_Evasion() };
            this.InheritEffects = new List<Effect>() { new Effect_CounterStrike() };
        }
        public override PlayerStats CalculateStats(PlayerStats stats)
        {
            stats.Strength += 0;
            stats.Agility += 3;
            stats.Dexterity += 2;

            stats.Intelligence += 0;
            stats.Willpower += 0;
            stats.Wisdom -= 2;

            stats.Constitution += 0;
            stats.Endurance += 1;
            stats.Fortitude += 0;

            return base.CalculateStats(stats);
        }

        public class Ability_PoisonStrike : Ability
        {
            public Ability_PoisonStrike()
                : base()
            {
                this.AbilityName = "Poison Strike";
                this.AbilityNameShort = "Psn Strk";

                this.abilityType = AbilityTypes.Physical;
                this.TargetingType = TargetingTypes.EntityTarget;
                this.abilityCost = 0;
                this.Range = 15;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
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

                    int result = RNG.Next(0, 100);
                    if (result <= 15)
                    {
                        if (!target.HasEffect(typeof(Effect_Poison)))
                            target.ApplyEffect(new Effect_Poison());
                        else
                        {
                            Effect_Poison poison = (Effect_Poison)target.GetEffect(typeof(Effect_Poison));
                            if (poison.stacks < 5)
                            {
                                poison.stacks++;
                                poison.Duration = 10;
                            }
                        }
                    }
                }

                return results;
            }
        }
        public class Ability_Evasion : Ability
        {
            public Ability_Evasion()
                : base()
            {
                this.AbilityName = "Evasion";
                this.AbilityNameShort = "Evsion";

                this.abilityType = AbilityTypes.Physical;
                this.TargetingType = TargetingTypes.Self;
                this.abilityCost = 10;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                caster.ApplyEffect(new Effect_Evasion());
                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }
        }
        public class Ability_CounterStrike : Ability
        {
            public Ability_CounterStrike()
                : base()
            {
                this.AbilityName = "Counter-Strike";
                this.AbilityNameShort = "Ctr Strk";

                this.abilityType = AbilityTypes.Physical;
                this.TargetingType = TargetingTypes.EntityTarget;
            }

            public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
            {
                CombatResults results = this.DoesAttackHit(caster, target);

                if (!results.DidMiss && !results.DidAvoid)
                {
                    int damage = (int)(caster.AttackPower.EffectiveValue * 0.4);

                    results.PureDamage = damage;
                    results.AbsorbedDamage = this.CalculateAbsorption(results.PureDamage, target);
                    results.AppliedDamage = results.PureDamage - results.AbsorbedDamage;
                    results.ReflectedDamage = this.CalculateReflectedDamage(results.AppliedDamage, target);
                }

                return results;
            }
        }
        public class Ability_ApplyPoison : Ability
        {
            public Ability_ApplyPoison()
                : base()
            {
                this.AbilityName = "Apply Poison";
                this.AbilityNameShort = "Aply Psn";

                this.TargetingType = TargetingTypes.EntityTarget;
                this.AbilityType = AbilityTypes.Physical;
                this.abilityCost = 10;
                this.Range = 15;
            }

            public override CombatResults CalculateResults(Stats.StatsPackage caster, Stats.StatsPackage target)
            {
                if (!target.HasEffect(typeof(Effect_Poison)))
                    target.ApplyEffect(new Effect_Poison());
                else
                {
                    Effect_Poison poison = (Effect_Poison)target.GetEffect(typeof(Effect_Poison));
                    if (poison.stacks < 5)
                    {
                        poison.stacks++;
                        poison.Duration = 10;
                    }
                }

                return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
            }
        }

        public class Effect_Poison : Effect
        {
            public int stacks = 1;
            private int baseDamage = 5;

            public Effect_Poison()
                : base(8)
            {
                this.EffectName = "Poison x 1";
                this.IsHarmful = true;

                this.EffectType = EffectTypes.Physical;
                this.EffectDescription = "Poison is being built up in your body!  Don't let it get too high.";
            }

            public override void UpdateStep()
            {
                this.EffectName = "Poison x " + this.stacks.ToString();
                //this.parent.DrainHealth(this.baseDamage * this.stacks);
                this.parent.DealDOTDamage(this.baseDamage * this.stacks, this);

                base.UpdateStep();
            }
        }
        public class Effect_Evasion : Effect
        {
            public Effect_Evasion()
                : base(4)
            {
                this.EffectName = "Evasion";
                this.IsHarmful = false;

                this.EffectDescription = "Your natural agility is allowing you to dodge more incoming attacks.";
            }

            public override void CalculateStats()
            {
                this.parent.PhysicalAvoidance.ModValue += 50;
                this.parent.SpellAvoidance.ModValue += 25;

                base.CalculateStats();
            }
        }
        public class Effect_CounterStrike : Effect
        {
            public Effect_CounterStrike()
                : base(0)
            {
                this.EffectName = "Counter-Strike";
                this.IsHarmful = false;

                this.EffectDescription = "You will counter-attack twice whenever an enemy critically strikes you.";
            }

            public override void OnAttack(CombatResults results)
            {
                base.OnAttack(results);
            }

            public override void OnDefend(CombatResults results)
            {
                if (results.DidCrit)
                {
                    CombatManager.PerformAbility(results.Target, results.Caster, new Ability_CounterStrike());
                    CombatManager.PerformAbility(results.Target, results.Caster, new Ability_CounterStrike());
                }

                base.OnDefend(results);
            }
        }
    }
}
