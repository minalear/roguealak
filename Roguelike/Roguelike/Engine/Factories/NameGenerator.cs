using System;
using System.IO;

namespace Roguelike.Engine.Factories
{
    public static class NameGenerator
    {
        private static string[] firstNames, lastNames, townNames, titles, dungeonNames;

        public static void Initialize()
        {
            firstNames = File.ReadAllLines("Content/Localization/First Names.txt");
            lastNames = File.ReadAllLines("Content/Localization/Last Names.txt");
            dungeonNames = File.ReadAllLines("Content/Localization/Dungeon Names.txt");
        }

        public static string GenerateFirstName()
        {
            return firstNames[RNG.Next(0, firstNames.Length)];
        }

        public static string GenerateLastName()
        {
            return lastNames[RNG.Next(0, lastNames.Length)];
        }

        public static string GenerateTownName()
        {
            return "";
        }

        public static string GenerateTitle()
        {
            return "";
        }

        public static string GenerateDungeonName()
        {
            return dungeonNames[RNG.Next(0, dungeonNames.Length)];
        }
    }
}
