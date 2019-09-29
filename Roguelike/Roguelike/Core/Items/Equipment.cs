using System;
using OpenTK.Graphics;
using Roguelike.Core.Combat;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Items
{
    public class Equipment : Item
    {
        public Equipment(EquipmentSlots slot)
            : base(ItemTypes.Equipment)
        {
            itemSlot = slot;
        }

        public override string GetDescription()
        {
            return Description + "\n\n" + ModPackage.GetStatInfo();
        }

        public virtual void OnAttack(CombatResults results) { }
        public virtual void OnDefend(CombatResults results) { }
        public virtual void OnMove() { }
        public virtual void OnEquip() { }
        public virtual void OnUnequip() { }
        public virtual bool CanEquip() { return true; }

        private Rarities itemRarity;
        private ModPackage itemModPackage;
        private EquipmentSlots itemSlot;
        private EquipmentSlots equippedSlot;

        public override Color4 TextColor
        {
            get
            {
                switch (itemRarity)
                {
                    case Rarities.Common:
                        return COMMON_ITEM_COLOR;
                    case Rarities.Uncommon:
                        return UNCOMMON_ITEM_COLOR;
                    case Rarities.Rare:
                        return RARE_ITEM_COLOR;
                    case Rarities.Epic:
                        return EPIC_ITEM_COLOR;
                    case Rarities.Legendary:
                        return LEGENDARY_ITEM_COLOR;
                }
                return UNIQUE_ITEM_COLOR;
            }
            set
            {
                base.TextColor = value;
            }
        }
        public override string ListText
        {
            get
            {
                return Name;
            }
            set
            {
                base.ListText = value;
            }
        }
        public Rarities ItemRarity { get { return itemRarity; } set { itemRarity = value; } }
        public ModPackage ModPackage { get { return itemModPackage; } set { itemModPackage = value; } }
        public EquipmentSlots EquipSlot { get { return itemSlot; } set { itemSlot = value; } }
        public EquipmentSlots EquippedSlot { get { return equippedSlot; } set { equippedSlot = value; } }

        private static Color4 COMMON_ITEM_COLOR = Color4.GhostWhite;
        private static Color4 UNCOMMON_ITEM_COLOR = Color4.LimeGreen;
        private static Color4 RARE_ITEM_COLOR = Color4.SteelBlue;
        private static Color4 EPIC_ITEM_COLOR = new Color4(204, 102, 204, 255);
        private static Color4 LEGENDARY_ITEM_COLOR = Color4.OrangeRed;
        private static Color4 UNIQUE_ITEM_COLOR = Color4.Red;
    }

    public enum EquipmentSlots { Head, Neck, Shoulder, Chest, Legs, Boots, Gloves, Ring, Relic, MainHand, OffHand, OneHand, TwoHand }
}
