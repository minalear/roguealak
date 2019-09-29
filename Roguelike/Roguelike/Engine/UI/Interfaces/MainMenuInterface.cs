using System;
using Roguelike.Engine.UI;
using Roguelike.Engine.UI.Controls;
using Roguelike.Core;

namespace Roguelike.Engine.UI.Interfaces
{
    public class MainMenuInterface : Interface
    {
        private Title mainTitle;
        private Title subTitle;

        private Button startNewGame;
        private Button resumeGame;
        private Button optionsButton;
        private Button aboutButton;
        private Button exitButton;

        public MainMenuInterface()
            : base()
        {
            mainTitle = new Title(this, "Roguealak", GraphicConsole.BufferWidth / 2, 2, Title.TextAlignModes.Center);
            subTitle = new Title(this, generateSubTitle(), GraphicConsole.BufferWidth / 2, 3, Title.TextAlignModes.Center);

            mainTitle.TextColor = Color4.Red;
            subTitle.TextColor = Color4.LightGray;

            startNewGame = new Button(this, "New Game", GraphicConsole.BufferWidth / 2 - 15, 10, 30, 3) { KeyShortcut = Keys.N };
            resumeGame = new Button(this, "Item Testing", GraphicConsole.BufferWidth / 2 - 15, 14, 30, 3) { KeyShortcut = Keys.L };
            optionsButton = new Button(this, "Quick Start", GraphicConsole.BufferWidth / 2 - 15, 18, 30, 3) { KeyShortcut = Keys.O };
            aboutButton = new Button(this, "About", GraphicConsole.BufferWidth / 2 - 15, 22, 30, 3) { KeyShortcut = Keys.A };
            exitButton = new Button(this, "Abandon", GraphicConsole.BufferWidth / 2 - 15, 26, 30, 3) { KeyShortcut = Keys.Escape };

            startNewGame.Click += startNewGame_Pressed;
            resumeGame.Click += resumeGame_Pressed;
            optionsButton.Click += optionsButton_Pressed;
            aboutButton.Click += aboutButton_Pressed;
            exitButton.Click += exitButton_Pressed;
        }

        #region Button Events
        void startNewGame_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.CharacterCreation);
        }

        void resumeGame_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.ItemTesting);
        }

        void optionsButton_Pressed(object sender, MouseButtons button)
        {
            Core.Stats.PlayerStats stats = new Core.Stats.PlayerStats(null);

            stats.Name = "Quickstart McGee";
            stats.Class = "Fagmo";
            stats.Culture = "Regular-ass";
            stats.Race = "Human";
            stats.Township = "Downtown McDowntown";
            stats.Title = "Wolfkin";

            stats.Strength = 10;
            stats.Agility = 10;
            stats.Dexterity = 10;

            stats.Intelligence = 10;
            stats.Willpower = 10;
            stats.Wisdom = 10;

            stats.Constitution = 10;
            stats.Endurance = 10;
            stats.Fortitude = 10;

            GameManager.SetupGame(stats);
            GameManager.ChangeGameState(GameStates.Game);

            GameManager.Player.X = 30;
            GameManager.Player.Y = 30;
            GameManager.SetCameraOffset();

            GameManager.Step();
        }

        void aboutButton_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.About);
        }

        void exitButton_Pressed(object sender, MouseButtons button)
        {
            Program.Exit();
        }
        #endregion

        public override void DrawStep()
        {
            base.DrawStep();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void UpdateStep()
        {
            base.UpdateStep();
        }

        public override void OnCall()
        {
            base.OnCall();
        }

        private string generateSubTitle()
        {
            string[] subtitles = new string[]
            {
                "Illusions of Despair",
                "Visions of Conquest",
                "Tithes to Armrok",
                "The Bloodbath of Westmarch",
                "Defense of the Ancients",
                "Defender of the Throne",
                "Kingslayers of Destiny",
                "Divine Brothers of the Brotherhood",
                "Mexican Pony Superstar",
                "Dangerous Casino Reloaded",
                "Holy Yak of Magic",
                "The Six Million Dollar Jazz Symphony",
                "8-Bit Barcode Dance Party",
                "In Search of the Alien Deathmatch",
                "Primal Toon Ultra",
                "Kabuki Circus Unleashed",
                "Guitar Goblin Battle",
                "Samba de Speed Hunter",
                "The Muppets Fashion Zone",
                "NBA Beautician in the Magic Kingdom",
                "Pinball Simulator",
                "Unholy Love in the Dark",
                "Scooby Doo and the Spelunking Cop",
                "True Crime: Golf Express",
                "Interactive Hitman Express",
                "Happy Handgun Police",
                "Final Fantasy Helicopter in the Salad Kingdom",
                "Amphibious Croquet Zombies",
                "Endless Shadow Summit",
                "Masters of the Harpoon of Mystery",
                "Undercover City Revenge",
                "Cool Jetpack xXx",
                "BudgetSoft Presents: Kung-fu Colosseum",
                "Kermit's Karate X-treme",
                "Erotic Office Kids",
                "Attack of the Kart Revisited",
                "A Boy and His Raccoon",
                "Battle Car Terror"
            };

            return subtitles[RNG.Next(0, subtitles.Length)];
        }
    }
}
