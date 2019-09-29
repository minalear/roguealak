using System;
using Roguelike.Engine;
using Roguelike.Engine.Factories;

namespace Roguelike.Core.Items.Test
{
    public class Dagger_Retribution : Weapon
    {
        public Dagger_Retribution()
            : base(WeaponTypes.Knife, EquipmentSlots.OneHand)
        {
            Name = "Dagger of Retribution";
            WeaponSubType = "Dagger";
            ModPackage = new Stats.ModPackage()
            {
                AttackPower = 25,
                PhysicalCritChance = 15,
                PhysicalCritPower = 0.8,
                PhysicalHitChance = 20,
                PhysicalHaste = 35
            };
            Weight = 10;
            WeaponLevel = 3;
            ItemRarity = Rarities.Epic;
            BaseDamage = 50;

            Description = "These daggers were used by a legendary rogue of yore, who died from dysentary.  Now his daggers lay unused.";
        }

        public override void OnAttack(Combat.CombatResults results)
        {
            if (results.UsedAbility.AbilityName == GameManager.Player.StatsPackage.AbilityList[0].AbilityName)
            {
                int damage = BaseDamage;
                if (EquippedSlot == EquipmentSlots.MainHand) //Main Hand weapons just apply regular weapon damage
                {
                    if (Inventory.OffHand != null && Inventory.OffHand.Name == Name)
                        damage += (int)(damage * 0.5);

                    results.AppliedDamage += damage - results.UsedAbility.CalculateAbsorption(damage, results.Target);
                }
                else if (EquippedSlot == EquipmentSlots.OffHand) //Off Hand weapons have a chance to damage, based off user's Haste stat
                {
                    if (Inventory.MainHand != null && Inventory.MainHand.Name == Name)
                        damage += (int)(damage * 0.5);

                    int chanceToHit = (int)(GameManager.Player.StatsPackage.PhysicalHaste.EffectiveValue);
                    if (Engine.RNG.Next(0, 100) < chanceToHit)
                        results.AppliedDamage += damage - results.UsedAbility.CalculateAbsorption(damage, results.Target);
                }
            }
        }
    }
}
