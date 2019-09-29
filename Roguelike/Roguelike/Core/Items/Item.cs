using System;
using OpenTK;
using OpenTK.Graphics;
using Roguelike.Engine;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Core.Items
{
    public class Item : ListItem
    {
        public Item(ItemTypes type)
        {
            itemType = type;

            TextColor = Color4.White;
            ListText = itemType.ToString();
        }

        public override string ToString()
        {
            return ListText;
        }
        public void DrawStep(Rectangle viewport)
        {
            int pointX = position.X - GameManager.CameraOffset.X + viewport.X;
            int pointY = position.Y - GameManager.CameraOffset.Y + viewport.Y;

            if (pointX >= viewport.Left && pointX < viewport.Right && pointY >= viewport.Top && pointY < viewport.Bottom)
            {
                if (!parentLevel.IsOutOfBounds(position.X, position.Y) && parentLevel.Matrix.TerrainMatrix[position.X, position.Y].IsVisible)
                {
                    GraphicConsole.SetColors(foregroundColor, backgroundColor);
                    GraphicConsole.Put(token, pointX, pointY);
                }
            }
        }
        public virtual string GetDescription()
        {
            if (string.IsNullOrEmpty(description))
                return Name + " - " + ItemType.ToString();

            return description;
        }

        public virtual void OnUse(Entities.Entity entity) { }
        public virtual void OnPickup() { }
        public virtual void OnDrop() { }

        private string name = "[ITEM]";
        private string description;
        private ItemTypes itemType = ItemTypes.Junk;
        private Point position = new Point(0, 0);
        private char token = '•';
        private int value = 1;
        private Color4 foregroundColor = Color4.White;
        private Color4 backgroundColor = Color4.Black;
        private Level parentLevel;
        private bool removeOnUse = false;

        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public ItemTypes ItemType { get { return itemType; } set { itemType = value; } }
        public Point Position { get { return position; } set { position = value; } }
        public char Token { get { return token; } set { token = value; } }
        public int Value { get { return value; } set { this.value = value; } }
        public Color4 ForegroundColor { get { return foregroundColor; } set { foregroundColor = value; } }
        public Color4 BackgroundColor { get { return backgroundColor; } set { backgroundColor = value; } }
        public Level ParentLevel { get { return parentLevel; } set { parentLevel = value; } }
        public override string ListText { get { return name; } set { base.ListText = value; } }
        public bool RemoveOnUse { get { return removeOnUse; } set { removeOnUse = value; } }
        public int Weight { get; set; }
    }

    public enum Rarities { Common, Uncommon, Rare, Epic, Legendary, Unique }
    public enum ItemTypes { Equipment, Scroll, Tome, Potion, Scrap, Junk, Food, Gold }
}
