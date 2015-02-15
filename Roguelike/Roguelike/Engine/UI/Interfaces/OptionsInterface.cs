using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Engine.UI;
using Roguelike.Engine.UI.Controls;
using Roguelike.Engine.Game;

namespace Roguelike.Engine.UI.Interfaces
{
    public class OptionsInterface : Interface
    {
        private Title mainTitle;

        private Button continueGame;
        private Button saveQuitButton;
        private Button quitButton;

        public OptionsInterface()
        {
            this.mainTitle = new Title(this, "Roguealak", GraphicConsole.BufferWidth / 2, 2, Title.TextAlignModes.Center);
            this.mainTitle.TextColor = Color.Red;

            this.continueGame = new Button(this, "Continue", GraphicConsole.BufferWidth / 2 - 15, 10, 30, 3) { KeyShortcut = Keys.Escape };
            this.saveQuitButton = new Button(this, "Save and Quit", GraphicConsole.BufferWidth / 2 - 15, 14, 30, 3);
            this.quitButton = new Button(this, "Quit", GraphicConsole.BufferWidth / 2 - 15, 18, 30, 3) { KeyShortcut = Keys.Q };

            this.continueGame.Click += continueGame_Pressed;
            this.saveQuitButton.Click += saveQuitButton_Pressed;
            this.quitButton.Click += quitButton_Pressed;
        }

        void continueGame_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.Game);
        }
        void saveQuitButton_Pressed(object sender, MouseButtons button)
        {
            Game.Items.Inventory.ClearInventory();
            GameManager.ChangeGameState(GameStates.MainMenu);
        }
        void quitButton_Pressed(object sender, MouseButtons button)
        {
            Game.Items.Inventory.ClearInventory();
            Program.Exit();
        }
    }
}
