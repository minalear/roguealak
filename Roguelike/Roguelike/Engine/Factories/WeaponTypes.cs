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
            return Engine.RNG.Next(DamageMin, DamageMax);
        }
        public int GetRandomWeight()
        {
            return Engine.RNG.Next(WeightMin, WeightMax);
        }
        public StatWeights GetRandomStatWeight()
        {
            return WeaponStats[Engine.RNG.Next(0, WeaponStats.Length)];
        }
        public EquipmentSlots GetRandomSlot()
        {
            return Slots[Engine.RNG.Next(0, Slots.Length)];
        }

        public Weapon GenerateWeapon()
        {
            Weapon weapon = new Weapon(PrimaryType, GetRandomSlot());
            weapon.WeaponSubType = Subtype;
            weapon.BaseDamage = GetRandomDamage();
            weapon.Weight = GetRandomWeight();
            weapon.StatWeight = GetRandomStatWeight();

            return weapon;
        }
    }

    #region Swords
    public class Sword_TitanBlade : WeaponGenTemplate
    {
        public Sword_TitanBlade()
        {
            PrimaryType = WeaponTypes.Sword;
            Subtype = "Titan Blade";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            WeaponAttack = WeaponAttacks.Melee;
            Multiplier = 1.8;

            DamageMin = 60;
            DamageMax = 140;

            WeightMin = 80;
            WeightMax = 100;
        }
    }
    public class Sword_Claymore : WeaponGenTemplate
    {
        public Sword_Claymore()
        {
            PrimaryType = WeaponTypes.Sword;
            Subtype = "Claymore";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            WeaponAttack = WeaponAttacks.Melee;
            Multiplier = 1.8;

            DamageMin = 50;
            DamageMax = 110;

            WeightMin = 60;
            WeightMax = 75;
        }
    }
    public class Sword_LongSword : WeaponGenTemplate
    {
        public Sword_LongSword()
        {
            PrimaryType = WeaponTypes.Sword;
            Subtype = "Longsword";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.MainHand, EquipmentSlots.TwoHand };
            WeaponAttack = WeaponAttacks.Melee;
            Multiplier = 1.2;

            DamageMin = 50;
            DamageMax = 90;

            WeightMin = 40;
            WeightMax = 50;
        }
    }
    public class Sword_Katana : WeaponGenTemplate
    {
        public Sword_Katana()
        {
            PrimaryType = WeaponTypes.Sword;
            Subtype = "Katana";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.PhysicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand, EquipmentSlots.TwoHand };
            WeaponAttack = WeaponAttacks.Melee;
            Multiplier = 1.2;

            DamageMin = 55;
            DamageMax = 85;

            WeightMin = 30;
            WeightMax = 40;
        }
    }
    public class Sword_Rapier : WeaponGenTemplate
    {
        public Sword_Rapier()
        {
            PrimaryType = WeaponTypes.Sword;
            Subtype = "Rapier";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 32;
            DamageMax = 48;

            WeightMin = 16;
            WeightMax = 20;
        }
    }
    public class Sword_Gladius : WeaponGenTemplate
    {
        public Sword_Gladius()
        {
            PrimaryType = WeaponTypes.Sword;
            Subtype = "Gladius";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalPower, StatWeights.PhysicalSpeed };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 20;
            DamageMax = 40;

            WeightMin = 20;
            WeightMax = 25;
        }
    }
    public class Sword_Scimitar : WeaponGenTemplate
    {
        public Sword_Scimitar()
        {
            PrimaryType = WeaponTypes.Sword;
            Subtype = "Scimitar";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            Slots = new EquipmentSlots[] { EquipmentSlots.MainHand, EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 30;
            DamageMax = 50;

            WeightMin = 20;
            WeightMax = 25;
        }
    }
    #endregion
    #region Axes
    public class Axe_Halberd : WeaponGenTemplate
    {
        public Axe_Halberd()
        {
            PrimaryType = WeaponTypes.Axe;
            Subtype = "Halberd";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            WeaponAttack = WeaponAttacks.Melee;
            Multiplier = 1.8;

            DamageMin = 45;
            DamageMax = 115;

            WeightMin = 70;
            WeightMax = 90;
        }
    }
    public class Axe_WarAxe : WeaponGenTemplate
    {
        public Axe_WarAxe()
        {
            PrimaryType = WeaponTypes.Axe;
            Subtype = "War Axe";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalPower, StatWeights.PhysicalSpeed };
            Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand, EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;
            Multiplier = 1.2;

            DamageMin = 45;
            DamageMax = 95;

            WeightMin = 50;
            WeightMax = 65;
        }
    }
    public class Axe_Hatchet : WeaponGenTemplate
    {
        public Axe_Hatchet()
        {
            PrimaryType = WeaponTypes.Axe;
            Subtype = "Hatchet";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalPower ,StatWeights.PhysicalSpeed };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 20;
            DamageMax = 50;

            WeightMin = 30;
            WeightMax = 40;
        }
    }
    #endregion
    #region Maces
    public class Mace_Club : WeaponGenTemplate
    {
        public Mace_Club()
        {
            PrimaryType = WeaponTypes.Mace;
            Subtype = "Club";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            WeaponAttack = WeaponAttacks.Melee;
            Multiplier = 1.8;

            DamageMin = 30;
            DamageMax = 100;

            WeightMin = 70;
            WeightMax = 90;
        }
    }
    public class Mace_Mace : WeaponGenTemplate
    {
        public Mace_Mace()
        {
            PrimaryType = WeaponTypes.Mace;
            Subtype = "Mace";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.PhysicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand, EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;
            Multiplier = 1.2;

            DamageMin = 42;
            DamageMax = 78;

            WeightMin = 36;
            WeightMax = 45;
        }
    }
    public class Mace_MorningStar : WeaponGenTemplate
    {
        public Mace_MorningStar()
        {
            PrimaryType = WeaponTypes.Mace;
            Subtype = "Morning Star";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 45;
            DamageMax = 85;

            WeightMin = 40;
            WeightMax = 50;
        }
    }
    public class Mace_Hammer : WeaponGenTemplate
    {
        public Mace_Hammer()
        {
            PrimaryType = WeaponTypes.Mace;
            Subtype = "Hammer";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 30;
            DamageMax = 60;

            WeightMin = 30;
            WeightMax = 40;
        }
    }
    #endregion
    #region Flails
    public class Flail_Flail : WeaponGenTemplate
    {
        public Flail_Flail()
        {
            PrimaryType = WeaponTypes.Flail;
            Subtype = "Flail";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 45;
            DamageMax = 75;

            WeightMin = 30;
            WeightMax = 40;
        }
    }
    #endregion
    #region Sickles
    public class Sickle_Scythe : WeaponGenTemplate
    {
        public Sickle_Scythe()
        {
            PrimaryType = WeaponTypes.Sickle;
            Subtype = "Scythe";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.PhysicalPower, StatWeights.MagicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            WeaponAttack = WeaponAttacks.Melee;
            Multiplier = 1.8;

            DamageMin = 53;
            DamageMax = 79;

            WeightMin = 26;
            WeightMax = 35;
        }
    }
    public class Sickle_Sickle : WeaponGenTemplate
    {
        public Sickle_Sickle()
        {
            PrimaryType = WeaponTypes.Sickle;
            Subtype = "Sickle";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 12;
            DamageMax = 20;

            WeightMin = 8;
            WeightMax = 10;
        }
    }
    #endregion
    #region Whips
    public class Whip_CombatCross : WeaponGenTemplate
    {
        public Whip_CombatCross()
        {
            PrimaryType = WeaponTypes.Whip;
            Subtype = "Combat Cross";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.MagicalSpeed };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 22;
            DamageMax = 38;

            WeightMin = 16;
            WeightMax = 20;
        }
    }
    public class Whip_Whip : WeaponGenTemplate
    {
        public Whip_Whip()
        {
            PrimaryType = WeaponTypes.Whip;
            Subtype = "Whip";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 20;
            DamageMax = 30;

            WeightMin = 10;
            WeightMax = 15;
        }
    }
    #endregion
    #region Knives
    public class Knife_Cleaver : WeaponGenTemplate
    {
        public Knife_Cleaver()
        {
            PrimaryType = WeaponTypes.Knife;
            Subtype = "Cleaver";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.PhysicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 20;
            DamageMax = 30;

            WeightMin = 10;
            WeightMax = 15;
        }
    }
    public class Knife_Knife : WeaponGenTemplate
    {
        public Knife_Knife()
        {
            PrimaryType = WeaponTypes.Knife;
            Subtype = "Knife";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 16;
            DamageMax = 24;

            WeightMin = 8;
            WeightMax = 10;
        }
    }
    public class Knife_Dirge : WeaponGenTemplate
    {
        public Knife_Dirge()
        {
            PrimaryType = WeaponTypes.Knife;
            Subtype = "Dirge";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 13;
            DamageMax = 17;

            WeightMin = 4;
            WeightMax = 5;
        }
    }
    public class Knife_Dagger : WeaponGenTemplate
    {
        public Knife_Dagger()
        {
            PrimaryType = WeaponTypes.Knife;
            Subtype = "Dagger";
            WeaponStats = new StatWeights[] { StatWeights.PhysicalSpeed, StatWeights.MagicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 12;
            DamageMax = 18;

            WeightMin = 6;
            WeightMax = 10;
        }
    }
    #endregion
    #region Staves
    public class Staff_Staff : WeaponGenTemplate
    {
        public Staff_Staff()
        {
            PrimaryType = WeaponTypes.Staff;
            Subtype = "Staff";
            WeaponStats = new StatWeights[] { StatWeights.MagicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand };
            WeaponAttack = WeaponAttacks.Melee;
            Multiplier = 1.8;

            DamageMin = 2;
            DamageMax = 18;

            WeightMin = 16;
            WeightMax = 20;
        }
    }
    public class Staff_SwordCane : WeaponGenTemplate
    {
        public Staff_SwordCane()
        {
            PrimaryType = WeaponTypes.Staff;
            Subtype = "Sword Cane";
            WeaponStats = new StatWeights[] { StatWeights.MagicalPower, StatWeights.PhysicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.TwoHand, EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;
            Multiplier = 1.2;

            DamageMin = 25;
            DamageMax = 45;

            WeightMin = 20;
            WeightMax = 25;
        }
    }
    public class Staff_QuarterStaff : WeaponGenTemplate
    {
        public Staff_QuarterStaff()
        {
            PrimaryType = WeaponTypes.Staff;
            Subtype = "Quarter Staff";
            WeaponStats = new StatWeights[] { StatWeights.MagicalPower };
            Slots = new EquipmentSlots[] { EquipmentSlots.OneHand };
            WeaponAttack = WeaponAttacks.Melee;

            DamageMin = 4;
            DamageMax = 12;

            WeightMin = 8;
            WeightMax = 10;
        }
    }
    #endregion

    public class Shield_KiteShield : WeaponGenTemplate
    {
        public Shield_KiteShield()
        {
            PrimaryType = WeaponTypes.Shield;
            Subtype = "Kite Shield";
            WeaponStats = new StatWeights[] { StatWeights.Defense };
            Slots = new EquipmentSlots[] { EquipmentSlots.OffHand };

            DamageMin = 0;
            DamageMax = 5;

            WeightMin = 20;
            WeightMax = 40;
        }
    }
}
