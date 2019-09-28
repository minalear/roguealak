using System;
using Microsoft.Xna.Framework;
using Roguelike.Core.Entities;
using Roguelike.Core.Stats;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Core.Combat
{
    public abstract class Ability : ListItem
    {
        protected int abilityCost = 0;
        protected int range = 0; //0 - No Range, 1 - Melee range, 2+ Range in terms of cardinal tiles
        protected int currentCD = 0; //0 - Can use, 1+ CANNOT
        protected int cooldown = 0;

        protected AbilityTypes abilityType = AbilityTypes.Physical;
        public AbilityTypes AbilityType { get { return this.abilityType; } set { this.abilityType = value; } }

        public string AbilityName { get; set; }
        public string AbilityNameShort { get { return this.getShortName(); } set { this.abilityNameShort = value; } }
        public TargetingTypes TargetingType { get; set; }
        public int Range { get { return this.range; } set { this.range = value; } }

        public Ability() { }
        public Ability(int cost)
        {
            this.abilityCost = cost;
            this.TargetingType = TargetingTypes.EntityTarget;
        }

        protected CombatResults results;
        public abstract CombatResults CalculateResults(StatsPackage caster, StatsPackage target);
        public virtual CombatResults CastAbilityTarget(StatsPackage caster, StatsPackage target)
        {
            if (this.CanCastAbility(caster, target))
            {
                this.ApplyAbilityCost(caster);
                this.currentCD = this.cooldown;
                this.results = CalculateResults(caster, target);

                return results;
            }
            return new CombatResults() { Caster = caster, Target = target, UsedAbility = this, CanAfford = false };
        }
        public virtual void UpdateStep()
        {
            this.currentCD--;
            if (this.currentCD < 0)
                this.currentCD = 0;
        }

        public virtual string GetDescription()
        {
            string description = this.AbilityName + " - " + this.abilityType.ToString() + "\n";
            description += "MP: " + this.abilityCost + " - Range: " + this.range + "\n";
            description += "Target: " + this.TargetingType.ToString();

            return description;
        }

        public CombatResults DoesAttackHit(StatsPackage caster, StatsPackage target)
        {
            CombatResults results = new CombatResults() { DidMiss = true, DidAvoid = false, Caster = caster, Target = target, UsedAbility = this };

            int result = RNG.Next(0, 100);
            if (this.abilityType == AbilityTypes.Physical)
            {
                if (result <= caster.PhysicalHitChance.EffectiveValue)
                {
                    results.DidMiss = false;

                    result = RNG.Next(0, 100);
                    if (result <= target.PhysicalAvoidance.EffectiveValue)
                        results.DidAvoid = true;
                    else
                        results.DidAvoid = false;
                }
            }
            else if (this.abilityType == AbilityTypes.Magical)
            {
                if (result <= caster.SpellHitChance.EffectiveValue)
                {
                    results.DidMiss = false;

                    result = RNG.Next(0, 100);
                    if (result <= target.SpellAvoidance.EffectiveValue)
                        results.DidAvoid = true;
                    else
                        results.DidAvoid = false;
                }
            }

            return results;
        }
        public bool DoesAttackCrit(StatsPackage caster)
        {
            int result = RNG.Next(0, 100);
            if (this.abilityType == AbilityTypes.Physical)
            {
                if (result <= caster.PhysicalCritChance.EffectiveValue)
                    return true;
            }
            else if (this.abilityType == AbilityTypes.Magical)
            {
                if (result <= caster.SpellCritChance.EffectiveValue)
                    return true;
            }

            return false;
        }
        public int ApplyCriticalDamage(int damage, StatsPackage caster)
        {
            if (this.abilityType == AbilityTypes.Physical)
                return (int)(damage * caster.PhysicalCritPower.EffectiveValue);

            return (int)(damage * caster.SpellCritPower.EffectiveValue);
        }
        public int CalculateAbsorption(int damage, StatsPackage target)
        {
            if (this.abilityType == AbilityTypes.Physical)
                return (int)(target.PhysicalReduction.EffectiveValue / 100 * damage);
            return (int)(target.SpellReduction.EffectiveValue / 100 * damage);
        }
        public int CalculateReflectedDamage(int damage, StatsPackage target)
        {
            if (this.abilityType == AbilityTypes.Physical)
                return (int)(damage * (target.PhysicalReflection.EffectiveValue / 100));
            return (int)(damage * (target.SpellReflection.EffectiveValue / 100));
        }
        public int CalculateHealthLeech(int damage, StatsPackage caster)
        {
            if (this.abilityType == AbilityTypes.Magical)
                return (int)(damage * (caster.HPLeechSpell / 100));
            return (int)(damage * (caster.HPLeechPhysical / 100));
        }
        public int CalculateManaLeech(int damage, StatsPackage caster)
        {
            if (this.abilityType == AbilityTypes.Magical)
                return (int)(damage * (caster.MPLeechSpell / 100));
            return (int)(damage * (caster.MPLeechPhysical / 100));
        }

        public virtual bool CanCastAbility(StatsPackage caster, StatsPackage target)
        {
            if (!CanAffordAbility(caster) && !IsOffCooldown())
                return false;
            if (!IsLineOfSight(caster, new Point(target.ParentEntity.X, target.ParentEntity.Y)))
                return false;

            if (range != 0) //Not infinite range
            {
                //Check range
                int distance = (int)Vector2.Distance(new Vector2(caster.ParentEntity.X, caster.ParentEntity.Y), new Vector2(target.ParentEntity.X, target.ParentEntity.Y));
                if (distance > this.range)
                    return false;
            }

            return true;
        }
        public virtual bool CanCastAbility(StatsPackage caster, int x1, int y1)
        {
            if (caster.Mana < this.abilityCost || this.currentCD > 0)
                return false;
            if (!caster.ParentEntity.ParentLevel.IsLineOfSight(new Point(caster.ParentEntity.X, caster.ParentEntity.Y), new Point(x1, y1)))
                return false;

            if (range != 0) //Not infinite range
            {
                //Check range
                int distance = (int)Vector2.Distance(new Vector2(caster.ParentEntity.X, caster.ParentEntity.Y), new Vector2(x1, y1));
                if (distance > this.range)
                    return false;
            }

            return true;
        }
        public virtual void CastAbilityGround(StatsPackage caster, int x0, int y0, int radius, Level level)
        {
            if (this.CanCastAbility(caster, x0, y0))
            {
                this.ApplyAbilityCost(caster);
                this.currentCD = this.cooldown;

                if (radius > 0)
                {
                    for (int angle = 0; angle < 360; angle += 1)
                    {
                        for (int r = 0; r < radius; r++)
                        {
                            int x = (int)(x0 + 0.5 + r * Math.Cos(angle));
                            int y = (int)(y0 + 0.5 + r * Math.Sin(angle));

                            Entity entity = level.GetEntity(x, y);
                            if (entity != null)
                                entity.Attack(caster.ParentEntity, this);
                        }
                    }
                }
                else
                {
                    Entity entity = level.GetEntity(x0, y0);
                    if (entity != null)
                        entity.Attack(caster.ParentEntity, this);
                }
            }
        }

        public virtual bool CanAffordAbility(StatsPackage caster)
        {
            return (caster.Mana > this.abilityCost);
        }
        public virtual void ApplyAbilityCost(StatsPackage caster)
        {
            caster.DrainMana(this.abilityCost);
        }
        public virtual bool IsOffCooldown()
        {
            return (this.currentCD == 0);
        }
        public virtual bool IsLineOfSight(StatsPackage caster, Point target)
        {
            return (caster.ParentEntity.ParentLevel.IsLineOfSight(new Point(caster.ParentEntity.X, caster.ParentEntity.Y), new Point(target.X, target.Y)));
        }

        public override string ListText
        {
            get
            {
                return this.AbilityName;
            }
            set
            {
                base.ListText = value;
            }
        }

        private string abilityNameShort;
        private string getShortName()
        {
            if (this.currentCD == 0)
                return this.abilityNameShort;
            else
                return "CD: " + this.currentCD;
        }
    }

    public enum AbilityTypes { Physical, Magical }
    public enum TargetingTypes { EntityTarget, GroundTarget, Self }
}
