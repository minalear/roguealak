using System;
using System.Collections.Generic;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.Game.Combat;

namespace Roguelike.Engine.Game.Stats
{
    public class StatsPackage
    {
        public StatsPackage()
        {
            this.appliedEffects = new List<Effect>();
            this.abilityList = new List<Ability>();
        }
        public StatsPackage(Entity parent)
        {
            this.appliedEffects = new List<Effect>();
            this.abilityList = new List<Ability>();

            this.abilityList.Add(new Combat.Abilities.BasicAttack());
            this.parent = parent;
        }

        public bool IsDead()
        {
            return (this.health <= 0);
        }
        public virtual string GetFormattedName()
        {
            return this.UnitName;
        }

        public virtual void UpdateStep()
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
            {
                this.appliedEffects[i].UpdateStep();

                if (this.appliedEffects[i].DoPurge)
                {
                    this.appliedEffects.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < this.abilityList.Count; i++)
            {
                this.abilityList[i].UpdateStep();
            }

            if (!this.IsDead())
            {
                this.AddHealth(this.hpPerTurn);
                this.AddMana(this.mpPerTurn);
            }
            else if (this.IsDead())
            {
                this.OnDeath();
            }

            this.CalculateStats();
        }
        public virtual void ApplyEffect(Effect effect)
        {
            effect.Parent = this;

            this.appliedEffects.Add(effect);
            effect.OnApplication(this.ParentEntity);
        }

        public virtual void OnAttack(CombatResults results)
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
                this.appliedEffects[i].OnAttack(results);
        }
        public virtual void OnDefend(CombatResults results)
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
                this.appliedEffects[i].OnDefend(results);
        }

        public virtual void OnDeath()
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
                this.appliedEffects[i].OnDeath();
            this.ParentEntity.OnDeath();
        }
        public virtual void OnSpawn()
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
                this.appliedEffects[i].OnSpawn();
        }
        public virtual void OnMove()
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
                this.appliedEffects[i].OnMove();
        }

        public virtual void AddHealth(int amount)
        {
            if (amount != 0)
            {
                for (int i = 0; i < this.appliedEffects.Count; i++)
                    amount = this.appliedEffects[i].OnHealthGain(amount);
                this.health += amount;

                if (this.health > this.maxHealth)
                    this.health = (int)maxHealth.EffectiveValue;
            }
        }
        public virtual void DrainHealth(int amount)
        {
            if (amount != 0 && !this.isImmune)
            {
                for (int i = 0; i < this.appliedEffects.Count; i++)
                    amount = this.appliedEffects[i].OnHealthLoss(amount);
                this.health -= amount;

                if (this.health < 0)
                    this.health = 0;
            }
        }
        public virtual void DealDOTDamage(int amount, Effect effect)
        {
            if (!this.isImmune)
            {
                int absorption = 0;
                if (effect.EffectType == EffectTypes.Magical)
                {
                    absorption = (int)(this.spellReduction.EffectiveValue / 100 * amount);
                }
                else if (effect.EffectType == EffectTypes.Physical)
                {
                    absorption = (int)(this.physicalReduction.EffectiveValue / 100 * amount);
                }
                else if (effect.EffectType == EffectTypes.Hybrid)
                {
                    double absorbPercent = ((this.spellReduction / 2) + (this.physicalReduction / 2)) / 100;
                    absorption = (int)(absorbPercent * amount);
                }

                this.DrainHealth(amount - absorption);
            }
        }

        public virtual void AddMana(int amount)
        {
            if (amount != 0)
            {
                for (int i = 0; i < this.appliedEffects.Count; i++)
                    amount = this.appliedEffects[i].OnManaGain(amount);
                this.mana += amount;

                if (this.mana > this.maxMana)
                    this.mana = (int)maxMana.EffectiveValue;
            }
        }
        public virtual void DrainMana(int amount)
        {
            if (amount != 0)
            {
                for (int i = 0; i < this.appliedEffects.Count; i++)
                    amount = this.appliedEffects[i].OnManaLoss(amount);
                this.mana -= amount;

                if (this.mana < 0)
                    this.mana = 0;
            }
        }

        public bool HasEffect(string effect)
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
            {
                if (this.appliedEffects[i].EffectName == effect)
                    return true;
            }

            return false;
        }
        public bool HasEffect(Type type)
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
            {
                if (this.appliedEffects[i].GetType() == type)
                    return true;
            }

            return false;
        }
        public Effect GetEffect(string effect)
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
            {
                if (this.appliedEffects[i].EffectName == effect)
                    return this.appliedEffects[i];
            }

            return null;
        }
        public Effect GetEffect(Type type)
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
            {
                if (this.appliedEffects[i].GetType() == type)
                    return this.appliedEffects[i];
            }

            return null;
        }
        public void RemoveEffect(string effectName)
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
            {
                if (this.appliedEffects[i].EffectName == effectName)
                {
                    this.appliedEffects.RemoveAt(i);
                    break;
                }
            }
        }
        public void RemoveEffect(Type type)
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
            {
                if (this.appliedEffects[i].GetType() == type)
                {
                    this.appliedEffects.RemoveAt(i);
                    break;
                }
            }
        }
        public void PurgeEffect(string effectName)
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
            {
                if (this.appliedEffects[i].EffectName == effectName)
                {
                    this.appliedEffects[i].OnRemoval();
                    break;
                }
            }
        }
        public void PurgeEffect(Type type)
        {
            for (int i = 0; i < this.appliedEffects.Count; i++)
            {
                if (this.appliedEffects[i].GetType() == type)
                {
                    this.appliedEffects[i].OnRemoval();
                    break;
                }
            }
        }

        public virtual void CalculateStats()
        {
            this.resetStats();

            for (int i = 0; i < this.appliedEffects.Count; i++)
                this.appliedEffects[i].CalculateStats();
        }
        protected virtual void resetStats()
        {
            //Default Stats
            this.attackPower.ModValue = 0.0;
            this.physicalHaste.ModValue = 0.0;
            this.physicalHitChance.ModValue = 0.0;
            this.physicalCritChance.ModValue = 0.0;
            this.physicalCritPower.ModValue = 0.0;
            this.physicalReduction.ModValue = 0.0;
            this.physicalReflection.ModValue = 0.0;
            this.physicalAvoidance.ModValue = 0.0;

            this.spellPower.ModValue = 0.0;
            this.spellHaste.ModValue = 0.0;
            this.spellHitChance.ModValue = 0.0;
            this.spellCritChance.ModValue = 0.0;
            this.spellCritPower.ModValue = 0.0;
            this.spellReduction.ModValue = 0.0;
            this.spellReflection.ModValue = 0.0;
            this.spellAvoidance.ModValue = 0.0;

            this.MaxHealth.ModValue = 10.0;
            this.MaxMana.ModValue = 10.0;

            this.sightRadius.ModValue = 8;
            this.movementSpeed.ModValue = 1;
        }

        public virtual string UnitName { get; set; }

        //Physical Stats
        protected Stat attackPower = new Stat();
        protected Stat physicalHaste = new Stat();
        protected Stat physicalHitChance = new Stat();
        protected Stat physicalCritChance = new Stat();
        protected Stat physicalCritPower = new Stat();
        protected Stat physicalReduction = new Stat();
        protected Stat physicalReflection = new Stat();
        protected Stat physicalAvoidance = new Stat();
        protected Stat hpLeechPhysical = new Stat();
        protected Stat mpLeechPhysical = new Stat();

        //Magical Stats
        protected Stat spellPower = new Stat();
        protected Stat spellHaste = new Stat();
        protected Stat spellHitChance = new Stat();
        protected Stat spellCritChance = new Stat();
        protected Stat spellCritPower = new Stat();
        protected Stat spellReduction = new Stat();
        protected Stat spellReflection = new Stat();
        protected Stat spellAvoidance = new Stat();
        protected Stat hpLeechSpell = new Stat();
        protected Stat mpLeechSpell = new Stat();

        //Other Stats
        protected Stat sightRadius = new Stat();
        protected Stat movementSpeed = new Stat();
        protected int mpPerTurn = 0;
        protected int hpPerTurn = 0;

        //Resources
        protected int health = 1;
        protected int mana;
        protected Stat maxHealth = new Stat();
        protected Stat maxMana = new Stat();

        protected List<Effect> appliedEffects;
        protected List<Ability> abilityList;
        protected Entity parent;

        protected bool isImmune = false;

        #region Properties
        public Stat AttackPower
        {
            get { return attackPower; }
            set { attackPower = value; }
        }
        public Stat PhysicalHaste
        {
            get { return physicalHaste; }
            set { physicalHaste = value; }
        }
        public Stat PhysicalHitChance
        {
            get { return physicalHitChance; }
            set { physicalHitChance = value; }
        }
        public Stat PhysicalCritChance
        {
            get { return physicalCritChance; }
            set { physicalCritChance = value; }
        }
        public Stat PhysicalCritPower
        {
            get { return physicalCritPower; }
            set { physicalCritPower = value; }
        }
        public Stat PhysicalReduction
        {
            get { return physicalReduction; }
            set { physicalReduction = value; }
        }
        public Stat PhysicalReflection
        {
            get { return physicalReflection; }
            set { physicalReflection = value; }
        }
        public Stat PhysicalAvoidance
        {
            get { return physicalAvoidance; }
            set { physicalAvoidance = value; }
        }

        public Stat SpellPower
        {
            get { return spellPower; }
            set { spellPower = value; }
        }
        public Stat SpellHaste
        {
            get { return spellHaste; }
            set { spellHaste = value; }
        }
        public Stat SpellHitChance
        {
            get { return spellHitChance; }
            set { spellHitChance = value; }
        }
        public Stat SpellCritChance
        {
            get { return spellCritChance; }
            set { spellCritChance = value; }
        }
        public Stat SpellCritPower
        {
            get { return spellCritPower; }
            set { spellCritPower = value; }
        }
        public Stat SpellReduction
        {
            get { return spellReduction; }
            set { spellReduction = value; }
        }
        public Stat SpellReflection
        {
            get { return spellReflection; }
            set { spellReflection = value; }
        }
        public Stat SpellAvoidance
        {
            get { return spellAvoidance; }
            set { spellAvoidance = value; }
        }

        public Stat HPLeechPhysical
        {
            get { return this.hpLeechPhysical; }
            set { this.hpLeechPhysical = value; }
        }
        public Stat HPLeechSpell
        {
            get { return this.hpLeechSpell; }
            set { this.hpLeechSpell = value; }
        }
        public Stat MPLeechPhysical
        {
            get { return this.mpLeechPhysical; }
            set { this.mpLeechPhysical = value; }
        }
        public Stat MPLeechSpell
        {
            get { return this.mpLeechSpell; }
            set { this.mpLeechSpell = value; }
        }

        public Stat SightRadius
        {
            get { return sightRadius; }
            set { sightRadius = value; }
        }
        public Stat MovementSpeed
        {
            get { return movementSpeed; }
            set { movementSpeed = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public int Mana
        {
            get { return mana; }
            set { mana = value; }
        }
        public Stat MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }
        public Stat MaxMana
        {
            get { return maxMana; }
            set { maxMana = value; }
        }

        public int MPRegen { get { return this.mpPerTurn; } set { this.mpPerTurn = value; } }
        public int HPRegen { get { return this.hpPerTurn; } set { this.hpPerTurn = value; } }

        public List<Effect> AppliedEffects { get { return this.appliedEffects; } set { this.appliedEffects = value; } }
        public List<Ability> AbilityList { get { return this.abilityList; } set { this.abilityList = value; } }
        public Entity ParentEntity { get { return this.parent; } set { this.parent = value; } }

        public bool IsImmune { get { return this.isImmune; } set { this.isImmune = value; } }
        #endregion

        //TODO:  May eventually combine haste/crit for simplicity sake.  Depends on how well the game plays
    }

    public class Stat
    {
        private double baseValue;
        private double modValue;

        public double BaseValue { get { return this.baseValue; } set { this.baseValue = value; } }
        public double ModValue { get { return this.modValue; } set { this.modValue = value; } }
        public double EffectiveValue { get { return this.baseValue + modValue; } }

        public override string ToString()
        {
            return (this.EffectiveValue).ToString();
        }

        public static implicit operator double(Stat stat)
        {
            return stat.EffectiveValue;
        }

        public static implicit operator Stat(double d)
        {
            return new Stat() { baseValue = d, modValue = 0.0 };
        }
    }
}
