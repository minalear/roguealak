using System;
using System.Collections.Generic;
using Roguelike.Core.Entities;
using Roguelike.Core.Combat;

namespace Roguelike.Core.Stats
{
    public class StatsPackage
    {
        public StatsPackage()
        {
            appliedEffects = new List<Effect>();
            abilityList = new List<Ability>();
        }
        public StatsPackage(Entity parent)
        {
            appliedEffects = new List<Effect>();
            abilityList = new List<Ability>();

            abilityList.Add(new Combat.Abilities.BasicAttack());
            parent = parent;
        }

        public bool IsDead()
        {
            return (health <= 0);
        }
        public virtual string GetFormattedName()
        {
            return UnitName;
        }
        public virtual string GetInformation()
        {
            string info =
                    "-=Physical Stats=-" + "<br>" +
                    "Attack Power: " + GameManager.Player.PlayerStats.AttackPower + "<br>" +
                    "       Haste: " + GameManager.Player.PlayerStats.PhysicalHaste + "%<br>" +
                    "  Hit Chance: " + GameManager.Player.PlayerStats.PhysicalHitChance + "%<br>" +
                    " Crit Chance: " + GameManager.Player.PlayerStats.PhysicalCritChance + "%<br>" +
                    "  Crit Power: " + GameManager.Player.PlayerStats.PhysicalCritPower + "<br>" +
                    "   Reduction: " + GameManager.Player.PlayerStats.PhysicalReduction + "%<br>" +
                    "  Reflection: " + GameManager.Player.PlayerStats.PhysicalReflection + "%<br>" +
                    "   Avoidance: " + GameManager.Player.PlayerStats.PhysicalAvoidance + "%<br><br>" +

                    "-=Magical Stats=-" + "<br>" +
                    "Spell Power: " + GameManager.Player.PlayerStats.SpellPower + "<br>" +
                    "      Haste: " + GameManager.Player.PlayerStats.SpellHaste + "%<br>" +
                    " Hit Chance: " + GameManager.Player.PlayerStats.SpellHitChance + "%<br>" +
                    "Crit Chance: " + GameManager.Player.PlayerStats.SpellCritChance + "%<br>" +
                    " Crit Power: " + GameManager.Player.PlayerStats.SpellCritPower + "<br>" +
                    "  Reduction: " + GameManager.Player.PlayerStats.SpellReduction + "%<br>" +
                    " Reflection: " + GameManager.Player.PlayerStats.SpellReflection + "%<br>" +
                    "  Avoidance: " + GameManager.Player.PlayerStats.SpellAvoidance + "%";
            return info;
        }

        public virtual void UpdateStep()
        {
            for (int i = 0; i < appliedEffects.Count; i++)
            {
                appliedEffects[i].UpdateStep();

                if (appliedEffects[i].DoPurge)
                {
                    appliedEffects.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < abilityList.Count; i++)
            {
                abilityList[i].UpdateStep();
            }

            AddHealth(hpPerTurn);
            AddMana(mpPerTurn);

            if (IsDead())
            {
                OnDeath();
            }

            CalculateStats();
        }
        public virtual void ApplyEffect(Effect effect)
        {
            effect.Parent = this;

            appliedEffects.Add(effect);
            effect.OnApplication(ParentEntity);
        }

        public virtual void OnAttack(CombatResults results)
        {
            for (int i = 0; i < appliedEffects.Count; i++)
                appliedEffects[i].OnAttack(results);
        }
        public virtual void OnDefend(CombatResults results)
        {
            for (int i = 0; i < appliedEffects.Count; i++)
                appliedEffects[i].OnDefend(results);
        }

        public virtual void OnDeath()
        {
            for (int i = 0; i < appliedEffects.Count; i++)
                appliedEffects[i].OnDeath();
            ParentEntity.OnDeath();
        }
        public virtual void OnSpawn()
        {
            for (int i = 0; i < appliedEffects.Count; i++)
                appliedEffects[i].OnSpawn();
        }
        public virtual void OnMove()
        {
            for (int i = 0; i < appliedEffects.Count; i++)
                appliedEffects[i].OnMove();
        }

        public virtual void AddHealth(int amount)
        {
            if (amount != 0 && !IsDead())
            {
                for (int i = 0; i < appliedEffects.Count; i++)
                    amount = appliedEffects[i].OnHealthGain(amount);
                health += amount;

                if (health > maxHealth)
                    health = (int)maxHealth.EffectiveValue;
            }
        }
        public virtual void DrainHealth(int amount)
        {
            if (amount != 0 && !isImmune && !IsDead())
            {
                for (int i = 0; i < appliedEffects.Count; i++)
                    amount = appliedEffects[i].OnHealthLoss(amount);
                health -= amount;

                if (health < 0)
                    health = 0;
            }
        }
        public virtual void DealDOTDamage(int amount, Effect effect)
        {
            if (!isImmune && !IsDead())
            {
                int absorption = 0;
                if (effect.EffectType == EffectTypes.Magical)
                {
                    absorption = (int)(spellReduction.EffectiveValue / 100 * amount);
                }
                else if (effect.EffectType == EffectTypes.Physical)
                {
                    absorption = (int)(physicalReduction.EffectiveValue / 100 * amount);
                }
                else if (effect.EffectType == EffectTypes.Hybrid)
                {
                    double absorbPercent = ((spellReduction / 2) + (physicalReduction / 2)) / 100;
                    absorption = (int)(absorbPercent * amount);
                }

                DrainHealth(amount - absorption);
            }
        }

        public virtual void AddMana(int amount)
        {
            if (amount != 0 && !IsDead())
            {
                for (int i = 0; i < appliedEffects.Count; i++)
                    amount = appliedEffects[i].OnManaGain(amount);
                mana += amount;

                if (mana > maxMana)
                    mana = (int)maxMana.EffectiveValue;
            }
        }
        public virtual void DrainMana(int amount)
        {
            if (amount != 0 && !IsDead())
            {
                for (int i = 0; i < appliedEffects.Count; i++)
                    amount = appliedEffects[i].OnManaLoss(amount);
                mana -= amount;

                if (mana < 0)
                    mana = 0;
            }
        }

        public bool HasEffect(string effect)
        {
            for (int i = 0; i < appliedEffects.Count; i++)
            {
                if (appliedEffects[i].EffectName == effect)
                    return true;
            }

            return false;
        }
        public bool HasEffect(Type type)
        {
            for (int i = 0; i < appliedEffects.Count; i++)
            {
                if (appliedEffects[i].GetType() == type)
                    return true;
            }

            return false;
        }
        public Effect GetEffect(string effect)
        {
            for (int i = 0; i < appliedEffects.Count; i++)
            {
                if (appliedEffects[i].EffectName == effect)
                    return appliedEffects[i];
            }

            return null;
        }
        public Effect GetEffect(Type type)
        {
            for (int i = 0; i < appliedEffects.Count; i++)
            {
                if (appliedEffects[i].GetType() == type)
                    return appliedEffects[i];
            }

            return null;
        }
        public void RemoveEffect(string effectName)
        {
            for (int i = 0; i < appliedEffects.Count; i++)
            {
                if (appliedEffects[i].EffectName == effectName)
                {
                    appliedEffects.RemoveAt(i);
                    break;
                }
            }
        }
        public void RemoveEffect(Type type)
        {
            for (int i = 0; i < appliedEffects.Count; i++)
            {
                if (appliedEffects[i].GetType() == type)
                {
                    appliedEffects.RemoveAt(i);
                    break;
                }
            }
        }
        public void PurgeEffect(string effectName)
        {
            for (int i = 0; i < appliedEffects.Count; i++)
            {
                if (appliedEffects[i].EffectName == effectName)
                {
                    appliedEffects[i].OnRemoval();
                    break;
                }
            }
        }
        public void PurgeEffect(Type type)
        {
            for (int i = 0; i < appliedEffects.Count; i++)
            {
                if (appliedEffects[i].GetType() == type)
                {
                    appliedEffects[i].OnRemoval();
                    break;
                }
            }
        }

        public virtual void CalculateStats()
        {
            resetStats();

            for (int i = 0; i < appliedEffects.Count; i++)
                appliedEffects[i].CalculateStats();
        }
        protected virtual void resetStats()
        {
            //Default Stats
            attackPower.ModValue = 0.0;
            physicalHaste.ModValue = 0.0;
            physicalHitChance.ModValue = 0.0;
            physicalCritChance.ModValue = 0.0;
            physicalCritPower.ModValue = 0.0;
            physicalReduction.ModValue = 0.0;
            physicalReflection.ModValue = 0.0;
            physicalAvoidance.ModValue = 0.0;

            spellPower.ModValue = 0.0;
            spellHaste.ModValue = 0.0;
            spellHitChance.ModValue = 0.0;
            spellCritChance.ModValue = 0.0;
            spellCritPower.ModValue = 0.0;
            spellReduction.ModValue = 0.0;
            spellReflection.ModValue = 0.0;
            spellAvoidance.ModValue = 0.0;

            MaxHealth.ModValue = 10.0;
            MaxMana.ModValue = 10.0;

            sightRadius.ModValue = 8;
            movementSpeed.ModValue = 1;
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
            get { return hpLeechPhysical; }
            set { hpLeechPhysical = value; }
        }
        public Stat HPLeechSpell
        {
            get { return hpLeechSpell; }
            set { hpLeechSpell = value; }
        }
        public Stat MPLeechPhysical
        {
            get { return mpLeechPhysical; }
            set { mpLeechPhysical = value; }
        }
        public Stat MPLeechSpell
        {
            get { return mpLeechSpell; }
            set { mpLeechSpell = value; }
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

        public int MPRegen { get { return mpPerTurn; } set { mpPerTurn = value; } }
        public int HPRegen { get { return hpPerTurn; } set { hpPerTurn = value; } }

        public List<Effect> AppliedEffects { get { return appliedEffects; } set { appliedEffects = value; } }
        public List<Ability> AbilityList { get { return abilityList; } set { abilityList = value; } }
        public Entity ParentEntity { get { return parent; } set { parent = value; } }

        public bool IsImmune { get { return isImmune; } set { isImmune = value; } }
        #endregion

        //TODO:  May eventually combine haste/crit for simplicity sake.  Depends on how well the game plays
    }

    public class Stat
    {
        private double baseValue;
        private double modValue;

        public double BaseValue { get { return baseValue; } set { baseValue = value; } }
        public double ModValue { get { return modValue; } set { modValue = value; } }
        public double EffectiveValue { get { return baseValue + modValue; } }

        public override string ToString()
        {
            return (EffectiveValue).ToString();
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
