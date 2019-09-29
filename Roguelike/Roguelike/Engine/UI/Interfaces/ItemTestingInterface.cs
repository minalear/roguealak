using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using Roguelike.Engine.UI.Controls;
using Roguelike.Core;
using Roguelike.Core.Items;

namespace Roguelike.Engine.UI.Interfaces
{
    public class ItemTestingInterface : Interface
    {
        Button generateButton;
        ScrollingList itemGenList;
        TextBox infoBox;

        List<Item> items = new List<Item>();

        public ItemTestingInterface()
            : base()
        {
            Title interfaceTitle = new Title(this, "Item Testing", GraphicConsole.BufferWidth / 2, 0, Title.TextAlignModes.Center);
            generateButton = new Button(this, "Generate", 72, 1);
            generateButton.Click += generateButton_Click;
            infoBox = new TextBox(this, 72, 5, 40, 37);
            infoBox.FillColor = new Color4(25, 25, 25, 255);

            itemGenList = new ScrollingList(this, 1, 2, 70, 40);
            itemGenList.FillColor = new Color4(25, 25, 25, 255);
            itemGenList.Selected += itemGenList_Selected;
        }

        void generateButton_Click(object sender, MouseButtons button)
        {
            itemGenList.Items.Clear();
            items.Clear();

            if (button == MouseButtons.Left)
            {
                for (int i = 0; i < 20; i++)
                {
                    Item weapon = Factories.ItemGenerator.GenerateRandomWeapon();
                    itemGenList.Items.Add(weapon);
                    items.Add(weapon);
                }
            }
            else if (button == MouseButtons.Right)
            {
                for (int i = 0; i < 20; i++)
                {
                    Item weapon = Factories.ItemGenerator.GenerateRandomWeapon(Rarities.Legendary);
                    itemGenList.Items.Add(weapon);
                    items.Add(weapon);
                }
            }
        }
        void itemGenList_Selected(object sender, int index)
        {
            infoBox.Text = items[index].GetDescription();
        }
    }
}
