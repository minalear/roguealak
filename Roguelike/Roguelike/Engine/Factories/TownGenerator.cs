using System;
using Roguelike.Core;
using Roguelike.Core.Entities;
using Roguelike.Core.Entities.Static;
using Roguelike.Core.Items;

namespace Roguelike.Engine.Factories
{
    public static class TownGenerator
    {
        private static char[] groundTextureTokens = new char[] { ' ', ',', '`', '·' };
        private static Color[] groundColors = new Color[] { new Color(41, 63, 41, 155), new Color(49, 69, 2, 155), new Color(68, 92, 75, 155), new Color(20, 104, 18, 155), new Color(97, 116, 76, 155) };

        public static Level GenerateTown(string path)
        {
            Texture2D townBlueprint = Program.Content.Load<Texture2D>(path);
            Level town = new Level(townBlueprint.Width, townBlueprint.Height);

            Color[] colorMatrix = new Color[town.Matrix.Width * town.Matrix.Height];
            townBlueprint.GetData<Color>(colorMatrix);

            int index = 0;
            for (int y = 0; y < town.Matrix.Height; y++)
            {
                for (int x = 0; x < town.Matrix.Width; x++)
                {
                    if (colorMatrix[index] == Color.Black) //Wall
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = true, Token = '█', ForegroundColor = Color.White, TileLoc = Tile.TileLocation.Wall };
                    else if (colorMatrix[index] == new Color(117, 76, 36)) //Tree
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = true, Token = '↑', ForegroundColor = Color.Green, TileLoc = Tile.TileLocation.Solid };
                    else if (colorMatrix[index] == new Color(247, 148, 29)) //Path
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '░', ForegroundColor = new Color(200, 165, 135), TileLoc = Tile.TileLocation.Corridor };
                    else if (colorMatrix[index] == new Color(194, 194, 194)) //Road
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '▒', ForegroundColor = new Color(200, 165, 135), TileLoc = Tile.TileLocation.Corridor };
                    else if (colorMatrix[index] == Color.White) //Ground
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = getGroundCharacter(), ForegroundColor = getGroundColor(), TileLoc = Tile.TileLocation.Corridor };

                    else if (colorMatrix[index] == new Color(115, 99, 87)) //Blacksmith
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '■', ForegroundColor = new Color(54, 54, 54), BackgroundColor = new Color(25, 25, 25), TileLoc = Tile.TileLocation.Corridor };
                    else if (colorMatrix[index] == new Color(153, 134, 117)) //General Goods
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '■', ForegroundColor = new Color(143, 107, 75), BackgroundColor = new Color(114, 80, 25), TileLoc = Tile.TileLocation.Corridor };
                    else if (colorMatrix[index] == new Color(160, 65, 13)) //House
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '■', ForegroundColor = new Color(128, 52, 25), BackgroundColor = new Color(99, 23, 0), TileLoc = Tile.TileLocation.Corridor };

                    else if (colorMatrix[index] == Color.Red) //Door
                    {
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '·', TileLoc = Tile.TileLocation.Door };
                        Door door = new Door(town) { X = x, Y = y };
                        town.Entities.Add(door);
                    }
                    else if (colorMatrix[index] == new Color(0, 52, 113)) //Training Dummy
                    {
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '·', TileLoc = Tile.TileLocation.Room };
                        PracticeDummy dummy = new PracticeDummy(town) { X = x, Y = y };
                        town.Entities.Add(dummy);
                    }
                    else if (colorMatrix[index] == new Color(161, 134, 190)) //Chest
                    {
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = ' ', TileLoc = Tile.TileLocation.Room };
                        Chest chest = new Chest(town) { X = x, Y = y };
                        town.Entities.Add(chest);
                    }

                    else if (colorMatrix[index] == new Color(0, 191, 243)) //Dungeon Ladder
                    {
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = ' ', TileLoc = Tile.TileLocation.Ladder };
                    }

                    index++;
                }
            }

            for (int y = 0; y < town.Matrix.Height; y++)
            {
                for (int x = 0; x < town.Matrix.Width; x++)
                {
                    town.Matrix.TerrainMatrix[x, y].WasVisible = true;
                }
            }

            return town;
        }
        public static Level CreateLadder(Level town, Level target)
        {
            //Find a safe spot for the ladder on the source level
            Point position = new Point();
            for (int y = 0; y < town.Matrix.Height; y++)
            {
                for (int x = 0; x < town.Matrix.Width; x++)
                {
                    if (town.Matrix.TerrainMatrix[x, y].TileLoc == Tile.TileLocation.Ladder)
                    {
                        position = new Point(x, y);
                        break;
                    }
                }
            }
            Point destination = new Point();
            int targetRoom = RNG.Next(0, target.Rooms.Count);
            destination.X = RNG.Next(target.Rooms[targetRoom].Left, target.Rooms[targetRoom].Right);
            destination.Y = RNG.Next(target.Rooms[targetRoom].Top, target.Rooms[targetRoom].Bottom);

            town.DownwardLadder = new Ladder(town, target, destination) { X = position.X, Y = position.Y, Token = TokenReference.LADDER_DOWN };
            target.UpwardLadder = new Ladder(target, town, position) { X = destination.X, Y = destination.Y, Token = TokenReference.LADDER_UP };

            target.Matrix.TerrainMatrix[destination.X, destination.Y].TileLoc = Tile.TileLocation.Ladder;

            town.Entities.Add(town.DownwardLadder);
            target.Entities.Add(target.UpwardLadder);

            target.UpwardLadder.ForegroundColor = Color.DarkGoldenrod;

            return town;
        }

        private static char getGroundCharacter()
        {
            int result = RNG.Next(0, 100);

            if (result <= 70)
                return groundTextureTokens[0];
            else if (result <= 80)
                return groundTextureTokens[1];
            else if (result <= 90)
                return groundTextureTokens[2];
            return groundTextureTokens[3];
        }
        private static Color getGroundColor()
        {
            return groundColors[RNG.Next(0, groundColors.Length)];
        }
    }
}
