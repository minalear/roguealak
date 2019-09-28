using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Core.Items
{
    public class Broadsword : Weapon
    {
        public Broadsword()
            : base(Factories.WeaponTypes.Sword, EquipmentSlots.OneHand)
        {
            this.Name = "Test Sword of the Brave";
            this.WeaponSubType = "Broadsword";
            this.ModPackage = new Stats.ModPackage()
                {
                    AttackPower = 25,
                    PhysicalCritChance = 3,
                    PhysicalCritPower = 0.2
                };
            this.Weight = 40;
            this.WeaponLevel = 1;
            this.ItemRarity = Rarities.Uncommon;
            this.BaseDamage = 25;
        }
    }
}
