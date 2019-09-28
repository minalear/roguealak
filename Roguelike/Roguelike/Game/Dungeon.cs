using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Roguelike.Engine.Game.Entities;

namespace Roguelike.Engine.Game
{
    public class Dungeon
    {
        public string DungeonName;
        public int NumberOfLevels;
        public Level[] DungeonLevels;

        public Dungeon(int numberOfLevels)
        {
            this.NumberOfLevels = numberOfLevels;
            this.DungeonLevels = new Level[this.NumberOfLevels];
        }
    }
}
