using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Engine.UI.Controls
{
    public class ScrollingList : Control
    {
        public ScrollingList(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            this.position = new Point(x, y);
            this.size = new Point(width, height);

            this.objectList = new List<ListItem>();
        }

        public override void DrawStep()
        {
            GraphicConsole.SetColors(Color.Transparent, this.fillColor);
            DrawingUtilities.DrawRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

            if (!scroll)
            {
                for (int i = 0; i < this.objectList.Count; i++)
                {
                    this.setConsoleColors(i);
                    GraphicConsole.SetCursor(this.Position.X, this.Position.Y + i);

                    this.writeLine(this.objectList[i].ListText);
                }
            }
            else
            {
                //Scroll Bar Rail
                GraphicConsole.SetColors(this.scrollRailColor, this.fillColor);
                DrawingUtilities.DrawLine(this.Position.X + this.Size.X, this.Position.Y, this.Position.X + this.Size.X, this.Position.Y + this.Size.Y - 1, this.scrollRail);

                //Scroll Barl
                GraphicConsole.SetColors(this.scrollBarColor, this.fillColor);
                GraphicConsole.SetCursor(this.Position.X + this.Size.X, (int)(this.scrollValue / 100f * this.Size.Y) + this.Position.Y);
                GraphicConsole.Write(this.scrollBar);

                int line = (int)(this.scrollValue / 100f * (this.objectList.Count - this.Size.Y + 1));
                if (line < 0)
                    line = 0;

                for (int y = 0; y < this.Size.Y; y++)
                {
                    if (line < objectList.Count)
                    {
                        this.setConsoleColors(line);
                        GraphicConsole.SetCursor(this.Position.X, this.Position.Y + y);
                        this.writeLine(this.objectList[line].ListText);
                    }
                    line++;
                }
            }

            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (this.isMouseHover())
            {
                #region Scrolling
                if (this.scroll)
                {
                    int differenceValue = InputManager.GetDistanceScrolled();

                    if (differenceValue != 0)
                    {
                        this.scrollValue -= differenceValue / (this.objectList.Count / 2);

                        if (this.scrollValue < 0f)
                            this.scrollValue = 0f;
                        else if (this.scrollValue >= 100f)
                            this.scrollValue = 99f;

                        InterfaceManager.DrawStep();
                    }
                }
                #endregion
                #region Selection/Hovering
                if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                {
                    int index = getIndexOfClick(GraphicConsole.GetTilePosition(InputManager.GetCurrentMousePosition()));
                    if (index >= 0 && index < this.objectList.Count)
                    {
                        this.selectedIndex = index;
                        InterfaceManager.DrawStep();

                        this.onSelect();
                    }
                    else
                    {
                        this.selectedIndex = -1;

                        if (this.Deselected != null)
                            this.Deselected(this);

                        InterfaceManager.DrawStep();
                    }
                }
                else //Get Hover
                {
                    int index = getIndexOfClick(GraphicConsole.GetTilePosition(InputManager.GetCurrentMousePosition()));
                    if (index >= 0 && index < this.objectList.Count)
                    {
                        this.hoverIndex = index;
                        InterfaceManager.DrawStep();

                        this.onHover();
                    }
                    else
                    {
                        this.hoverIndex = -1;
                        InterfaceManager.DrawStep();
                    }
                }
                #endregion
            }
            else if (this.wasHover())
            {
                this.hoverIndex = -1;
                InterfaceManager.DrawStep();
            }

            base.Update(gameTime);
        }

        public void SetList<T>(List<T> newList) where T:ListItem
        {
            this.objectList.Clear();

            for (int i = 0; i < newList.Count; i++)
                this.objectList.Add(newList[i]);
            this.setupList();

            if (this.selectedIndex >= this.Items.Count)
                this.ClearSelection();
        }
        public void ClearSelection()
        {
            this.selectedIndex = -1;
            InterfaceManager.DrawStep();
        }
        public void SetSelection(int index)
        {
            if (index >= 0 && index < this.objectList.Count)
            {
                this.selectedIndex = index;
                this.onSelect();
            }
            else
                this.ClearSelection();

            InterfaceManager.DrawStep();
        }
        public void SetSelection(string item)
        {
            for (int i = 0; i < this.objectList.Count; i++)
            {
                if (this.objectList[i] == item)
                {
                    this.selectedIndex = i;
                    InterfaceManager.DrawStep();

                    this.onSelect();

                    break;
                }
            }

            this.selectedIndex = -1;
            InterfaceManager.DrawStep();
        }
        public ListItem GetSelection()
        {
            return this.objectList[this.selectedIndex];
        }
        public void RemoveItem(string item)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].ListText == item)
                {
                    this.Items.RemoveAt(i);
                    break;
                }
            }
            this.ClearSelection();
        }
        public void RemoveItem(int index)
        {
            if (index < this.Items.Count)
            {
                this.Items.RemoveAt(index);

                if (this.selectedIndex >= this.Items.Count)
                    this.ClearSelection();
            }
        }

        protected void onHover()
        {
            if (this.Hover != null)
                this.Hover(this, this.hoverIndex);
        }
        protected void onSelect()
        {
            if (this.Selected != null)
                this.Selected(this, this.selectedIndex);
        }

        private void setupList()
        {
            if (this.objectList.Count > this.Size.Y)
                scroll = true;
            else
                scroll = false;

            this.scrollValue = 0f;
        }
        private int getIndexOfClick(Point point)
        {
            int index = -1;
            if (this.scroll)
            {
                int line = (int)(this.scrollValue / 100f * (this.objectList.Count - this.Size.Y + 1));

                index = point.Y - this.Position.Y;
                index += line;
            }
            else
            {
                index = point.Y - this.Position.Y;
            }

            return index;
        }
        private void writeLine(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                //Ensure text doesn't go past the size of the list
                int i = 0;
                for (i = 0; i < line.Length && i < this.Size.X; i++)
                    GraphicConsole.Write(line[i]);
                for (; i < this.Size.X; i++)
                    GraphicConsole.Write(' ');
            }
            else
            {
                for (int i = 0; i < this.Size.X; i++)
                    GraphicConsole.Write(' ');
            }
        }
        private void setConsoleColors(int index)
        {
            if (index == this.selectedIndex)
                GraphicConsole.SetColors(this.selectedTextColor, this.selectedFillColor);
            else if (index == this.hoverIndex)
                GraphicConsole.SetColors(this.hoverTextColor, this.hoverFillColor);
            else
                GraphicConsole.SetColors(this.objectList[index].TextColor, this.fillColor);
        }

        private List<ListItem> objectList;
        private Color textColor = Color.White;
        private Color fillColor = Color.Black;
        private Color selectedTextColor = Color.Black;
        private Color selectedFillColor = Color.White;
        private Color hoverTextColor = Color.White;
        private Color hoverFillColor = new Color(170, 181, 187);
        private Color scrollRailColor = Color.Gray;
        private Color scrollBarColor = Color.LightGray;

        private bool scroll = false;
        private int selectedIndex = -1;
        private int hoverIndex = -1;
        private char scrollRail = '║';
        private char scrollBar = '▓';

        private float scrollValue = 0f;

        public event ItemHovered Hover;
        public event ItemSelected Selected;
        public event ItemDeselected Deselected;
        public delegate void ItemHovered(object sender, int index);
        public delegate void ItemSelected(object sender, int index);
        public delegate void ItemDeselected(object sender);

        #region Properties
        public List<ListItem> Items { get { return this.objectList; } set { this.SetList(value); } }
        public Color TextColor { get { return this.textColor; } set { this.textColor = value; } }
        public Color FillColor { get { return this.fillColor; } set { this.fillColor = value; } }
        public Color SelectedTextColor { get { return this.selectedTextColor; } set { this.selectedTextColor = value; } }
        public Color SelectedFillColor { get { return this.selectedFillColor; } set { this.selectedFillColor = value; } }
        public Color HoverTextColor { get { return this.hoverTextColor; } set { this.hoverTextColor = value; } }
        public Color HoverFillColor { get { return this.hoverFillColor; } set { this.hoverFillColor = value; } }
        public Color ScrollRailColor { get { return this.scrollRailColor; } set { this.scrollRailColor = value; } }
        public Color ScrollBarColor { get { return this.scrollBarColor; } set { this.scrollBarColor = value; } }
        public bool HasSelection { get { return (this.selectedIndex != -1); } }
        public int SelectedIndex { get { return this.selectedIndex; } set { this.SetSelection(value); } }
        #endregion
    }
    public class ListItem
    {
        public virtual Color TextColor { get; set; }
        public virtual string ListText { get; set; }

        public ListItem()
        {
            this.ListText = " ";
            this.TextColor = Color.White;
        }

        public static implicit operator string(ListItem item)
        {
            return item.ListText;
        }
        public static implicit operator ListItem(string item)
        {
            return new ListItem() { ListText = item, TextColor = Color.White };
        }
    }
}
