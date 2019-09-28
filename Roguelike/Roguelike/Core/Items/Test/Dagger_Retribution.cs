using System;

namespace Roguelike.Core.Items.Test
{
    public class Dagger_Retribution : Weapon
    {
        public Dagger_Retribution()
            : base(Factories.WeaponTypes.Knife, EquipmentSlots.OneHand)
        {
            this.Name = "Dagger of Retribution";
            this.WeaponSubType = "Dagger";
            this.ModPackage = new Stats.ModPackage()
            {
                AttackPower = 25,
                PhysicalCritChance = 15,
                PhysicalCritPower = 0.8,
                PhysicalHitChance = 20,
                PhysicalHaste = 35
            };
            this.Weight = 10;
            this.WeaponLevel = 3;
            this.ItemRarity = Rarities.Epic;
            this.BaseDamage = 50;

            this.Description = "These daggers were used by a legendary rogue of yore, who died from dysentary.  Now his daggers lay unused.";
        }

        public override void OnAttack(Combat.CombatResults results)
        {
            if (results.UsedAbility.AbilityName == GameManager.Player.StatsPackage.AbilityList[0].AbilityName)
            {
                int damage = this.BaseDamage;
                if (this.EquippedSlot == EquipmentSlots.MainHand) //Main Hand weapons just apply regular weapon damage
                {
                    if (Inventory.OffHand != null && Inventory.OffHand.Name == this.Name)
                        damage += (int)(damage * 0.5);

                    results.AppliedDamage += damage - results.UsedAbility.CalculateAbsorption(damage, results.Target);
                }
                else if (this.EquippedSlot == EquipmentSlots.OffHand) //Off Hand weapons have a chance to damage, based off user's Haste stat
                {
                    if (Inventory.MainHand != null && Inventory.MainHand.Name == this.Name)
                        damage += (int)(damage * 0.5);

                    int chanceToHit = (int)(GameManager.Player.StatsPackage.PhysicalHaste.EffectiveValue);
                    if (RNG.Next(0, 100) < chanceToHit)
                        results.AppliedDamage += damage - results.UsedAbility.CalculateAbsorption(damage, results.Target);
                }
            }
        }
    }
}
