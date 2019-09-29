using System;
using OpenTK.Input;
using OpenTK.Graphics;
using Roguelike.Engine.Console;
using Roguelike.Engine.UI.Controls;

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
            mainTitle = new Title(this, "Roguealak", GraphicConsole.Instance.BufferWidth / 2, 2, Title.TextAlignModes.Center);
            mainTitle.TextColor = Color4.Red;

            continueGame = new Button(this, "Continue", GraphicConsole.Instance.BufferWidth / 2 - 15, 10, 30, 3) { KeyShortcut = Key.Escape };
            saveQuitButton = new Button(this, "Save and Quit", GraphicConsole.Instance.BufferWidth / 2 - 15, 14, 30, 3);
            quitButton = new Button(this, "Quit", GraphicConsole.Instance.BufferWidth / 2 - 15, 18, 30, 3) { KeyShortcut = Key.Q };

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
