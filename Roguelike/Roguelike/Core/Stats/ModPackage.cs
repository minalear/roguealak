using System;
using System.Collections.Generic;
using Roguelike.Core.Entities;
using Roguelike.Core.Combat;
using Roguelike.Core.Items;

namespace Roguelike.Core.Stats
{
    public class ModPackage
    {
        //Physical Stats
        protected double attackPower = 0.0;
        protected double physicalHaste = 0.0;
        protected double physicalHitChance = 0.0;
        protected double physicalCritChance = 0.0;
        protected double physicalCritPower = 0.0;
        protected double physicalReduction = 0.0;
        protected double physicalReflection = 0.0;
        protected double physicalAvoidance = 0.0;

        //Magical Stats
        protected double spellPower = 0.0;
        protected double spellHaste = 0.0;
        protected double spellHitChance = 0.0;
        protected double spellCritChance = 0.0;
        protected double spellCritPower = 0.0;
        protected double spellReduction = 0.0;
        protected double spellReflection = 0.0;
        protected double spellAvoidance = 0.0;

        protected int bonusHealth = 0;
        protected int bonusMana = 0;

        public StatsPackage ApplyPackage(StatsPackage package)
        {
            package.AttackPower.ModValue += this.attackPower;
            package.PhysicalHaste.ModValue += this.physicalHaste;
            package.PhysicalHitChance.ModValue += this.physicalHitChance;
            package.PhysicalCritChance.ModValue += this.physicalCritChance;
            package.PhysicalCritPower.ModValue += this.physicalCritPower;
            package.PhysicalReduction.ModValue += this.physicalReduction;
            package.PhysicalReflection.ModValue += this.physicalReflection;
            package.PhysicalAvoidance.ModValue += this.physicalAvoidance;

            package.SpellPower.ModValue += this.spellPower;
            package.SpellHaste.ModValue += this.spellHaste;
            package.SpellHitChance.ModValue += this.spellHitChance;
            package.SpellCritChance.ModValue += this.spellCritChance;
            package.SpellCritPower.ModValue += this.spellCritPower;
            package.SpellReduction.ModValue += this.spellReduction;
            package.SpellReflection.ModValue += this.spellReflection;
            package.SpellAvoidance.ModValue += this.spellAvoidance;

            package.MaxHealth.ModValue += bonusHealth;
            package.MaxMana.ModValue += bonusMana;

            return package;
        }
        public string GetStatInfo()
        {
            string info =
                "";

            if (AttackPower != 0)
                info += "Attack Power: " + AttackPower.ToString() + "\n";
            if (PhysicalHaste != 0)
                info += "P. Haste: " + PhysicalHaste.ToString() + "%\n";
            if (PhysicalHitChance != 0)
                info += "P. Hit Chance: " + PhysicalHitChance.ToString() + "%\n";
            if (PhysicalCritChance != 0)
                info += "P. Crit Chance: " + PhysicalCritChance.ToString() + "%\n";
            if (PhysicalCritPower != 0)
                info += "P. Crit Power: " + PhysicalCritPower.ToString() + "\n";
            if (PhysicalReduction != 0)
                info += "P. Reduction: " + PhysicalReduction.ToString() + "%\n";
            if (PhysicalReflection != 0)
                info += "P. Reflection: " + PhysicalReflection.ToString() + "%\n";
            if (PhysicalAvoidance != 0)
                info += "P. Avoidance: " + PhysicalAvoidance.ToString() + "%\n";




            if (SpellPower != 0)
                info += "Spell Power: " + SpellPower.ToString() + "\n";
            if (SpellHaste != 0)
                info += "S. Haste: " + SpellHaste.ToString() + "%\n";
            if (SpellHitChance != 0)
                info += "S. Hit Chance: " + SpellHitChance.ToString() + "%\n";
            if (SpellCritChance != 0)
                info += "S. Crit Chance: " + SpellCritChance.ToString() + "%\n";
            if (SpellCritPower != 0)
                info += "S. Crit Power: " + SpellCritPower.ToString() + "\n";
            if (SpellReduction != 0)
                info += "S. Reduction: " + SpellReduction.ToString() + "%\n";
            if (SpellReflection != 0)
                info += "S. Reflection: " + SpellReflection.ToString() + "%\n";
            if (SpellAvoidance != 0)
                info += "S. Avoidance: " + SpellAvoidance.ToString() + "%\n";

            if (BonusHealth != 0)
                info += "Bonus Health: " + BonusHealth.ToString() + "\n";
            if (BonusMana != 0)
                info += "Bonus Mana: " + BonusMana.ToString() + "\n";

            return info;
        }

        public double AttackPower
        {
            get { return attackPower; }
            set { attackPower = value; }
        }
        public double PhysicalHaste
        {
            get { return physicalHaste; }
            set { physicalHaste = value; }
        }
        public double PhysicalHitChance
        {
            get { return physicalHitChance; }
            set { physicalHitChance = value; }
        }
        public double PhysicalCritChance
        {
            get { return physicalCritChance; }
            set { physicalCritChance = value; }
        }
        public double PhysicalCritPower
        {
            get { return physicalCritPower; }
            set { physicalCritPower = value; }
        }
        public double PhysicalReduction
        {
            get { return physicalReduction; }
            set { physicalReduction = value; }
        }
        public double PhysicalReflection
        {
            get { return physicalReflection; }
            set { physicalReflection = value; }
        }
        public double PhysicalAvoidance
        {
            get { return physicalAvoidance; }
            set { physicalAvoidance = value; }
        }

        public double SpellPower
        {
            get { return spellPower; }
            set { spellPower = value; }
        }
        public double SpellHaste
        {
            get { return spellHaste; }
            set { spellHaste = value; }
        }
        public double SpellHitChance
        {
            get { return spellHitChance; }
            set { spellHitChance = value; }
        }
        public double SpellCritChance
        {
            get { return spellCritChance; }
            set { spellCritChance = value; }
        }
        public double SpellCritPower
        {
            get { return spellCritPower; }
            set { spellCritPower = value; }
        }
        public double SpellReduction
        {
            get { return spellReduction; }
            set { spellReduction = value; }
        }
        public double SpellReflection
        {
            get { return spellReflection; }
            set { spellReflection = value; }
        }
        public double SpellAvoidance
        {
            get { return spellAvoidance; }
            set { spellAvoidance = value; }
        }

        public int BonusHealth { get { return this.bonusHealth; } set { this.bonusHealth = value; } }
        public int BonusMana { get { return this.bonusMana; } set { this.bonusMana = value; } }

        //TODO:  HP/MP Regen, HP/MP Leech 
    }
}
