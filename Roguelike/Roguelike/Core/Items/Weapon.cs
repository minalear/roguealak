using System;
using Roguelike.Core.Stats;
using Roguelike.Engine;
using Roguelike.Engine.Factories;

namespace Roguelike.Core.Items
{
    public class Weapon : Equipment
    {
        public Weapon(WeaponTypes type, EquipmentSlots slot)
            : base(slot)
        {
            weaponType = type;
        }

        public override string GetDescription()
        {
            string desc = Name + "\n";
            desc += WeaponSubType + " - " + WeaponType + " - " + ItemRarity.ToString() + "\n";
            desc += "Gold: " + Value + " - " + EquipSlot.ToString() + "\n";
            desc += Description + "\n\n";
            desc += ModPackage.GetStatInfo();

            return desc;
        }

        public override void OnAttack(Combat.CombatResults results)
        {
            if (results.UsedAbility.AbilityName == GameManager.Player.StatsPackage.AbilityList[0].AbilityName)
            {
                int initialDamage = getWeaponDamage();
                if (EquippedSlot == EquipmentSlots.MainHand) //Main Hand weapons just apply regular weapon damage
                {
                    results.AppliedDamage += initialDamage - results.UsedAbility.CalculateAbsorption(initialDamage, results.Target);
                }
                else if (EquippedSlot == EquipmentSlots.OffHand) //Off Hand weapons have a chance to damage, based off user's Haste stat
                {
                    int chanceToHit = (int)(GameManager.Player.StatsPackage.PhysicalHaste.EffectiveValue);
                    if (RNG.Next(0, 100) < chanceToHit)
                        results.AppliedDamage += initialDamage - results.UsedAbility.CalculateAbsorption(initialDamage, results.Target);
                }
                else if (EquippedSlot == EquipmentSlots.TwoHand) //Two Handed weapons have a chance to crit, based off user's crit chance and power
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
            return baseWeaponDamage + RNG.Next(-Weight, Weight);
        }

        private WeaponTypes weaponType;
        private WeaponAttacks weaponAttack;
        private StatWeights statWeight;
        private int baseWeaponDamage;
        private string subType;

        public WeaponTypes WeaponType { get { return weaponType; } set { weaponType = value; } }
        public WeaponAttacks WeaponAttack { get { return weaponAttack; } set { weaponAttack = value; } }
        public StatWeights StatWeight { get { return statWeight; } set { statWeight = value; } }
        public int BaseDamage { get { return baseWeaponDamage; } set { baseWeaponDamage = value; } }
        public string WeaponSubType { get { return subType; } set { subType = value; } }

        public int WeaponLevel { get; set; }
    }
}
