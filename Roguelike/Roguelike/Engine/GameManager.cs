using System;
using Microsoft.Xna.Framework;
using Roguelike.Engine.UI;
using Roguelike.Engine.Game;
using Roguelike.Engine.Game.Entities;
using Roguelike.Engine.Game.Stats;
using Roguelike.Engine.Game.Combat;
using Roguelike.Engine.Game.Items;

namespace Roguelike.Engine
{
    public static class GameManager
    {
        public static GameStates CurrentGameState;
        public static Point CameraOffset;

        public static Dungeon TestDungeon;
        public static Player TestPlayer;

        public static Random RNG;

        private static Level currentLevel;

        public static Player Player { get { return TestPlayer; } set { TestPlayer = value; } }
        public static Level CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }
        public static Dungeon CurrentDungeon { get { return TestDungeon; } set { TestDungeon = value; } }

        public static Rectangle Viewport = new Rectangle(1, 3, 123, 47);
        public static void Initialize()
        {
            RNG = new Random();
            CameraOffset = Point.Zero;

            ChangeGameState(GameStates.MainMenu);
        }

        public static void Update(GameTime gameTime)
        {
            if (CurrentGameState == GameStates.Game)
                GameManager.CurrentLevel.Update(gameTime);

            InterfaceManager.Update(gameTime);
        }

        public static void UpdateStep()
        {
            if (CurrentGameState == GameStates.Game)
            {
                GameManager.CurrentLevel.UpdateStep();
                Pathing.PathCalculator.UpdateStep();
            }

            InterfaceManager.UpdateStep();
        }

        public static void DrawStep()
        {
            GraphicConsole.Clear();

            if (CurrentGameState == GameStates.Game)
                DrawGameWorld();

            InterfaceManager.DrawStep();
        }

        public static void ChangeGameState(GameStates gameState)
        {
            CurrentGameState = gameState;
            InterfaceManager.SwitchInterface(gameState);
        }

        public static void DrawGameWorld()
        {
            DrawingUtilities.DrawRect(Viewport.X, Viewport.Y, Viewport.Width, Viewport.Height, ' ', true);

            CurrentLevel.DrawLevel(Viewport);
        }
        public static void DrawGameWorld(Point offset)
        {
            DrawingUtilities.DrawRect(Viewport.X, Viewport.Y, Viewport.Width, Viewport.Height, ' ', true);

            CurrentLevel.DrawLevel(Viewport, offset);
        }

        public static void Step()
        {
            UpdateStep();
            DrawStep();
        }

        public static void SetupGame(PlayerStats stats)
        {
            //TestLevel = Factories.LevelGenerator.GenerateLevel(1, false);
            TestDungeon = Factories.DungeonGenerator.GenerateDungeon();
            currentLevel = TestDungeon.DungeonLevels[0];

            spawnPlayer(stats);

            Pathing.PathCalculator.CacheLevel(CurrentLevel);
        }

        public static void SetCameraOffset()
        {
            GameManager.CameraOffset.X = Player.X - GraphicConsole.BufferWidth / 2;
            GameManager.CameraOffset.Y = Player.Y - GraphicConsole.BufferHeight / 2;
        }

        private static void spawnPlayer(PlayerStats stats)
        {
            int x = 1;
            int y = 1;

            if (CurrentLevel.Rooms.Count > 0)
            {
                int room = RNG.Next(0, CurrentLevel.Rooms.Count);

                x = CurrentLevel.Rooms[room].X + 1;
                y = CurrentLevel.Rooms[room].Y + 1;
            }

            Player = new Player(CurrentLevel) { X = x, Y = y, Token = '@', ForegroundColor = Color.Gold, IsSolid = true };

            Player.StatsPackage = stats;
            Player.PlayerStats.SetInitialStats();

            Player.StatsPackage.CalculateStats();
            Player.StatsPackage = Inventory.CalculateStats(Player.StatsPackage);

            Player.StatsPackage.Health = (int)Player.StatsPackage.MaxHealth.EffectiveValue;
            Player.StatsPackage.Mana = (int)Player.StatsPackage.MaxMana.EffectiveValue;

            Player.StatsPackage.ParentEntity = Player;
            Player.PlayerStats.ParentEntity = Player;

            populateSpellbook();
            SetCameraOffset();

            Game.Items.Inventory.PlayerInventory.Add(new Game.Items.Test.Dagger_Retribution());
            Game.Items.Inventory.PlayerInventory.Add(new Game.Items.Test.Dagger_Retribution());

            CurrentLevel.Entities.Add(Player);
        }
        private static void populateSpellbook()
        {
            for (int i = 1; i < Player.StatsPackage.AbilityList.Count; i++)
            {
                SpellBook.SetSpell(i, Player.StatsPackage.AbilityList[i]);
            }
            SpellBook.SetSpell(0, Player.StatsPackage.AbilityList[0]);
        }
    }

    public enum GameStates
    {
        MainMenu,
        Game,
        CharacterCreation,
        InterfaceTest,
        Dead,
        Map,
        Win,
        Options,
        ItemTesting,
        About
    }
}
