using System;
using Roguelike.Engine.Factories;

namespace Roguelike.Core.Items
{
    public class Broadsword : Weapon
    {
        public Broadsword()
            : base(WeaponTypes.Sword, EquipmentSlots.OneHand)
        {
            Name = "Test Sword of the Brave";
            WeaponSubType = "Broadsword";
            ModPackage = new Stats.ModPackage()
                {
                    AttackPower = 25,
                    PhysicalCritChance = 3,
                    PhysicalCritPower = 0.2
                };
            Weight = 40;
            WeaponLevel = 1;
            ItemRarity = Rarities.Uncommon;
            BaseDamage = 25;
        }
    }
}
