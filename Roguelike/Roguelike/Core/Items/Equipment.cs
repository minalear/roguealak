using System;
using Roguelike.Core.Combat;
using Roguelike.Core.Stats;

namespace Roguelike.Core.Items
{
    public class Equipment : Item
    {
        public Equipment(EquipmentSlots slot)
            : base(ItemTypes.Equipment)
        {
            this.itemSlot = slot;
        }

        public override string GetDescription()
        {
            return this.Description + "\n\n" + this.ModPackage.GetStatInfo();
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

        public override Color TextColor
        {
            get
            {
                switch (this.itemRarity)
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
                return this.Name;
            }
            set
            {
                base.ListText = value;
            }
        }
        public Rarities ItemRarity { get { return this.itemRarity; } set { this.itemRarity = value; } }
        public ModPackage ModPackage { get { return this.itemModPackage; } set { this.itemModPackage = value; } }
        public EquipmentSlots EquipSlot { get { return this.itemSlot; } set { this.itemSlot = value; } }
        public EquipmentSlots EquippedSlot { get { return this.equippedSlot; } set { this.equippedSlot = value; } }

        private static Color COMMON_ITEM_COLOR = Color.GhostWhite;
        private static Color UNCOMMON_ITEM_COLOR = Color.LimeGreen;
        private static Color RARE_ITEM_COLOR = Color.SteelBlue;
        private static Color EPIC_ITEM_COLOR = new Color(204, 102, 204);
        private static Color LEGENDARY_ITEM_COLOR = Color.OrangeRed;
        private static Color UNIQUE_ITEM_COLOR = Color.Red;
    }

    public enum EquipmentSlots { Head, Neck, Shoulder, Chest, Legs, Boots, Gloves, Ring, Relic, MainHand, OffHand, OneHand, TwoHand }
}
