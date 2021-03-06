﻿using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using Roguelike.Engine.Console;

namespace Roguelike.Engine.UI.Controls
{
    public class ScrollingList : Control
    {
        public ScrollingList(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            position = new Point(x, y);
            size = new Point(width, height);

            objectList = new List<ListItem>();
        }

        public override void DrawStep()
        {
            GraphicConsole.Instance.SetColors(Color4.Transparent, fillColor);
            DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

            if (!scroll)
            {
                for (int i = 0; i < objectList.Count; i++)
                {
                    setConsoleColors(i);
                    GraphicConsole.Instance.SetCursor(Position.X, Position.Y + i);

                    writeLine(objectList[i].ListText);
                }
            }
            else
            {
                //Scroll Bar Rail
                GraphicConsole.Instance.SetColors(scrollRailColor, fillColor);
                DrawingUtilities.DrawLine(Position.X + Size.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y - 1, scrollRail);

                //Scroll Barl
                GraphicConsole.Instance.SetColors(scrollBarColor, fillColor);
                GraphicConsole.Instance.SetCursor(Position.X + Size.X, (int)(scrollValue / 100f * Size.Y) + Position.Y);
                GraphicConsole.Instance.Write(scrollBar);

                int line = (int)(scrollValue / 100f * (objectList.Count - Size.Y + 1));
                if (line < 0)
                    line = 0;

                for (int y = 0; y < Size.Y; y++)
                {
                    if (line < objectList.Count)
                    {
                        setConsoleColors(line);
                        GraphicConsole.Instance.SetCursor(Position.X, Position.Y + y);
                        writeLine(objectList[line].ListText);
                    }
                    line++;
                }
            }

            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (isMouseHover())
            {
                #region Scrolling
                if (scroll)
                {
                    int differenceValue = InputManager.GetDistanceScrolled();

                    if (differenceValue != 0)
                    {
                        scrollValue -= differenceValue / (objectList.Count / 2);

                        if (scrollValue < 0f)
                            scrollValue = 0f;
                        else if (scrollValue >= 100f)
                            scrollValue = 99f;

                        InterfaceManager.DrawStep();
                    }
                }
                #endregion
                #region Selection/Hovering
                if (InputManager.MouseButtonWasClicked(MouseButtons.Left))
                {
                    int index = getIndexOfClick(GraphicConsole.Instance.GetTilePosition(InputManager.GetCurrentMousePosition()));
                    if (index >= 0 && index < objectList.Count)
                    {
                        selectedIndex = index;
                        InterfaceManager.DrawStep();

                        onSelect();
                    }
                    else
                    {
                        selectedIndex = -1;

                        if (Deselected != null)
                            Deselected(this);

                        InterfaceManager.DrawStep();
                    }
                }
                else //Get Hover
                {
                    int index = getIndexOfClick(GraphicConsole.Instance.GetTilePosition(InputManager.GetCurrentMousePosition()));
                    if (index >= 0 && index < objectList.Count)
                    {
                        hoverIndex = index;
                        InterfaceManager.DrawStep();

                        onHover();
                    }
                    else
                    {
                        hoverIndex = -1;
                        InterfaceManager.DrawStep();
                    }
                }
                #endregion
            }
            else if (wasHover())
            {
                hoverIndex = -1;
                InterfaceManager.DrawStep();
            }

            base.Update(gameTime);
        }

        public void SetList<T>(List<T> newList) where T:ListItem
        {
            objectList.Clear();

            for (int i = 0; i < newList.Count; i++)
                objectList.Add(newList[i]);
            setupList();

            if (selectedIndex >= Items.Count)
                ClearSelection();
        }
        public void ClearSelection()
        {
            selectedIndex = -1;
            InterfaceManager.DrawStep();
        }
        public void SetSelection(int index)
        {
            if (index >= 0 && index < objectList.Count)
            {
                selectedIndex = index;
                onSelect();
            }
            else
                ClearSelection();

            InterfaceManager.DrawStep();
        }
        public void SetSelection(string item)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList[i] == item)
                {
                    selectedIndex = i;
                    InterfaceManager.DrawStep();

                    onSelect();

                    break;
                }
            }

            selectedIndex = -1;
            InterfaceManager.DrawStep();
        }
        public ListItem GetSelection()
        {
            return objectList[selectedIndex];
        }
        public void RemoveItem(string item)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].ListText == item)
                {
                    Items.RemoveAt(i);
                    break;
                }
            }
            ClearSelection();
        }
        public void RemoveItem(int index)
        {
            if (index < Items.Count)
            {
                Items.RemoveAt(index);

                if (selectedIndex >= Items.Count)
                    ClearSelection();
            }
        }

        protected void onHover()
        {
            Hover?.Invoke(this, hoverIndex);
        }
        protected void onSelect()
        {
            Selected?.Invoke(this, selectedIndex);
        }

        private void setupList()
        {
            if (objectList.Count > Size.Y)
                scroll = true;
            else
                scroll = false;

            scrollValue = 0f;
        }
        private int getIndexOfClick(Point point)
        {
            int index = -1;
            if (scroll)
            {
                int line = (int)(scrollValue / 100f * (objectList.Count - Size.Y + 1));

                index = point.Y - Position.Y;
                index += line;
            }
            else
            {
                index = point.Y - Position.Y;
            }

            return index;
        }
        private void writeLine(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                //Ensure text doesn't go past the size of the list
                int i = 0;
                for (i = 0; i < line.Length && i < Size.X; i++)
                    GraphicConsole.Instance.Write(line[i]);
                for (; i < Size.X; i++)
                    GraphicConsole.Instance.Write(' ');
            }
            else
            {
                for (int i = 0; i < Size.X; i++)
                    GraphicConsole.Instance.Write(' ');
            }
        }
        private void setConsoleColors(int index)
        {
            if (index == selectedIndex)
                GraphicConsole.Instance.SetColors(selectedTextColor, selectedFillColor);
            else if (index == hoverIndex)
                GraphicConsole.Instance.SetColors(hoverTextColor, hoverFillColor);
            else
                GraphicConsole.Instance.SetColors(objectList[index].TextColor, fillColor);
        }

        private List<ListItem> objectList;
        private Color4 textColor = Color4.White;
        private Color4 fillColor = Color4.Black;
        private Color4 selectedTextColor = Color4.Black;
        private Color4 selectedFillColor = Color4.White;
        private Color4 hoverTextColor = Color4.White;
        private Color4 hoverFillColor = new Color4(170, 181, 187, 255);
        private Color4 scrollRailColor = Color4.Gray;
        private Color4 scrollBarColor = Color4.LightGray;

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
        public List<ListItem> Items { get { return objectList; } set { SetList(value); } }
        public Color4 TextColor { get { return textColor; } set { textColor = value; } }
        public Color4 FillColor { get { return fillColor; } set { fillColor = value; } }
        public Color4 SelectedTextColor { get { return selectedTextColor; } set { selectedTextColor = value; } }
        public Color4 SelectedFillColor { get { return selectedFillColor; } set { selectedFillColor = value; } }
        public Color4 HoverTextColor { get { return hoverTextColor; } set { hoverTextColor = value; } }
        public Color4 HoverFillColor { get { return hoverFillColor; } set { hoverFillColor = value; } }
        public Color4 ScrollRailColor { get { return scrollRailColor; } set { scrollRailColor = value; } }
        public Color4 ScrollBarColor { get { return scrollBarColor; } set { scrollBarColor = value; } }
        public bool HasSelection { get { return (selectedIndex != -1); } }
        public int SelectedIndex { get { return selectedIndex; } set { SetSelection(value); } }
        #endregion
    }
    public class ListItem
    {
        public virtual Color4 TextColor { get; set; }
        public virtual string ListText { get; set; }

        public ListItem()
        {
            ListText = " ";
            TextColor = Color4.White;
        }

        public static implicit operator string(ListItem item)
        {
            return item.ListText;
        }
        public static implicit operator ListItem(string item)
        {
            return new ListItem() { ListText = item, TextColor = Color4.White };
        }
    }
}
