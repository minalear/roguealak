using System;
using Roguelike.Engine.UI.Controls;

namespace Roguelike.Core.Items
{
    public class Item : ListItem
    {
        public Item(ItemTypes type)
        {
            this.itemType = type;

            this.TextColor = Color.White;
            this.ListText = this.itemType.ToString();
        }

        public override string ToString()
        {
            return this.ListText;
        }
        public void DrawStep(Rectangle viewport)
        {
            int pointX = this.position.X - GameManager.CameraOffset.X + viewport.X;
            int pointY = this.position.Y - GameManager.CameraOffset.Y + viewport.Y;

            if (pointX >= viewport.Left && pointX < viewport.Right && pointY >= viewport.Top && pointY < viewport.Bottom)
            {
                if (!this.parentLevel.IsOutOfBounds(this.position.X, this.position.Y) && this.parentLevel.Matrix.TerrainMatrix[this.position.X, this.position.Y].IsVisible)
                {
                    GraphicConsole.SetColors(this.foregroundColor, this.backgroundColor);
                    GraphicConsole.Put(this.token, pointX, pointY);
                }
            }
        }
        public virtual string GetDescription()
        {
            if (string.IsNullOrEmpty(this.description))
                return this.Name + " - " + this.ItemType.ToString();

            return this.description;
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
        private Color foregroundColor = Color.White;
        private Color backgroundColor = Color.Black;
        private Level parentLevel;
        private bool removeOnUse = false;

        public string Name { get { return this.name; } set { this.name = value; } }
        public string Description { get { return this.description; } set { this.description = value; } }
        public ItemTypes ItemType { get { return this.itemType; } set { this.itemType = value; } }
        public Point Position { get { return this.position; } set { this.position = value; } }
        public char Token { get { return this.token; } set { this.token = value; } }
        public int Value { get { return this.value; } set { this.value = value; } }
        public Color ForegroundColor { get { return this.foregroundColor; } set { this.foregroundColor = value; } }
        public Color BackgroundColor { get { return this.backgroundColor; } set { this.backgroundColor = value; } }
        public Level ParentLevel { get { return this.parentLevel; } set { this.parentLevel = value; } }
        public override string ListText { get { return this.name; } set { base.ListText = value; } }
        public bool RemoveOnUse { get { return this.removeOnUse; } set { this.removeOnUse = value; } }
        public int Weight { get; set; }
    }

    public enum Rarities { Common, Uncommon, Rare, Epic, Legendary, Unique }
    public enum ItemTypes { Equipment, Scroll, Tome, Potion, Scrap, Junk, Food, Gold }
}
