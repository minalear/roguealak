using System;
using System.Collections.Generic;
using Roguelike.Core;
using Roguelike.Core.Items;
using Roguelike.Core.Stats;

namespace Roguelike.Engine.Factories
{
    public enum WeaponTypes { Sword, Axe, Mace, Flail, Sickle, Whip, Knife, Staff, Shield }
    public enum StatWeights { PhysicalPower, PhysicalSpeed, Defense, MagicalPower, MagicalSpeed }
    public enum WeaponAttacks { Melee, RangePhysical, RangeMagical }

    public class WeaponGenTemplate
    {
        public WeaponTypes PrimaryType;
        public string Subtype;
        public StatWeights[] WeaponStats;
        public EquipmentSlots[] Slots;
        public WeaponAttacks WeaponAttack;
        public double Multiplier = 1.0;

        public int DamageMin;
        public int DamageMax;

        public int WeightMin;
        public int WeightMax;

        public int GetRandomDamage()
        {
            return RNG.Next(DamageMin, DamageMax);
        }
        public int GetRandomWeight()
        {
            return RNG.Next(WeightMin, WeightMax);
        }
        public StatWeights GetRandomStatWeight()
        {
            return this.WeaponStats[RNG.Next(0, this.WeaponStats.Length)];
        }
        public EquipmentSlots GetRandomSlot()
        {
            return this.Slots[RNG.Next(0, this.Slots.Length)];
        }

        public Weapon GenerateWeapon()
        {
            Weapon weapon = new Weapon(this.PrimaryType, this.GetRandomSlot());
            weapon.WeaponSubType = Subtype;
            weapon.BaseDamage = this.GetRandomDamage();
            weapon.Weight = this.GetRandomWeight();
            weapon.StatWeight = this.GetRandomStatWeight();

            return weapon;
        }
    }

    #region Swords
    public class Sword_TitanBlade : WeaponGenTemplate
    {
        public Sword_TitanBlade()
        {
            this.PrimaryType = WeaponTypes.Sword;
            this.Subtype = "Titan Blade";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            this.WeaponAttack = WeaponAttacks.Melee;
            this.Multiplier = 1.8;

            this.DamageMin = 60;
            this.DamageMax = 140;

            this.WeightMin = 80;
            this.WeightMax = 100;
        }
    }
    public class Sword_Claymore : WeaponGenTemplate
    {
        public Sword_Claymore()
        {
            this.PrimaryType = WeaponTypes.Sword;
            this.Subtype = "Claymore";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            this.WeaponAttack = WeaponAttacks.Melee;
            this.Multiplier = 1.8;

            this.DamageMin = 50;
            this.DamageMax = 110;

            this.WeightMin = 60;
            this.WeightMax = 75;
        }
    }
    public class Sword_LongSword : WeaponGenTemplate
    {
        public Sword_LongSword()
        {
            this.PrimaryType = WeaponTypes.Sword;
            this.Subtype = "Longsword";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.MainHand, EquipmentSlots.TwoHand };
            this.WeaponAttack = WeaponAttacks.Melee;
            this.Multiplier = 1.2;

            this.DamageMin = 50;
            this.DamageMax = 90;

            this.WeightMin = 40;
            this.WeightMax = 50;
        }
    }
    public class Sword_Katana : WeaponGenTemplate
    {
        public Sword_Katana()
        {
            this.PrimaryType = WeaponTypes.Sword;
            this.Subtype = "Katana";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.PhysicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand, EquipmentSlots.TwoHand };
            this.WeaponAttack = WeaponAttacks.Melee;
            this.Multiplier = 1.2;

            this.DamageMin = 55;
            this.DamageMax = 85;

            this.WeightMin = 30;
            this.WeightMax = 40;
        }
    }
    public class Sword_Rapier : WeaponGenTemplate
    {
        public Sword_Rapier()
        {
            this.PrimaryType = WeaponTypes.Sword;
            this.Subtype = "Rapier";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 32;
            this.DamageMax = 48;

            this.WeightMin = 16;
            this.WeightMax = 20;
        }
    }
    public class Sword_Gladius : WeaponGenTemplate
    {
        public Sword_Gladius()
        {
            this.PrimaryType = WeaponTypes.Sword;
            this.Subtype = "Gladius";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalPower, StatWeights.PhysicalSpeed };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 20;
            this.DamageMax = 40;

            this.WeightMin = 20;
            this.WeightMax = 25;
        }
    }
    public class Sword_Scimitar : WeaponGenTemplate
    {
        public Sword_Scimitar()
        {
            this.PrimaryType = WeaponTypes.Sword;
            this.Subtype = "Scimitar";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.MainHand, EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 30;
            this.DamageMax = 50;

            this.WeightMin = 20;
            this.WeightMax = 25;
        }
    }
    #endregion
    #region Axes
    public class Axe_Halberd : WeaponGenTemplate
    {
        public Axe_Halberd()
        {
            this.PrimaryType = WeaponTypes.Axe;
            this.Subtype = "Halberd";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            this.WeaponAttack = WeaponAttacks.Melee;
            this.Multiplier = 1.8;

            this.DamageMin = 45;
            this.DamageMax = 115;

            this.WeightMin = 70;
            this.WeightMax = 90;
        }
    }
    public class Axe_WarAxe : WeaponGenTemplate
    {
        public Axe_WarAxe()
        {
            this.PrimaryType = WeaponTypes.Axe;
            this.Subtype = "War Axe";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalPower, StatWeights.PhysicalSpeed };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand, EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;
            this.Multiplier = 1.2;

            this.DamageMin = 45;
            this.DamageMax = 95;

            this.WeightMin = 50;
            this.WeightMax = 65;
        }
    }
    public class Axe_Hatchet : WeaponGenTemplate
    {
        public Axe_Hatchet()
        {
            this.PrimaryType = WeaponTypes.Axe;
            this.Subtype = "Hatchet";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalPower ,StatWeights.PhysicalSpeed };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 20;
            this.DamageMax = 50;

            this.WeightMin = 30;
            this.WeightMax = 40;
        }
    }
    #endregion
    #region Maces
    public class Mace_Club : WeaponGenTemplate
    {
        public Mace_Club()
        {
            this.PrimaryType = WeaponTypes.Mace;
            this.Subtype = "Club";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            this.WeaponAttack = WeaponAttacks.Melee;
            this.Multiplier = 1.8;

            this.DamageMin = 30;
            this.DamageMax = 100;

            this.WeightMin = 70;
            this.WeightMax = 90;
        }
    }
    public class Mace_Mace : WeaponGenTemplate
    {
        public Mace_Mace()
        {
            this.PrimaryType = WeaponTypes.Mace;
            this.Subtype = "Mace";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.PhysicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand, EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;
            this.Multiplier = 1.2;

            this.DamageMin = 42;
            this.DamageMax = 78;

            this.WeightMin = 36;
            this.WeightMax = 45;
        }
    }
    public class Mace_MorningStar : WeaponGenTemplate
    {
        public Mace_MorningStar()
        {
            this.PrimaryType = WeaponTypes.Mace;
            this.Subtype = "Morning Star";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 45;
            this.DamageMax = 85;

            this.WeightMin = 40;
            this.WeightMax = 50;
        }
    }
    public class Mace_Hammer : WeaponGenTemplate
    {
        public Mace_Hammer()
        {
            this.PrimaryType = WeaponTypes.Mace;
            this.Subtype = "Hammer";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 30;
            this.DamageMax = 60;

            this.WeightMin = 30;
            this.WeightMax = 40;
        }
    }
    #endregion
    #region Flails
    public class Flail_Flail : WeaponGenTemplate
    {
        public Flail_Flail()
        {
            this.PrimaryType = WeaponTypes.Flail;
            this.Subtype = "Flail";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 45;
            this.DamageMax = 75;

            this.WeightMin = 30;
            this.WeightMax = 40;
        }
    }
    #endregion
    #region Sickles
    public class Sickle_Scythe : WeaponGenTemplate
    {
        public Sickle_Scythe()
        {
            this.PrimaryType = WeaponTypes.Sickle;
            this.Subtype = "Scythe";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.PhysicalPower, StatWeights.MagicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            this.WeaponAttack = WeaponAttacks.Melee;
            this.Multiplier = 1.8;

            this.DamageMin = 53;
            this.DamageMax = 79;

            this.WeightMin = 26;
            this.WeightMax = 35;
        }
    }
    public class Sickle_Sickle : WeaponGenTemplate
    {
        public Sickle_Sickle()
        {
            this.PrimaryType = WeaponTypes.Sickle;
            this.Subtype = "Sickle";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 12;
            this.DamageMax = 20;

            this.WeightMin = 8;
            this.WeightMax = 10;
        }
    }
    #endregion
    #region Whips
    public class Whip_CombatCross : WeaponGenTemplate
    {
        public Whip_CombatCross()
        {
            this.PrimaryType = WeaponTypes.Whip;
            this.Subtype = "Combat Cross";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.MagicalSpeed };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 22;
            this.DamageMax = 38;

            this.WeightMin = 16;
            this.WeightMax = 20;
        }
    }
    public class Whip_Whip : WeaponGenTemplate
    {
        public Whip_Whip()
        {
            this.PrimaryType = WeaponTypes.Whip;
            this.Subtype = "Whip";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 20;
            this.DamageMax = 30;

            this.WeightMin = 10;
            this.WeightMax = 15;
        }
    }
    #endregion
    #region Knives
    public class Knife_Cleaver : WeaponGenTemplate
    {
        public Knife_Cleaver()
        {
            this.PrimaryType = WeaponTypes.Knife;
            this.Subtype = "Cleaver";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.PhysicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 20;
            this.DamageMax = 30;

            this.WeightMin = 10;
            this.WeightMax = 15;
        }
    }
    public class Knife_Knife : WeaponGenTemplate
    {
        public Knife_Knife()
        {
            this.PrimaryType = WeaponTypes.Knife;
            this.Subtype = "Knife";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 16;
            this.DamageMax = 24;

            this.WeightMin = 8;
            this.WeightMax = 10;
        }
    }
    public class Knife_Dirge : WeaponGenTemplate
    {
        public Knife_Dirge()
        {
            this.PrimaryType = WeaponTypes.Knife;
            this.Subtype = "Dirge";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 13;
            this.DamageMax = 17;

            this.WeightMin = 4;
            this.WeightMax = 5;
        }
    }
    public class Knife_Dagger : WeaponGenTemplate
    {
        public Knife_Dagger()
        {
            this.PrimaryType = WeaponTypes.Knife;
            this.Subtype = "Dagger";
            this.WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.MagicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 12;
            this.DamageMax = 18;

            this.WeightMin = 6;
            this.WeightMax = 10;
        }
    }
    #endregion
    #region Staves
    public class Staff_Staff : WeaponGenTemplate
    {
        public Staff_Staff()
        {
            this.PrimaryType = WeaponTypes.Staff;
            this.Subtype = "Staff";
            this.WeaponStats = new StatWeights[] { StatWeights.MagicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            this.WeaponAttack = WeaponAttacks.Melee;
            this.Multiplier = 1.8;

            this.DamageMin = 2;
            this.DamageMax = 18;

            this.WeightMin = 16;
            this.WeightMax = 20;
        }
    }
    public class Staff_SwordCane : WeaponGenTemplate
    {
        public Staff_SwordCane()
        {
            this.PrimaryType = WeaponTypes.Staff;
            this.Subtype = "Sword Cane";
            this.WeaponStats = new StatWeights[] { StatWeights.MagicalPower, StatWeights.PhysicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand, EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;
            this.Multiplier = 1.2;

            this.DamageMin = 25;
            this.DamageMax = 45;

            this.WeightMin = 20;
            this.WeightMax = 25;
        }
    }
    public class Staff_QuarterStaff : WeaponGenTemplate
    {
        public Staff_QuarterStaff()
        {
            this.PrimaryType = WeaponTypes.Staff;
            this.Subtype = "Quarter Staff";
            this.WeaponStats = new StatWeights[] { StatWeights.MagicalPower };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            this.WeaponAttack = WeaponAttacks.Melee;

            this.DamageMin = 4;
            this.DamageMax = 12;

            this.WeightMin = 8;
            this.WeightMax = 10;
        }
    }
    #endregion

    public class Shield_KiteShield : WeaponGenTemplate
    {
        public Shield_KiteShield()
        {
            this.PrimaryType = WeaponTypes.Shield;
            this.Subtype = "Kite Shield";
            this.WeaponStats = new StatWeights[] { StatWeights.Defense };
            this.Slots = new EquipmentSlots[] { EquipmentSlots.OffHand };

            this.DamageMin = 0;
            this.DamageMax = 5;

            this.WeightMin = 20;
            this.WeightMax = 40;
        }
    }
}
