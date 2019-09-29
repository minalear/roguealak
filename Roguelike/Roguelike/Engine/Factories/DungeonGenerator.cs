using System;
using Roguelike.Core;
using Roguelike.Core.Entities;

namespace Roguelike.Engine.Factories
{
    public static class DungeonGenerator
    {
        public static Dungeon GenerateDungeon()
        {
            Dungeon dungeon = new Dungeon(6); //Town + 5 Dungeon Levels
            dungeon = GenerateLevels(dungeon);
            dungeon.DungeonName = Factories.NameGenerator.GenerateDungeonName();

            return dungeon;
        }

        public static Dungeon GenerateLevels(Dungeon dungeon)
        {
            //Generate Levels
            dungeon.DungeonLevels[0] = Factories.TownGenerator.GenerateTown(@"Textures/testTown");
            for (int i = 1; i < dungeon.NumberOfLevels; i++)
            {
                dungeon.DungeonLevels[i] = Factories.LevelGenerator.GenerateLevel(i, false);
            }

            //Connect Levels with Ladders
            dungeon.DungeonLevels[0] = Factories.TownGenerator.CreateLadder(dungeon.DungeonLevels[0], dungeon.DungeonLevels[1]);
            for (int i = 1; i < dungeon.NumberOfLevels - 1; i++)
            {
                Factories.LevelGenerator.GenerateLadders(dungeon.DungeonLevels[i], dungeon.DungeonLevels[i + 1]);
            }

            dungeon.DungeonLevels[dungeon.DungeonLevels.Length - 1] = LevelGenerator.GenerateWinShrine(dungeon.DungeonLevels[dungeon.DungeonLevels.Length - 1]);

            //Export PNGs of Levels
            for (int i = 1; i < dungeon.NumberOfLevels; i++)
            {
                //Factories.LevelGenerator.ExportPNG(dungeon.DungeonLevels[i]);
                dungeon.DungeonLevels[i] = setupSomeEnemies(dungeon.DungeonLevels[i]);
            }

            return dungeon;
        }

        private static Level setupSomeEnemies(Level level)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = 1;
                int y = 1;

                if (level.Rooms.Count > 0)
                {
                    int room = Engine.RNG.Next(0, level.Rooms.Count);

                    x = level.Rooms[room].X + 1;
                    y = level.Rooms[room].Y + 1;
                }

                PathingEntity entity = new PathingEntity(level) { X = x, Y = y };
                level.Entities.Add(entity);
            }

            return level;
        }
    }
}
