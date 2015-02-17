using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Roguelike.Engine.Game.Stats;

namespace Roguelike.Engine.Game.Items
{
    public class Weapon : Equipment
    {
        public Weapon(Factories.WeaponTypes type, EquipmentSlots slot)
            : base(slot)
        {
            this.weaponType = type;
        }

        public override string GetDescription()
        {
            string desc = this.Name + "\n";
            desc += this.WeaponSubType + " - " + this.WeaponType + " - " + this.ItemRarity.ToString() + "\n";
            desc += this.EquipSlot.ToString() + "\n";
            desc += this.Description + "\n\n";
            desc += this.ModPackage.GetStatInfo();

            return desc;
        }

        public override void OnAttack(Combat.CombatResults results)
        {
            if (results.UsedAbility.AbilityName == GameManager.Player.StatsPackage.AbilityList[0].AbilityName)
            {
                int initialDamage = this.getWeaponDamage();
                if (this.EquippedSlot == EquipmentSlots.MainHand) //Main Hand weapons just apply regular weapon damage
                {
                    results.AppliedDamage += initialDamage - results.UsedAbility.CalculateAbsorption(initialDamage, results.Target);
                }
                else if (this.EquippedSlot == EquipmentSlots.OffHand) //Off Hand weapons have a chance to damage, based off user's Haste stat
                {
                    int chanceToHit = (int)(GameManager.Player.StatsPackage.PhysicalHaste.EffectiveValue);
                    if (RNG.Next(0, 100) < chanceToHit)
                        results.AppliedDamage += initialDamage - results.UsedAbility.CalculateAbsorption(initialDamage, results.Target);
                }
                else if (this.EquippedSlot == EquipmentSlots.TwoHand) //Two Handed weapons have a chance to crit, based off user's crit chance and power
                {
                    int chanceToCrit = (int)(GameManager.Player.StatsPackage.PhysicalCritChance);
                    if (RNG.Next(0, 100) <= chanceToCrit)
                    {
                        int damage = (int)(initialDamage * GameManager.Player.StatsPackage.PhysicalCritPower);
                        results.AppliedDamage += damage - results.UsedAbility.CalculateAbsorption(damage, results.Target);
                    }
                    else
                        results.AppliedDamage += initialDamage - results.UsedAbility.CalculateAbsorption(initialDamage, results.Target);
                }
            }
        }
        protected virtual int getWeaponDamage()
        {
            return this.baseWeaponDamage + RNG.Next(-this.Weight, this.Weight);
        }

        private Factories.WeaponTypes weaponType;
        private Factories.WeaponAttacks weaponAttack;
        private Factories.StatWeights statWeight;
        private int baseWeaponDamage;
        private string subType;

        public Factories.WeaponTypes WeaponType { get { return this.weaponType; } set { this.weaponType = value; } }
        public Factories.WeaponAttacks WeaponAttack { get { return this.weaponAttack; } set { this.weaponAttack = value; } }
        public Factories.StatWeights StatWeight { get { return this.statWeight; } set { this.statWeight = value; } }
        public int BaseDamage { get { return this.baseWeaponDamage; } set { this.baseWeaponDamage = value; } }
        public string WeaponSubType { get { return this.subType; } set { this.subType = value; } }

        public int WeaponLevel { get; set; }
    }
}
