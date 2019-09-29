using System;
using OpenTK.Graphics;
using Roguelike.Engine.Console;
using Roguelike.Engine.UI;
using Roguelike.Core;
using Roguelike.Core.Entities;
using Roguelike.Core.Stats;
using Roguelike.Core.Combat;
using Roguelike.Core.Items;

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

        public static int FakeScore = 0;
        public static int SweetRolls = 0;

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
                CurrentLevel.Update(gameTime);

            InterfaceManager.Update(gameTime);
        }

        public static void UpdateStep()
        {
            if (CurrentGameState == GameStates.Game)
            {
                CurrentLevel.UpdateStep();
                Pathing.PathCalculator.UpdateStep();
            }

            InterfaceManager.UpdateStep();
        }

        public static void DrawStep()
        {
            GraphicConsole.Instance.Clear();

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
            CameraOffset.X = Player.X - GraphicConsole.Instance.BufferWidth / 2;
            CameraOffset.Y = Player.Y - GraphicConsole.Instance.BufferHeight / 2;
        }

        public static void ResetGame()
        {
            SpellBook.ClearSpells();
            Inventory.ClearInventory();

            MessageCenter.MessageLog.Clear();

            FakeScore = 0;
            SweetRolls = 0;
        }

        private static void spawnPlayer(PlayerStats stats)
        {
            int x = 1;
            int y = 1;

            if (CurrentLevel.Rooms.Count > 0)
            {
                int room = Engine.RNG.Next(0, CurrentLevel.Rooms.Count);

                x = CurrentLevel.Rooms[room].X + 1;
                y = CurrentLevel.Rooms[room].Y + 1;
            }

            Player = new Player(CurrentLevel) { X = x, Y = y, Token = '@', ForegroundColor = Color4.Gold, IsSolid = true };

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

            /*Game.Items.Inventory.PlayerInventory.Add(new Game.Items.Test.Dagger_Retribution());
            Game.Items.Inventory.PlayerInventory.Add(new Game.Items.Test.Dagger_Retribution());*/

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
