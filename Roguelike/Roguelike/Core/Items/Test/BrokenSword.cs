using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Core.Items
{
    public class BrokenSword : Weapon
    {
        public BrokenSword()
            : base(Factories.WeaponTypes.Sword, EquipmentSlots.OneHand)
        {
            this.Name = "Broken Sword of the Fallen";
            this.WeaponSubType = "Godsword";
            this.ModPackage = new Stats.ModPackage()
            {
                AttackPower = 105,
                PhysicalCritChance = 40,
                PhysicalCritPower = 5,
                PhysicalHitChance = 80
            };
            this.Weight = 125;
            this.WeaponLevel = 5;
            this.ItemRarity = Rarities.Legendary;
            this.BaseDamage = 125;
        }
    }
}
