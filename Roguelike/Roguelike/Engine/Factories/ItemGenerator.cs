using System;
using Roguelike.Engine.Game;
using Roguelike.Engine.Game.Items;
using Roguelike.Engine.Game.Stats;
using Roguelike.Engine.Game.Combat;

using Roguelike.Engine.Factories.Weapons;
using Roguelike.Engine.Factories.Armor;
using Roguelike.Engine.Factories.Consumables;

namespace Roguelike.Engine.Factories
{
    public static class ItemGenerator
    {
        public static Item GenerateRandomItem()
        {
            int result = RNG.Next(0, 100);
            if (result <= 20)
                return WeaponGenerator.GenerateRandomWeapon();
            else if (result <= 40)
                return ConsumableGenerator.GeneratePotion();
            else if (result <= 60)
                return ConsumableGenerator.GenerateScroll();
            else if (result <= 80)
                return ArmorGenerator.GenerateRandomArmor();

            return ConsumableGenerator.GenerateFood();
        }
        private static ItemTypes getRandomItemType()
        {
            return (ItemTypes)itemTypes.GetValue(RNG.Next(0, itemTypes.Length));
        }

        public static Weapon GenerateRandomWeapon()
        {
            return WeaponGenerator.GenerateRandomWeapon();
        }
        public static Weapon GenerateRandomWeapon(Rarities rarity)
        {
            return WeaponGenerator.GenerateRandomWeapon(rarity);
        }

        private static Array itemTypes = Enum.GetValues(typeof(ItemTypes));
        private static Array weaponTypes = Enum.GetValues(typeof(Factories.WeaponTypes));
    }
}

namespace Roguelike.Engine.Factories.Weapons
{
    public static class WeaponGenerator
    {
        private static Array weaponTypes = Enum.GetValues(typeof(Factories.WeaponTypes));

        public static Weapon GenerateRandomWeapon()
        {
            int rarity = getRandomRarityLevel();

            WeaponGenTemplate template = generateRandomWeaponFramework();
            Weapon randomWeapon = template.GenerateWeapon();

            randomWeapon.ItemRarity = ((Rarities)rarity - 1);
            randomWeapon.Name = getRandomWeaponName(randomWeapon.StatWeight, randomWeapon.WeaponSubType, randomWeapon.ItemRarity);
            randomWeapon.ModPackage = getRandomStats(randomWeapon.StatWeight, rarity, template.Multiplier);

            calculateWeaponValue(randomWeapon);

            return randomWeapon;
        }
        public static Weapon GenerateRandomWeapon(Rarities weaponRarity)
        {
            int rarity = (int)weaponRarity + 1;

            WeaponGenTemplate template = generateRandomWeaponFramework();
            Weapon randomWeapon = template.GenerateWeapon();

            randomWeapon.ItemRarity = weaponRarity;
            randomWeapon.Name = getRandomWeaponName(randomWeapon.StatWeight, randomWeapon.WeaponSubType, weaponRarity);
            randomWeapon.ModPackage = getRandomStats(randomWeapon.StatWeight, rarity, template.Multiplier);

            calculateWeaponValue(randomWeapon);

            return randomWeapon;
        }

        private static string getRandomWeaponName(StatWeights weight, string subtype, Rarities rarity)
        {
            string name = string.Empty;

            #region Prefixes
            if (rarity == Rarities.Common) //Common
            {
                string[] tags = new string[] { string.Empty, "Basic ", "Chipped ", "Rusted " };
                name += tags[RNG.Next(0, tags.Length)] + subtype;

                return name;
            }
            else if (rarity == Rarities.Uncommon) //Uncommon
            {
                string[] tags = new string[] { string.Empty, "Enhanced ", "Solid ", "Improved ", "Hardened ", "Battle-worn " };
                name += tags[RNG.Next(0, tags.Length)] + subtype;

                if (RNG.Next(0, 100) <= 75)
                    return name;
            }
            else if (rarity == Rarities.Rare) //Rare
            {
                string[] tags = new string[] { string.Empty, "Greater ", "Rare ", "Enchanted ", "Mithril ", "Reinforced " };
                name += tags[RNG.Next(0, tags.Length)] + subtype;

                if (RNG.Next(0, 100) <= 45)
                    return name;
            }
            else if (rarity == Rarities.Epic) //Epic
            {
                string[] tags = new string[] { string.Empty, "Insane ", "Chaotic ", "Mythical ", "Infused " };
                name += tags[RNG.Next(0, tags.Length)] + subtype;

                if (RNG.Next(0, 100) <= 15)
                    return name;
            }
            else if (rarity == Rarities.Legendary) //Legendary
            {
                string[] tags = new string[] { "Legendary ", "Cataclysmic ", "Dragonbane ", "Godlike " };
                name += tags[RNG.Next(0, tags.Length)] + subtype;
            }
            else if (rarity == Rarities.Unique) //Unique
            {
                string[] tags = new string[] { "Unique " };
                name += tags[RNG.Next(0, tags.Length)] + subtype;
            }
            #endregion

            name += " of ";

            if (weight == StatWeights.Defense)
            {
                string[] tags = new string[] { "the Sentinel", "the Whale", "the Tank", "the Behemoth", "the Iron Brigade", "the Steelskin", "the Fortress" };
                name += tags[RNG.Next(0, tags.Length)];
            }
            else if (weight == StatWeights.MagicalPower)
            {
                string[] tags = new string[] { "the Hawk", "the Owl", "the Eagle", "the Arcane", "the Spellsword", "the Dragon", "the Mind" };
                name += tags[RNG.Next(0, tags.Length)];
            }
            else if (weight == StatWeights.MagicalSpeed)
            {
                string[] tags = new string[] { "the Sparrow", "the Swift Mind", "the Talon", "the Flame", "Lightning", "the Lover", "the Magician" };
                name += tags[RNG.Next(0, tags.Length)];
            }
            else if (weight == StatWeights.PhysicalPower)
            {
                string[] tags = new string[] { "the Bear", "the Fist", "the Hammer Lords", "Power", "the Caber Toss", "the Ram", "the Brute" };
                name += tags[RNG.Next(0, tags.Length)];
            }
            else if (weight == StatWeights.PhysicalSpeed)
            {
                string[] tags = new string[] { "the Monkey", "the Feline", "the Snake", "the Cobra", "the Bandit", "the Marksman", "the Zephyr" };
                name += tags[RNG.Next(0, tags.Length)];
            }

            return name;
        }

        private static Factories.WeaponTypes getRandomWeaponType()
        {
            Array weaponTypes = Enum.GetValues(typeof(Factories.WeaponTypes));
            return (Factories.WeaponTypes)weaponTypes.GetValue(RNG.Next(0, weaponTypes.Length));
        }
        private static int getRandomRarityLevel()
        {
            //1 - Common, 2 - Uncommon, 3 - Rare, 4 - Epic, 5 - Legendary, 6 - Unique
            int result = RNG.Next(0, 1000);
            if (result < 800)
                return 1;
            else if (result < 900)
                return 2;
            else if (result < 970)
                return 3;
            else if (result < 990)
                return 4;
            else if (result < 998)
                return 5;
            else if (result < 1000)
                return 6;

            return 1;
        }
        private static void calculateWeaponValue(Weapon weapon)
        {
            double value = 0;
            if (weapon.ItemRarity == Rarities.Common)
                value += 1;
            else if (weapon.ItemRarity == Rarities.Uncommon)
                value += 8;
            else if (weapon.ItemRarity == Rarities.Rare)
                value += 16;
            else if (weapon.ItemRarity == Rarities.Epic)
                value += 30;
            else if (weapon.ItemRarity == Rarities.Legendary)
                value += 65;
            else if (weapon.ItemRarity == Rarities.Unique)
                value += 127;

            value += weapon.ModPackage.AttackPower * 1.0;
            value += weapon.ModPackage.PhysicalHitChance * 0.5;
            value += weapon.ModPackage.PhysicalCritChance * 0.75;
            value += weapon.ModPackage.PhysicalCritPower * 5;
            value += weapon.ModPackage.PhysicalHaste * 0.5;
            value += weapon.ModPackage.PhysicalReduction * 1.0;
            value += weapon.ModPackage.PhysicalReflection * 8.0;
            value += weapon.ModPackage.PhysicalAvoidance * 4.0;

            value += weapon.ModPackage.SpellPower * 1.5;
            value += weapon.ModPackage.SpellHitChance * 0.75;
            value += weapon.ModPackage.SpellCritChance * 1;
            value += weapon.ModPackage.SpellCritPower * 10;
            value += weapon.ModPackage.SpellHaste * 1;
            value += weapon.ModPackage.SpellReduction * 2;
            value += weapon.ModPackage.SpellReflection * 16;
            value += weapon.ModPackage.SpellAvoidance * 8;

            value += weapon.ModPackage.BonusHealth * 1.0;
            value += weapon.ModPackage.BonusMana * 2.0;

            weapon.Value = (int)value;
        }

        private static WeaponGenTemplate generateRandomWeaponFramework()
        {
            return templates[RNG.Next(0, templates.Length)];
        }
        private static StatWeights getStatWeight(Factories.WeaponTypes type)
        {
            if (type == Factories.WeaponTypes.Axe)
                return StatWeights.PhysicalPower;
            else if (type == Factories.WeaponTypes.Flail)
                return StatWeights.PhysicalPower;
            else if (type == Factories.WeaponTypes.Knife)
                return StatWeights.PhysicalSpeed;
            else if (type == Factories.WeaponTypes.Mace)
                return StatWeights.PhysicalPower;
            else if (type == Factories.WeaponTypes.Sickle)
                return StatWeights.PhysicalSpeed;
            else if (type == Factories.WeaponTypes.Staff)
                return StatWeights.MagicalPower;
            else if (type == Factories.WeaponTypes.Sword)
                return StatWeights.PhysicalSpeed;
            else if (type == Factories.WeaponTypes.Whip)
                return StatWeights.PhysicalSpeed;
            else if (type == Factories.WeaponTypes.Shield)
                return StatWeights.Defense;

            return StatWeights.PhysicalPower;
        }

        private static ModPackage getRandomStats(StatWeights weight, int rarityLevel, double baseMultiplier)
        {
            if (weight == StatWeights.Defense)
                return getDefenseStats(rarityLevel, baseMultiplier);
            else if (weight == StatWeights.MagicalPower)
                return getMagicalPowerStats(rarityLevel, baseMultiplier);
            else if (weight == StatWeights.MagicalSpeed)
                return getMagicalSpeedStats(rarityLevel, baseMultiplier);
            else if (weight == StatWeights.PhysicalPower)
                return getPhysicalPowerStats(rarityLevel, baseMultiplier);
            else if (weight == StatWeights.PhysicalSpeed)
                return getPhysicalSpeedStats(rarityLevel, baseMultiplier);

            return getPhysicalPowerStats(rarityLevel, baseMultiplier);
        }
        private static ModPackage getPhysicalPowerStats(int rarityLevel, double baseMultiplier)
        {
            ModPackage package = new ModPackage();

            double multiplier = baseMultiplier;
            if (rarityLevel == 3)
                multiplier += 0.2;
            else if (rarityLevel == 4)
                multiplier += 0.45;
            else if (rarityLevel == 5)
                multiplier += 1.0;
            else if (rarityLevel == 6)
                multiplier += 4.0;

            for (int i = 0; i < rarityLevel; i++)
            {
                //1  - 50  -  Attack Power
                //51 - 70  -  Crit Power
                //71 - 90  -  Hit Chance
                //91 - 95  -  Crit Chance
                //96 - 100 -  Haste

                int result = RNG.Next(1, 101);

                if (result <= 50)
                    package.AttackPower += (RNG.NextDouble(1, 10) * multiplier).Truncate(2);
                else if (result <= 70)
                    package.PhysicalCritPower += (RNG.NextDouble(0.1, 0.3) * multiplier).Truncate(2);
                else if (result <= 90)
                    package.PhysicalHitChance += (RNG.NextDouble(2, 6) * multiplier).Truncate(2);
                else if (result <= 95)
                    package.PhysicalCritChance += (RNG.NextDouble(1, 3) * multiplier).Truncate(2);
                else if (result <= 100)
                    package.PhysicalHaste += (RNG.NextDouble(1, 4) * multiplier).Truncate(2);
            }
            return package;
        }
        private static ModPackage getPhysicalSpeedStats(int rarityLevel, double baseMultiplier)
        {
            ModPackage package = new ModPackage();

            double multiplier = baseMultiplier;
            if (rarityLevel == 3)
                multiplier += 0.2;
            else if (rarityLevel == 4)
                multiplier += 0.45;
            else if (rarityLevel == 5)
                multiplier += 1.0;
            else if (rarityLevel == 6)
                multiplier += 4.0;

            for (int i = 0; i < rarityLevel; i++)
            {
                //1  -  30  -  Crit Chance
                //31 -  60  -  Hit Chance
                //61 -  75  -  Haste
                //76 -  90  -  Attack Power
                //91 -  100 -  Crit Power

                int result = RNG.Next(1, 101);

                if (result <= 30)
                    package.PhysicalCritChance += (RNG.NextDouble(1.0, 3.0) * multiplier).Truncate(2);
                else if (result <= 60)
                    package.PhysicalHitChance += (RNG.NextDouble(2.0, 6.0) * multiplier).Truncate(2);
                else if (result <= 75)
                    package.PhysicalHaste += (RNG.NextDouble(1.0, 4.0) * multiplier).Truncate(2);
                else if (result <= 90)
                    package.AttackPower += (RNG.NextDouble(1, 10) * multiplier).Truncate(2);
                else if (result <= 100)
                    package.PhysicalCritPower += (RNG.NextDouble(0.1, 0.3) * multiplier).Truncate(2);
            }
            return package;
        }
        private static ModPackage getDefenseStats(int rarityLevel, double baseMultiplier)
        {
            ModPackage package = new ModPackage();

            double multiplier = baseMultiplier;
            if (rarityLevel == 3)
                multiplier += 0.2;
            else if (rarityLevel == 4)
                multiplier += 0.45;
            else if (rarityLevel == 5)
                multiplier += 1.0;
            else if (rarityLevel == 6)
                multiplier += 4.0;

            for (int i = 0; i < rarityLevel; i++)
            {
                //1  -  30  -  Health
                //31 -  60  -  P Reduction
                //61 -  75  -  S Reduction
                //76 -  90  -  S Avoidance
                //91 -  100 -  Health Regen

                int result = RNG.Next(1, 101);

                if (result <= 30)
                    package.BonusHealth += (int)(RNG.NextDouble(10, 25) * multiplier).Truncate(2);
                else if (result <= 60)
                    package.PhysicalReduction += (RNG.NextDouble(0.25, 1.5) * multiplier).Truncate(2);
                else if (result <= 75)
                    package.SpellReduction += (RNG.NextDouble(0.2, 1.0) * multiplier).Truncate(2);
                else if (result <= 90)
                    package.SpellAvoidance += (RNG.NextDouble(0.1, 0.5) * multiplier).Truncate(2);
                else if (result <= 100)
                    package.BonusHealth += (int)(RNG.NextDouble(10, 25) * multiplier).Truncate(2); //TODO: Add Health regen
            }
            return package;
        }
        private static ModPackage getMagicalPowerStats(int rarityLevel, double baseMultiplier)
        {
            ModPackage package = new ModPackage();

            double multiplier = baseMultiplier;
            if (rarityLevel == 3)
                multiplier += 0.2;
            else if (rarityLevel == 4)
                multiplier += 0.45;
            else if (rarityLevel == 5)
                multiplier += 1.0;
            else if (rarityLevel == 6)
                multiplier += 4.0;

            for (int i = 0; i < rarityLevel; i++)
            {
                //1  - 50  -  Spell Power
                //51 - 70  -  Crit Power
                //71 - 90  -  Hit Chance
                //91 - 95  -  Crit Chance
                //96 - 100 -  Haste

                int result = RNG.Next(1, 101);

                if (result <= 50)
                    package.SpellPower += (RNG.NextDouble(1, 10) * multiplier).Truncate(2);
                else if (result <= 70)
                    package.SpellCritPower += (RNG.NextDouble(0.1, 0.3) * multiplier).Truncate(2);
                else if (result <= 90)
                    package.SpellHitChance += (RNG.NextDouble(2, 6) * multiplier).Truncate(2);
                else if (result <= 95)
                    package.SpellCritChance += (RNG.NextDouble(1, 3) * multiplier).Truncate(2);
                else if (result <= 100)
                    package.SpellHaste += (RNG.NextDouble(1, 4) * multiplier).Truncate(2);
            }
            return package;
        }
        private static ModPackage getMagicalSpeedStats(int rarityLevel, double baseMultiplier)
        {
            ModPackage package = new ModPackage();

            double multiplier = baseMultiplier;
            if (rarityLevel == 3)
                multiplier += 0.2;
            else if (rarityLevel == 4)
                multiplier += 0.45;
            else if (rarityLevel == 5)
                multiplier += 1.0;
            else if (rarityLevel == 6)
                multiplier += 4.0;

            for (int i = 0; i < rarityLevel; i++)
            {
                //1  -  30  -  Haste
                //31 -  60  -  Crit Chance
                //61 -  75  -  Hit Chance
                //76 -  90  -  Spell Power
                //91 -  100 -  Crit Power

                int result = RNG.Next(1, 101);

                if (result <= 30)
                    package.SpellHaste += (RNG.NextDouble(1, 4) * multiplier).Truncate(2);
                else if (result <= 60)
                    package.PhysicalCritChance += (RNG.NextDouble(1, 3) * multiplier).Truncate(2);
                else if (result <= 75)
                    package.SpellHitChance += (RNG.NextDouble(2, 6) * multiplier).Truncate(2);
                else if (result <= 90)
                    package.SpellPower += (RNG.NextDouble(1, 10) * multiplier).Truncate(2);
                else if (result <= 100)
                    package.SpellCritPower += (RNG.NextDouble(0.1, 0.3) * multiplier).Truncate(2);
            }
            return package;
        }

        private static WeaponGenTemplate[] templates = new WeaponGenTemplate[] 
            { 
                #region Templates
                new Sword_TitanBlade(),
                new Sword_Claymore(),
                new Sword_LongSword(),
                new Sword_Katana(),
                new Sword_Rapier(),
                new Sword_Gladius(),
                new Sword_Scimitar(),

                new Axe_Halberd(), 
                new Axe_WarAxe(),
                new Axe_Hatchet(),

                new Mace_Club(),
                new Mace_Mace(),
                new Mace_MorningStar(),
                new Mace_Hammer(),

                new Flail_Flail(),

                new Sickle_Scythe(), 
                new Sickle_Sickle(),

                new Whip_CombatCross(),
                new Whip_Whip(), 

                new Knife_Cleaver(),
                new Knife_Knife(),
                new Knife_Dirge(),
                new Knife_Dagger(),

                new Staff_Staff(),
                new Staff_SwordCane(),
                new Staff_QuarterStaff(),

                new Shield_KiteShield()
                #endregion
            };
    }
}
namespace Roguelike.Engine.Factories.Armor
{
    public static class ArmorGenerator
    {
        public static Equipment GenerateRandomArmor()
        {
            Equipment armorPiece = new Equipment(getRandomSlot());
            int rarity = getRandomRarityLevel();

            armorPiece.ItemRarity = (Rarities)(rarity - 1);
            armorPiece.Name = getRandomName(armorPiece.EquipSlot);
            armorPiece.Value = RNG.Next(2, 150);
            armorPiece.ModPackage = getRandomStats(getStatWeight(), rarity, 1.0);
            armorPiece.Description = armorPiece.Name + " - " + armorPiece.EquipSlot.ToString();

            return armorPiece;
        }

        private static int getRandomRarityLevel()
        {
            //1 - Common, 2 - Uncommon, 3 - Rare, 4 - Epic, 5 - Legendary, 6 - Unique
            int result = RNG.Next(0, 1000);
            if (result < 800)
                return 1;
            else if (result < 900)
                return 2;
            else if (result < 970)
                return 3;
            else if (result < 990)
                return 4;
            else if (result < 998)
                return 5;
            else if (result < 1000)
                return 6;

            return 1;
        }
        private static EquipmentSlots getRandomSlot()
        {
            Array slots = Enum.GetValues(typeof(EquipmentSlots));
            return (EquipmentSlots)slots.GetValue(RNG.Next(0, slots.Length - 4)); //Don't include the weapon slots
        }
        private static string getRandomName(EquipmentSlots slot)
        {
            string name = string.Empty;

            int result = RNG.Next(0, 100);
            if (result <= 15)
            {
                string[] prefixes = new string[] { "Studded", "Improved", "Forged", "Reforged", "Ehanced", "Bolstered" };
                name += prefixes[RNG.Next(0, prefixes.Length)] + " ";
            }

            if (slot == EquipmentSlots.Head)
            {
                string[] prefixes = new string[] { "Helm", "Helmet", "Cap", "Hat", "Hood", "Sombrero", "Mask" };
                name += prefixes[RNG.Next(0, prefixes.Length)] + " of ";
            }
            else if (slot == EquipmentSlots.Shoulder)
            {
                string[] prefixes = new string[] { "Shoulderpads", "Cape", "Scarf", "Cloak", "Animal Pelt", "Living Monkey" };
                name += prefixes[RNG.Next(0, prefixes.Length)] + " of ";
            }
            else if (slot == EquipmentSlots.Chest)
            {
                string[] prefixes = new string[] { "Chestpiece", "Jerkin", "Chestplate", "Tanktop", "Robe" };
                name += prefixes[RNG.Next(0, prefixes.Length)] + " of ";
            }
            else if (slot == EquipmentSlots.Legs)
            {
                string[] prefixes = new string[] { "Leggings", "Greaves", "Pants", "Shorts", "Kilt", "Skirt", "Short Shorts", "Capris", "Underwear" };
                name += prefixes[RNG.Next(0, prefixes.Length)] + " of ";
            }
            else if (slot == EquipmentSlots.Boots)
            {
                string[] prefixes = new string[] { "Boots", "Shoes", "Sneakers", "Sneaks", "Sandles", "Crocs" };
                name += prefixes[RNG.Next(0, prefixes.Length)] + " of ";
            }
            else if (slot == EquipmentSlots.Gloves)
            {
                string[] prefixes = new string[] { "Gloves", "Gauntlet", "Fingerless Gloves", "Mittens", "Oven Mits", "Socks" };
                name += prefixes[RNG.Next(0, prefixes.Length)] + " of ";
            }
            else if (slot == EquipmentSlots.Neck)
            {
                string[] prefixes = new string[] { "Necklace", "Amulet", "Choker", "Locket", "Pendant" };
                name += prefixes[RNG.Next(0, prefixes.Length)] + " of ";
            }
            else if (slot == EquipmentSlots.Ring)
            {
                string[] prefixes = new string[] { "Ring", "Band", "Signet" };
                name += prefixes[RNG.Next(0, prefixes.Length)] + " of ";
            }
            else if (slot == EquipmentSlots.Relic)
            {
                string[] prefixes = new string[] { "Relic", "Totem", "Heirloom", "Fetish" };
                name += prefixes[RNG.Next(0, prefixes.Length)] + " of ";
            }

            string[] suffixes = new string[] { "Power", "the Smith", "Idiocy", "the Flinnan" };
            name += suffixes[RNG.Next(0, suffixes.Length)];

            return name;
        }

        private static StatWeights getStatWeight()
        {
            Array weights = Enum.GetValues(typeof(StatWeights));
            return (StatWeights)weights.GetValue(RNG.Next(0, weights.Length));
        }

        private static ModPackage getRandomStats(StatWeights weight, int rarityLevel, double baseMultiplier)
        {
            if (weight == StatWeights.Defense)
                return getDefenseStats(rarityLevel, baseMultiplier);
            else if (weight == StatWeights.MagicalPower)
                return getMagicalPowerStats(rarityLevel, baseMultiplier);
            else if (weight == StatWeights.MagicalSpeed)
                return getMagicalSpeedStats(rarityLevel, baseMultiplier);
            else if (weight == StatWeights.PhysicalPower)
                return getPhysicalPowerStats(rarityLevel, baseMultiplier);
            else if (weight == StatWeights.PhysicalSpeed)
                return getPhysicalSpeedStats(rarityLevel, baseMultiplier);

            return getPhysicalPowerStats(rarityLevel, baseMultiplier);
        }
        private static ModPackage getPhysicalPowerStats(int rarityLevel, double baseMultiplier)
        {
            ModPackage package = new ModPackage();

            double multiplier = baseMultiplier;
            if (rarityLevel == 3)
                multiplier += 0.2;
            else if (rarityLevel == 4)
                multiplier += 0.45;
            else if (rarityLevel == 5)
                multiplier += 1.0;
            else if (rarityLevel == 6)
                multiplier += 4.0;

            for (int i = 0; i < rarityLevel; i++)
            {
                //1  - 50  -  Attack Power
                //51 - 70  -  Crit Power
                //71 - 90  -  Hit Chance
                //91 - 95  -  Crit Chance
                //96 - 100 -  Haste

                int result = RNG.Next(1, 101);

                if (result <= 50)
                    package.AttackPower += (RNG.NextDouble(1, 10) * multiplier).Truncate(2);
                else if (result <= 70)
                    package.PhysicalCritPower += (RNG.NextDouble(0.1, 0.3) * multiplier).Truncate(2);
                else if (result <= 90)
                    package.PhysicalHitChance += (RNG.NextDouble(2, 6) * multiplier).Truncate(2);
                else if (result <= 95)
                    package.PhysicalCritChance += (RNG.NextDouble(1, 3) * multiplier).Truncate(2);
                else if (result <= 100)
                    package.PhysicalHaste += (RNG.NextDouble(1, 4) * multiplier).Truncate(2);
            }
            return package;
        }
        private static ModPackage getPhysicalSpeedStats(int rarityLevel, double baseMultiplier)
        {
            ModPackage package = new ModPackage();

            double multiplier = baseMultiplier;
            if (rarityLevel == 3)
                multiplier += 0.2;
            else if (rarityLevel == 4)
                multiplier += 0.45;
            else if (rarityLevel == 5)
                multiplier += 1.0;
            else if (rarityLevel == 6)
                multiplier += 4.0;

            for (int i = 0; i < rarityLevel; i++)
            {
                //1  -  30  -  Crit Chance
                //31 -  60  -  Hit Chance
                //61 -  75  -  Haste
                //76 -  90  -  Attack Power
                //91 -  100 -  Crit Power

                int result = RNG.Next(1, 101);

                if (result <= 30)
                    package.PhysicalCritChance += (RNG.NextDouble(1.0, 3.0) * multiplier).Truncate(2);
                else if (result <= 60)
                    package.PhysicalHitChance += (RNG.NextDouble(2.0, 6.0) * multiplier).Truncate(2);
                else if (result <= 75)
                    package.PhysicalHaste += (RNG.NextDouble(1.0, 4.0) * multiplier).Truncate(2);
                else if (result <= 90)
                    package.AttackPower += (RNG.NextDouble(1, 10) * multiplier).Truncate(2);
                else if (result <= 100)
                    package.PhysicalCritPower += (RNG.NextDouble(0.1, 0.3) * multiplier).Truncate(2);
            }
            return package;
        }
        private static ModPackage getDefenseStats(int rarityLevel, double baseMultiplier)
        {
            ModPackage package = new ModPackage();

            double multiplier = baseMultiplier;
            if (rarityLevel == 3)
                multiplier += 0.2;
            else if (rarityLevel == 4)
                multiplier += 0.45;
            else if (rarityLevel == 5)
                multiplier += 1.0;
            else if (rarityLevel == 6)
                multiplier += 4.0;

            for (int i = 0; i < rarityLevel; i++)
            {
                //1  -  30  -  Health
                //31 -  60  -  P Reduction
                //61 -  75  -  S Reduction
                //76 -  90  -  S Avoidance
                //91 -  100 -  Health Regen

                int result = RNG.Next(1, 101);

                if (result <= 30)
                    package.BonusHealth += (int)(RNG.NextDouble(10, 25) * multiplier).Truncate(2);
                else if (result <= 60)
                    package.PhysicalReduction += (RNG.NextDouble(0.25, 1.5) * multiplier).Truncate(2);
                else if (result <= 75)
                    package.SpellReduction += (RNG.NextDouble(0.2, 1.0) * multiplier).Truncate(2);
                else if (result <= 90)
                    package.SpellAvoidance += (RNG.NextDouble(0.1, 0.5) * multiplier).Truncate(2);
                else if (result <= 100)
                    package.BonusHealth += (int)(RNG.NextDouble(10, 25) * multiplier).Truncate(2); //TODO: Add Health regen
            }
            return package;
        }
        private static ModPackage getMagicalPowerStats(int rarityLevel, double baseMultiplier)
        {
            ModPackage package = new ModPackage();

            double multiplier = baseMultiplier;
            if (rarityLevel == 3)
                multiplier += 0.2;
            else if (rarityLevel == 4)
                multiplier += 0.45;
            else if (rarityLevel == 5)
                multiplier += 1.0;
            else if (rarityLevel == 6)
                multiplier += 4.0;

            for (int i = 0; i < rarityLevel; i++)
            {
                //1  - 50  -  Spell Power
                //51 - 70  -  Crit Power
                //71 - 90  -  Hit Chance
                //91 - 95  -  Crit Chance
                //96 - 100 -  Haste

                int result = RNG.Next(1, 101);

                if (result <= 50)
                    package.SpellPower += (RNG.NextDouble(1, 10) * multiplier).Truncate(2);
                else if (result <= 70)
                    package.SpellCritPower += (RNG.NextDouble(0.1, 0.3) * multiplier).Truncate(2);
                else if (result <= 90)
                    package.SpellHitChance += (RNG.NextDouble(2, 6) * multiplier).Truncate(2);
                else if (result <= 95)
                    package.SpellCritChance += (RNG.NextDouble(1, 3) * multiplier).Truncate(2);
                else if (result <= 100)
                    package.SpellHaste += (RNG.NextDouble(1, 4) * multiplier).Truncate(2);
            }
            return package;
        }
        private static ModPackage getMagicalSpeedStats(int rarityLevel, double baseMultiplier)
        {
            ModPackage package = new ModPackage();

            double multiplier = baseMultiplier;
            if (rarityLevel == 3)
                multiplier += 0.2;
            else if (rarityLevel == 4)
                multiplier += 0.45;
            else if (rarityLevel == 5)
                multiplier += 1.0;
            else if (rarityLevel == 6)
                multiplier += 4.0;

            for (int i = 0; i < rarityLevel; i++)
            {
                //1  -  30  -  Haste
                //31 -  60  -  Crit Chance
                //61 -  75  -  Hit Chance
                //76 -  90  -  Spell Power
                //91 -  100 -  Crit Power

                int result = RNG.Next(1, 101);

                if (result <= 30)
                    package.SpellHaste += (RNG.NextDouble(1, 4) * multiplier).Truncate(2);
                else if (result <= 60)
                    package.PhysicalCritChance += (RNG.NextDouble(1, 3) * multiplier).Truncate(2);
                else if (result <= 75)
                    package.SpellHitChance += (RNG.NextDouble(2, 6) * multiplier).Truncate(2);
                else if (result <= 90)
                    package.SpellPower += (RNG.NextDouble(1, 10) * multiplier).Truncate(2);
                else if (result <= 100)
                    package.SpellCritPower += (RNG.NextDouble(0.1, 0.3) * multiplier).Truncate(2);
            }
            return package;
        }
    }
}
namespace Roguelike.Engine.Factories.Consumables
{
    public static class ConsumableGenerator
    {
        public static Food GenerateFood()
        {
            string name = healthyFoods[RNG.Next(0, healthyFoods.Length)];
            string description = name + " is a nutritional item that will restore some health.";
            return new Food() { Name = name, Value = 1, Weight = 2, OnUseEffect = new BasicFoodHeal(name.Length, false), Description = description };
        }

        public static Potion GeneratePotion()
        {
            Effect[] potionEffects = new Effect[] { new BasicHealthPotion(), new BasicManaPotion(), new BasicPoisonPotion(), new BasicDeathPotion() };

            Potion potion = new Potion(potionEffects[RNG.Next(0, potionEffects.Length)]) { Name = "Unknown Potion", Description = "You don't know what this potion is.  Try it?" };

            return potion;
        }

        public static Scroll GenerateScroll()
        {
            Ability[] scrollAbilities = new Ability[] { new Game.Stats.Classes.Mage.Ability_Fireball(), new Game.Stats.Classes.Warlock.Ability_Havoc(), new AbilityBlink() };
            Scroll scroll = new Scroll(scrollAbilities[RNG.Next(0, scrollAbilities.Length)]);

            return scroll;
        }

        private static string[] healthyFoods = new string[] { "Bread", "Cheese", "Apple", "Lettuce", "Cucumber", "Beef Jerky", "Lutefisk", "Sweet Roll" };
        private static string[] unhealthyFood = new string[] { "Feces" };
    }

    public class BasicFoodHeal : Effect
    {
        int healAmount;
        public BasicFoodHeal(int healAmount, bool scale)
            : base(0)
        {
            this.EffectName = "Yum!";
            this.EffectDescription = "You have recently eaten and are feeling the benefits.";

            this.healAmount = healAmount;
            if (scale) //Scale to user level
                this.healAmount *= 1; //TODO: Scale healing to user level
        }

        public override void UpdateStep()
        {
            if (this.healAmount > 0)
            {
                this.parent.AddHealth(2);
                this.healAmount -= 2;
            }
            else
                this.DoPurge = true;
        }
    }

    public class BasicHealthPotion : Effect
    {
        public BasicHealthPotion()
            : base(1) { }

        public override void OnApplication(Game.Entities.Entity entity)
        {
            entity.StatsPackage.AddHealth(RNG.Next(5, (int)(entity.StatsPackage.MaxHealth / 2)));
            base.OnApplication(entity);
        }
    }
    public class BasicManaPotion : Effect
    {
        public BasicManaPotion()
            : base(1) { }

        public override void OnApplication(Game.Entities.Entity entity)
        {
            entity.StatsPackage.AddMana(RNG.Next(5, (int)(entity.StatsPackage.MaxHealth / 2)));
            base.OnApplication(entity);
        }
    }
    public class BasicPoisonPotion : Effect
    {
        public BasicPoisonPotion()
            : base(1) { }

        public override void OnApplication(Game.Entities.Entity entity)
        {
            MessageCenter.PostMessage("You have been poisoned!", "You have been poisoned by that potion!", entity);

            entity.StatsPackage.ApplyEffect(new Game.Stats.Classes.Rogue.Effect_Poison());
            base.OnApplication(entity);
        }
    }
    public class BasicDeathPotion : Effect
    {
        public BasicDeathPotion()
            : base(1) { }

        public override void OnApplication(Game.Entities.Entity entity)
        {
            entity.StatsPackage.DrainHealth((int)entity.StatsPackage.MaxHealth.EffectiveValue);
            base.OnApplication(entity);
        }
    }

    public class AbilityBlink : Ability
    {
        public AbilityBlink()
            : base(10)
        {
            this.AbilityName = "Blink";
            this.AbilityNameShort = "Blink";

            this.TargetingType = TargetingTypes.GroundTarget;
            this.abilityType = AbilityTypes.Magical;
            this.abilityCost = 10;
            this.cooldown = 10;
            this.range = 5;
        }

        public override CombatResults CalculateResults(StatsPackage caster, StatsPackage target)
        {
            return new CombatResults() { Caster = caster, Target = target, UsedAbility = this };
        }

        public override void CastAbilityGround(StatsPackage caster, int x0, int y0, int radius, Level level)
        {
            if (!GameManager.CurrentLevel.IsTileSolid(x0, y0))
            {
                GameManager.Player.TeleportPlayer(GameManager.CurrentLevel, new Microsoft.Xna.Framework.Point(x0, y0));
            }
        }
    }
}