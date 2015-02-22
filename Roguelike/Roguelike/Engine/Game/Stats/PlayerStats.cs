using System;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.Game.Items;

namespace Roguelike.Engine.Game.Stats
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

        public override string UnitName { get { return this.Name; } set { this.Name = value; } }

        public PlayerStats()
            : base()
        {
            this.setBaseStats();
        }
        public PlayerStats(Player player)
            : base(player)
        {
            this.setBaseStats();
        }

        public void SetInitialStats()
        {
            this.CalculateStats();
            this.health = (int)this.maxHealth.EffectiveValue;
            this.mana = (int)this.maxMana.EffectiveValue;
        }

        public override void CalculateStats()
        {
            this.resetStats();

            //100.0 = 100%, 0.1 = 0.1%

            //Strength Scaling
            this.attackPower.BaseValue += this.strength * 3;
            this.physicalCritPower.BaseValue += this.strength * 0.1;
            this.physicalCritChance.BaseValue += this.strength * 0.05;

            //Agility Scaling
            this.attackPower.BaseValue += this.agility * 1.5;
            this.physicalCritChance.BaseValue += this.agility * 1.5;
            this.physicalHitChance.BaseValue += this.agility * 2.0;
            this.physicalAvoidance.BaseValue += this.agility * 0.05;

            //Dexterity Scaling
            this.physicalCritChance.BaseValue += this.dexterity * 1;
            this.physicalHaste.BaseValue += this.dexterity * 2.0;
            this.physicalAvoidance.BaseValue += this.dexterity * 0.2;


            //Intelligence Scaling
            this.spellPower.BaseValue += this.intelligence * 3;
            this.spellCritPower.BaseValue += this.intelligence * 0.05;
            this.maxMana.BaseValue += this.intelligence * 5;

            //Willpower Scaling
            this.spellHitChance.BaseValue += this.willpower * 1.5;
            this.spellCritChance.BaseValue += this.willpower * 1.0;
            this.spellReduction.BaseValue += this.willpower * 2.0;

            //Wisdom Scaling
            this.spellPower.BaseValue += this.wisdom * 1;
            this.maxMana.BaseValue += this.wisdom * 15;
            this.mpPerTurn = (int)(this.wisdom / 6);

            //Constitution Scaling
            this.physicalReduction.BaseValue += this.constitution * 2.0;
            this.maxHealth.BaseValue += this.constitution * 20;

            //Endurance Scaling
            this.physicalReduction.BaseValue += this.endurance * 1.0;
            this.physicalAvoidance.BaseValue += this.endurance * 0.1;
            this.hpPerTurn = (int)(this.endurance / 6);

            //Fortitude
            this.spellReduction.BaseValue += this.fortitude * 2.0;
            this.maxMana.BaseValue += this.fortitude * 5;

            for (int i = 0; i < this.appliedEffects.Count; i++)
                this.appliedEffects[i].CalculateStats();
        }
        protected override void resetStats()
        {
            this.setBaseStats();

            base.resetStats();
        }
        private void setBaseStats()
        {
            //Default Stats
            this.attackPower.BaseValue = 10.0;
            this.physicalHaste.BaseValue = 0.0;
            this.physicalHitChance.BaseValue = 68.0;
            this.physicalCritChance.BaseValue = 5.0;
            this.physicalCritPower.BaseValue = 1.0;
            this.physicalReduction.BaseValue = 0.0;
            this.physicalReflection.BaseValue = 0.0;
            this.physicalAvoidance.BaseValue = 0.5;

            this.spellPower.BaseValue = 8.0;
            this.spellHaste.BaseValue = 0.0;
            this.spellHitChance.BaseValue = 75.0;
            this.spellCritChance.BaseValue = 7.0;
            this.spellCritPower.BaseValue = 1.2;
            this.spellReduction.BaseValue = 0.0;
            this.spellReflection.BaseValue = 0.0;
            this.spellAvoidance.BaseValue = 0.0;

            this.sightRadius.BaseValue = 8;
            this.movementSpeed.BaseValue = 1;

            this.maxHealth.BaseValue = 10;
            this.maxMana.BaseValue = 10;
        }

        public override string GetFormattedName()
        {
            string name = this.Name;

            if (!string.IsNullOrEmpty(this.Township))
                name += " of " + Township;
            if (!string.IsNullOrEmpty(this.Title))
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

        public int Strength { get { return this.strength; } set { this.strength = value; } }
        public int Agility { get { return this.agility; } set { this.agility = value; } }
        public int Dexterity { get { return this.dexterity; } set { this.dexterity = value; } }

        public int Intelligence { get { return this.intelligence; } set { this.intelligence = value; } }
        public int Willpower { get { return this.willpower; } set { this.willpower = value; } }
        public int Wisdom { get { return this.wisdom; } set { this.wisdom = value; } }

        public int Constitution { get { return this.constitution; } set { this.constitution = value; } }
        public int Endurance { get { return this.endurance; } set { this.endurance = value; } }
        public int Fortitude { get { return this.fortitude; } set { this.fortitude = value; } }
    }
}
