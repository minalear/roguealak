using System;
using Roguelike.Core.Entities;
using Roguelike.Core.Items;

namespace Roguelike.Core.Stats
{
    public class PlayerStats : StatsPackage
    {
        public string Name;
        public string Township;
        public string Title;

        public string Race;
        public string Culture;
        public string Class;
        public string Gender = "Male";

        public override string UnitName { get { return Name; } set { Name = value; } }

        public PlayerStats()
            : base()
        {
            setBaseStats();
        }
        public PlayerStats(Player player)
            : base(player)
        {
            setBaseStats();
        }

        public void SetInitialStats()
        {
            CalculateStats();
            health = (int)maxHealth.EffectiveValue;
            mana = (int)maxMana.EffectiveValue;
        }

        public override void CalculateStats()
        {
            resetStats();

            //100.0 = 100%, 0.1 = 0.1%

            //Strength Scaling
            attackPower.BaseValue += strength * 3;
            physicalCritPower.BaseValue += strength * 0.1;
            physicalCritChance.BaseValue += strength * 0.05;

            //Agility Scaling
            attackPower.BaseValue += agility * 1.5;
            physicalCritChance.BaseValue += agility * 1.5;
            physicalHitChance.BaseValue += agility * 2.0;
            physicalAvoidance.BaseValue += agility * 0.05;

            //Dexterity Scaling
            physicalCritChance.BaseValue += dexterity * 1;
            physicalHaste.BaseValue += dexterity * 2.0;
            physicalAvoidance.BaseValue += dexterity * 0.2;


            //Intelligence Scaling
            spellPower.BaseValue += intelligence * 3;
            spellCritPower.BaseValue += intelligence * 0.05;
            maxMana.BaseValue += intelligence * 5;

            //Willpower Scaling
            spellHitChance.BaseValue += willpower * 1.5;
            spellCritChance.BaseValue += willpower * 1.0;
            spellReduction.BaseValue += willpower * 2.0;

            //Wisdom Scaling
            spellPower.BaseValue += wisdom * 1;
            maxMana.BaseValue += wisdom * 15;
            mpPerTurn = (int)(wisdom / 6);

            //Constitution Scaling
            physicalReduction.BaseValue += constitution * 2.0;
            maxHealth.BaseValue += constitution * 20;

            //Endurance Scaling
            physicalReduction.BaseValue += endurance * 1.0;
            physicalAvoidance.BaseValue += endurance * 0.1;
            hpPerTurn = (int)(endurance / 6);

            //Fortitude
            spellReduction.BaseValue += fortitude * 2.0;
            maxMana.BaseValue += fortitude * 5;

            for (int i = 0; i < appliedEffects.Count; i++)
                appliedEffects[i].CalculateStats();
        }
        protected override void resetStats()
        {
            setBaseStats();

            base.resetStats();
        }
        private void setBaseStats()
        {
            //Default Stats
            attackPower.BaseValue = 10.0;
            physicalHaste.BaseValue = 0.0;
            physicalHitChance.BaseValue = 68.0;
            physicalCritChance.BaseValue = 5.0;
            physicalCritPower.BaseValue = 1.0;
            physicalReduction.BaseValue = 0.0;
            physicalReflection.BaseValue = 0.0;
            physicalAvoidance.BaseValue = 0.5;

            spellPower.BaseValue = 8.0;
            spellHaste.BaseValue = 0.0;
            spellHitChance.BaseValue = 75.0;
            spellCritChance.BaseValue = 7.0;
            spellCritPower.BaseValue = 1.2;
            spellReduction.BaseValue = 0.0;
            spellReflection.BaseValue = 0.0;
            spellAvoidance.BaseValue = 0.0;

            sightRadius.BaseValue = 8;
            movementSpeed.BaseValue = 1;

            maxHealth.BaseValue = 10;
            maxMana.BaseValue = 10;
        }

        public override string GetFormattedName()
        {
            string name = Name;

            if (!string.IsNullOrEmpty(Township))
                name += " of " + Township;
            if (!string.IsNullOrEmpty(Title))
                name += " the " + Title;

            return name;
        }
        public override string ToString()
        {
            return base.ToString();
        }

        public override void OnAttack(Combat.CombatResults results)
        {
            Inventory.OnAttack(results);

            base.OnAttack(results);
        }
        public override void OnDefend(Combat.CombatResults results)
        {
            Inventory.OnDefend(results);

            base.OnDefend(results);
        }
        public override void OnDeath()
        {
            base.OnDeath();
        }
        
        //Physical Offensive
        private int strength;
        private int agility;
        private int dexterity;

        //Spell Offensive
        private int intelligence;
        private int willpower;
        private int wisdom;

        //Defensive
        private int constitution;
        private int endurance;
        private int fortitude;

        public int Strength { get { return strength; } set { strength = value; } }
        public int Agility { get { return agility; } set { agility = value; } }
        public int Dexterity { get { return dexterity; } set { dexterity = value; } }

        public int Intelligence { get { return intelligence; } set { intelligence = value; } }
        public int Willpower { get { return willpower; } set { willpower = value; } }
        public int Wisdom { get { return wisdom; } set { wisdom = value; } }

        public int Constitution { get { return constitution; } set { constitution = value; } }
        public int Endurance { get { return endurance; } set { endurance = value; } }
        public int Fortitude { get { return fortitude; } set { fortitude = value; } }
    }
}
