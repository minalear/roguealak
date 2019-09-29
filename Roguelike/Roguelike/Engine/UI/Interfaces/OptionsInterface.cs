using System;
using Roguelike.Engine.UI;
using Roguelike.Engine.UI.Controls;
using Roguelike.Core;

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
            mainTitle = new Title(this, "Roguealak", GraphicConsole.BufferWidth / 2, 2, Title.TextAlignModes.Center);
            mainTitle.TextColor = Color.Red;

            continueGame = new Button(this, "Continue", GraphicConsole.BufferWidth / 2 - 15, 10, 30, 3) { KeyShortcut = Keys.Escape };
            saveQuitButton = new Button(this, "Save and Quit", GraphicConsole.BufferWidth / 2 - 15, 14, 30, 3);
            quitButton = new Button(this, "Quit", GraphicConsole.BufferWidth / 2 - 15, 18, 30, 3) { KeyShortcut = Keys.Q };

            continueGame.Click += continueGame_Pressed;
            saveQuitButton.Click += saveQuitButton_Pressed;
            quitButton.Click += quitButton_Pressed;
        }

        void continueGame_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.Game);
        }
        void saveQuitButton_Pressed(object sender, MouseButtons button)
        {
            GameManager.ResetGame();
            GameManager.ChangeGameState(GameStates.MainMenu);
        }
        void quitButton_Pressed(object sender, MouseButtons button)
        {
            GameManager.ResetGame();
            Program.Exit();
        }
    }
}
