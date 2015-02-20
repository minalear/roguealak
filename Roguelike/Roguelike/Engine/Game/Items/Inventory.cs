using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Engine.Game.Stats;
using Roguelike.Engine.Game.Combat;

namespace Roguelike.Engine.Game.Items
{
    public static class Inventory
    {
        private static List<Item> inventory = new List<Item>();
        private static List<Equipment> equippedItems = new List<Equipment>();

        public static List<Item> PlayerInventory { get { return inventory; } set { inventory = value; } }

        #region Variables
        public static Equipment HeadSlot;
        public static Equipment NeckSlot;
        public static Equipment ShoulderSlot;
        public static Equipment ChestSlot;
        public static Equipment LegsSlot;
        public static Equipment BootsSlot;
        public static Equipment GlovesSlot;
        public static Equipment RingSlot;
        public static Equipment RelicSlot;

        public static Equipment MainHand;
        public static Equipment OffHand;
        public static Equipment TwoHand;

        public static int Gold = 0;
        #endregion

        public static void DropItem(int index, Level level, int x, int y)
        {
            inventory[index].ParentLevel = level;
            inventory[index].Position = new Microsoft.Xna.Framework.Point(x, y);

            level.FloorItems.Add(inventory[index]);
            inventory.RemoveAt(index);
        }

        public static bool ItemEquipped(EquipmentSlots slot)
        {
            if (slot == EquipmentSlots.Head)
                return (HeadSlot != null);
            else if (slot == EquipmentSlots.Neck)
                return (NeckSlot != null);
            else if (slot == EquipmentSlots.Shoulder)
                return (ShoulderSlot != null);
            else if (slot == EquipmentSlots.Chest)
                return (ChestSlot != null);
            else if (slot == EquipmentSlots.Legs)
                return (LegsSlot != null);
            else if (slot == EquipmentSlots.Boots)
                return (BootsSlot != null);
            else if (slot == EquipmentSlots.Gloves)
                return (GlovesSlot != null);
            
            else if (slot == EquipmentSlots.Ring)
                return (RingSlot != null);
            else if (slot == EquipmentSlots.Relic)
                return (RelicSlot != null);

            else if (slot == EquipmentSlots.MainHand)
                return (MainHand != null);
            else if (slot == EquipmentSlots.OffHand)
                return (OffHand != null);
            else if (slot == EquipmentSlots.TwoHand)
                return (TwoHand != null);

            return false;
        }
        public static Item GetEquipment(EquipmentSlots slot)
        {
            if (slot == EquipmentSlots.Head)
                return HeadSlot;
            else if (slot == EquipmentSlots.Neck)
                return NeckSlot;
            else if (slot == EquipmentSlots.Shoulder)
                return ShoulderSlot;
            else if (slot == EquipmentSlots.Chest)
                return ChestSlot;
            else if (slot == EquipmentSlots.Legs)
                return LegsSlot;
            else if (slot == EquipmentSlots.Boots)
                return BootsSlot;
            else if (slot == EquipmentSlots.Gloves)
                return GlovesSlot;
            
            else if (slot == EquipmentSlots.Ring)
                return RingSlot;
            else if (slot == EquipmentSlots.Relic)
                return RelicSlot;

            else if (slot == EquipmentSlots.MainHand)
                return MainHand;
            else if (slot == EquipmentSlots.OffHand)
                return OffHand;

            return null;
        }

        public static void EquipItem(Equipment item, EquipmentSlots designatedSlot)
        {
            if (item.CanEquip())
            {
                EquipmentSlots itemSlot = item.EquipSlot;

                //Weapon Pre-Check
                if (itemSlot == EquipmentSlots.OneHand)
                {
                    itemSlot = designatedSlot;
                }
                if (designatedSlot == EquipmentSlots.OneHand)
                {
                    if (!ItemEquipped(EquipmentSlots.MainHand))
                        itemSlot = EquipmentSlots.MainHand;
                    else if (!ItemEquipped(EquipmentSlots.OffHand))
                        itemSlot = EquipmentSlots.OffHand;
                    else
                        itemSlot = EquipmentSlots.MainHand;
                }
                if (itemSlot == EquipmentSlots.TwoHand)
                {
                    designatedSlot = EquipmentSlots.MainHand;
                }

                if (itemSlot == designatedSlot)
                {
                    UnequipItem(designatedSlot);
                    equipItem(item, designatedSlot);
                }
                else
                {
                    //Cannot equip item into designated slot, will pick best slot
                    UnequipItem(itemSlot);
                    equipItem(item, itemSlot);
                }

                inventory.Remove(item);
            }
        }
        public static void UnequipItem(EquipmentSlots slot)
        {
            if (ItemEquipped(slot))
            {
                if (slot == EquipmentSlots.Head)
                    unequipItem(HeadSlot, slot);
                else if (slot == EquipmentSlots.Shoulder)
                    unequipItem(ShoulderSlot, slot);
                else if (slot == EquipmentSlots.Chest)
                    unequipItem(ChestSlot, slot);
                else if (slot == EquipmentSlots.Legs)
                    unequipItem(LegsSlot, slot);
                else if (slot == EquipmentSlots.Boots)
                    unequipItem(BootsSlot, slot);
                else if (slot == EquipmentSlots.Gloves)
                    unequipItem(GlovesSlot, slot);

                else if (slot == EquipmentSlots.Neck)
                    unequipItem(NeckSlot, slot);
                else if (slot == EquipmentSlots.Ring)
                    unequipItem(RingSlot, slot);
                else if (slot == EquipmentSlots.Relic)
                    unequipItem(RelicSlot, slot);

                else if (slot == EquipmentSlots.MainHand)
                    unequipItem(MainHand, slot);
                else if (slot == EquipmentSlots.OffHand)
                    unequipItem(OffHand, slot);
                else if (slot == EquipmentSlots.TwoHand)
                    unequipItem(TwoHand, slot);
            }
        }
        
        public static StatsPackage CalculateStats(StatsPackage stats)
        {
            if (ItemEquipped(EquipmentSlots.MainHand))
                stats = MainHand.ModPackage.ApplyPackage(stats);
            if (ItemEquipped(EquipmentSlots.OffHand))
                stats = OffHand.ModPackage.ApplyPackage(stats);
            if (ItemEquipped(EquipmentSlots.TwoHand))
                stats = TwoHand.ModPackage.ApplyPackage(stats);

            if (ItemEquipped(EquipmentSlots.Head))
                stats = HeadSlot.ModPackage.ApplyPackage(stats);
            if (ItemEquipped(EquipmentSlots.Neck))
                stats = NeckSlot.ModPackage.ApplyPackage(stats);
            if (ItemEquipped(EquipmentSlots.Shoulder))
                stats = ShoulderSlot.ModPackage.ApplyPackage(stats);
            if (ItemEquipped(EquipmentSlots.Chest))
                stats = ChestSlot.ModPackage.ApplyPackage(stats);
            if (ItemEquipped(EquipmentSlots.Legs))
                stats = LegsSlot.ModPackage.ApplyPackage(stats);
            if (ItemEquipped(EquipmentSlots.Boots))
                stats = BootsSlot.ModPackage.ApplyPackage(stats);
            if (ItemEquipped(EquipmentSlots.Gloves))
                stats = GlovesSlot.ModPackage.ApplyPackage(stats);
            if (ItemEquipped(EquipmentSlots.Ring))
                stats = RingSlot.ModPackage.ApplyPackage(stats);
            if (ItemEquipped(EquipmentSlots.Relic))
                stats = RelicSlot.ModPackage.ApplyPackage(stats);

            return stats;
        }
        public static void ClearInventory()
        {
            inventory.Clear();

            setToNull(EquipmentSlots.Head);
            setToNull(EquipmentSlots.Neck);
            setToNull(EquipmentSlots.Shoulder);
            setToNull(EquipmentSlots.Chest);
            setToNull(EquipmentSlots.Legs);
            setToNull(EquipmentSlots.Boots);
            setToNull(EquipmentSlots.Gloves);
            setToNull(EquipmentSlots.Ring);
            setToNull(EquipmentSlots.Relic);

            setToNull(EquipmentSlots.MainHand);
            setToNull(EquipmentSlots.OffHand);
            setToNull(EquipmentSlots.TwoHand);
            setToNull(EquipmentSlots.OneHand);

            Gold = 0;
        }

        public static void OnAttack(CombatResults results)
        {
            for (int i = 0; i < equippedItems.Count; i++)
                equippedItems[i].OnAttack(results);
        }
        public static void OnDefend(CombatResults results)
        {
            for (int i = 0; i < equippedItems.Count; i++)
                equippedItems[i].OnDefend(results);
        }
        public static void OnMove()
        {
            for (int i = 0; i < equippedItems.Count; i++)
                equippedItems[i].OnMove();
        }

        public static void PurgeItem(Item item)
        {
            inventory.Remove(item);
        }

        private static void setToNull(EquipmentSlots slot)
        {
            if (slot == EquipmentSlots.Head)
                HeadSlot = null;
            else if (slot == EquipmentSlots.Neck)
                NeckSlot = null;
            else if (slot == EquipmentSlots.Shoulder)
                ShoulderSlot = null;
            else if (slot == EquipmentSlots.Chest)
                ChestSlot = null;
            else if (slot == EquipmentSlots.Legs)
                LegsSlot = null;
            else if (slot == EquipmentSlots.Boots)
                BootsSlot = null;
            else if (slot == EquipmentSlots.Gloves)
                GlovesSlot = null;
            
            else if (slot == EquipmentSlots.Ring)
                RingSlot = null;
            else if (slot == EquipmentSlots.Relic)
                RelicSlot = null;

            else if (slot == EquipmentSlots.MainHand)
                MainHand = null;
            else if (slot == EquipmentSlots.OffHand)
                OffHand = null;

            else if (slot == EquipmentSlots.TwoHand)
                TwoHand = null;
        }
        private static void equipItem(Equipment item, EquipmentSlots slot)
        {
            if (slot == EquipmentSlots.Head)
                HeadSlot = item;
            else if (slot == EquipmentSlots.Neck)
                NeckSlot = item;
            else if (slot == EquipmentSlots.Shoulder)
                ShoulderSlot = item;
            else if (slot == EquipmentSlots.Chest)
                ChestSlot = item;
            else if (slot == EquipmentSlots.Legs)
                LegsSlot = item;
            else if (slot == EquipmentSlots.Boots)
                BootsSlot = item;
            else if (slot == EquipmentSlots.Gloves)
                GlovesSlot = item;
            
            else if (slot == EquipmentSlots.Ring)
                RingSlot = item;
            else if (slot == EquipmentSlots.Relic)
                RelicSlot = item;

            else if (slot == EquipmentSlots.MainHand)
            {
                MainHand = item;
                UnequipItem(EquipmentSlots.TwoHand);
            }
            else if (slot == EquipmentSlots.OffHand)
            {
                OffHand = item;
                UnequipItem(EquipmentSlots.TwoHand);
            }
            else if (slot == EquipmentSlots.TwoHand)
            {
                TwoHand = item;
                UnequipItem(EquipmentSlots.MainHand);
                UnequipItem(EquipmentSlots.OffHand);
            }

            item.EquippedSlot = slot;
            
            item.OnEquip();
            equippedItems.Add(item);
        }
        private static void unequipItem(Equipment item, EquipmentSlots slot)
        {
            equippedItems.Remove(item);
            setToNull(slot);

            inventory.Add(item);
        }
    }
}
