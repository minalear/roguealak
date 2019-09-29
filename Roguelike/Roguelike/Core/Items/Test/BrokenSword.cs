using System;
using Roguelike.Engine.Factories;

namespace Roguelike.Core.Items
{
    public class BrokenSword : Weapon
    {
        public BrokenSword()
            : base(WeaponTypes.Sword, EquipmentSlots.OneHand)
        {
            Name = "Broken Sword of the Fallen";
            WeaponSubType = "Godsword";
            ModPackage = new Stats.ModPackage()
            {
                AttackPower = 105,
                PhysicalCritChance = 40,
                PhysicalCritPower = 5,
                PhysicalHitChance = 80
            };
            Weight = 125;
            WeaponLevel = 5;
            ItemRarity = Rarities.Legendary;
            BaseDamage = 125;
        }
    }
}
