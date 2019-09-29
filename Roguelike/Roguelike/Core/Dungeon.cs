using System;
using System.Collections.Generic;
using Roguelike.Core.Entities;

namespace Roguelike.Core
{
    public class Dungeon
    {
        public string DungeonName;
        public int NumberOfLevels;
        public Level[] DungeonLevels;

        public Dungeon(int numberOfLevels)
        {
            NumberOfLevels = numberOfLevels;
            DungeonLevels = new Level[NumberOfLevels];
        }
    }
}
