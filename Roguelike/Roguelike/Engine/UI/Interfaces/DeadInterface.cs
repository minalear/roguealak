using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Engine.UI;
using Roguelike.Engine.UI.Controls;
using Roguelike.Engine.Game;
using Roguelike.Engine.Game.Items;
using Roguelike.Engine.Game.Combat;

namespace Roguelike.Engine.UI.Interfaces
{
    public class DeadInterface : Interface
    {
        Title message;
        Button backButton;

        public DeadInterface()
        {
            this.message = new Title(this, "You have died.", GraphicConsole.BufferWidth / 2, GraphicConsole.BufferHeight / 2, Title.TextAlignModes.Center);

            backButton = new Button(this, "X", GraphicConsole.BufferWidth - 1, 0, 1, 1);
            backButton.Click += backButton_Pressed;
        }

        public override void OnCall()
        {
            Inventory.ClearInventory();
            SpellBook.ClearSpells();
            
            base.OnCall();
        }

        void backButton_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.MainMenu);
        }
    }
}
