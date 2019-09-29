using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics;
using Roguelike.Core;
using Roguelike.Core.Entities;
using Roguelike.Core.Entities.Static;
using Roguelike.Core.Items;

namespace Roguelike.Engine.Factories
{
    public static class TownGenerator
    {
        private static char[] groundTextureTokens = new char[] { ' ', ',', '`', '·' };
        private static Color4[] groundColors = new Color4[] { new Color4(41, 63, 41, 155), new Color4(49, 69, 2, 155), new Color4(68, 92, 75, 155), new Color4(20, 104, 18, 155), new Color4(97, 116, 76, 155) };

        public static Level GenerateTown(string path)
        {
            //var townBlueprint = Program.Content.Load<Texture2D>(path);

            Level town;
            Color4[] colorMatrix;
            using (var townBlueprint = new Bitmap(path))
            {
                var data = townBlueprint.LockBits(new System.Drawing.Rectangle(0, 0, townBlueprint.Width, townBlueprint.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                var ptr = data.Scan0;

                //townBlueprint.GetData<Color4>(colorMatrix);
                int bytes = Math.Abs(data.Stride) * townBlueprint.Height;
                byte[] rgbValues = new byte[bytes];

                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                // TODO: Fill out colormatrix with texture data https://docs.microsoft.com/en-us/dotnet/api/system.drawing.imaging.bitmapdata?view=netframework-4.8

                town = new Level(townBlueprint.Width, townBlueprint.Height);
                colorMatrix = new Color4[town.Matrix.Width * town.Matrix.Height];
            }

            int index = 0;
            for (int y = 0; y < town.Matrix.Height; y++)
            {
                for (int x = 0; x < town.Matrix.Width; x++)
                {
                    if (colorMatrix[index] == Color4.Black) //Wall
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = true, Token = '█', ForegroundColor = Color4.White, TileLoc = Tile.TileLocation.Wall };
                    else if (colorMatrix[index] == new Color4(117, 76, 36, 255)) //Tree
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = true, Token = '↑', ForegroundColor = Color4.Green, TileLoc = Tile.TileLocation.Solid };
                    else if (colorMatrix[index] == new Color4(247, 148, 29, 255)) //Path
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '░', ForegroundColor = new Color4(200, 165, 135, 255), TileLoc = Tile.TileLocation.Corridor };
                    else if (colorMatrix[index] == new Color4(194, 194, 194, 255)) //Road
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '▒', ForegroundColor = new Color4(200, 165, 135, 255), TileLoc = Tile.TileLocation.Corridor };
                    else if (colorMatrix[index] == Color4.White) //Ground
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = getGroundCharacter(), ForegroundColor = getGroundColor(), TileLoc = Tile.TileLocation.Corridor };

                    else if (colorMatrix[index] == new Color4(115, 99, 87, 255)) //Blacksmith
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '■', ForegroundColor = new Color4(54, 54, 54, 255), BackgroundColor = new Color4(25, 25, 25, 255), TileLoc = Tile.TileLocation.Corridor };
                    else if (colorMatrix[index] == new Color4(153, 134, 117, 255)) //General Goods
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '■', ForegroundColor = new Color4(143, 107, 75, 255), BackgroundColor = new Color4(114, 80, 25, 255), TileLoc = Tile.TileLocation.Corridor };
                    else if (colorMatrix[index] == new Color4(160, 65, 13, 255)) //House
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '■', ForegroundColor = new Color4(128, 52, 25, 255), BackgroundColor = new Color4(99, 23, 0, 255), TileLoc = Tile.TileLocation.Corridor };

                    else if (colorMatrix[index] == Color4.Red) //Door
                    {
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '·', TileLoc = Tile.TileLocation.Door };
                        Door door = new Door(town) { X = x, Y = y };
                        town.Entities.Add(door);
                    }
                    else if (colorMatrix[index] == new Color4(0, 52, 113, 255)) //Training Dummy
                    {
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = '·', TileLoc = Tile.TileLocation.Room };
                        PracticeDummy dummy = new PracticeDummy(town) { X = x, Y = y };
                        town.Entities.Add(dummy);
                    }
                    else if (colorMatrix[index] == new Color4(161, 134, 190, 255)) //Chest
                    {
                        town.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = ' ', TileLoc = Tile.TileLocation.Room };
                        Chest chest = new Chest(town) { X = x, Y = y };
                        town.Entities.Add(chest);
                    }

                    else if (colorMatrix[index] == new Color4(0, 191, 243, 255)) //Dungeon Ladder
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
            int targetRoom = Engine.RNG.Next(0, target.Rooms.Count);
            destination.X = Engine.RNG.Next(target.Rooms[targetRoom].Left, target.Rooms[targetRoom].Right);
            destination.Y = Engine.RNG.Next(target.Rooms[targetRoom].Top, target.Rooms[targetRoom].Bottom);

            town.DownwardLadder = new Ladder(town, target, destination) { X = position.X, Y = position.Y, Token = TokenReference.LADDER_DOWN };
            target.UpwardLadder = new Ladder(target, town, position) { X = destination.X, Y = destination.Y, Token = TokenReference.LADDER_UP };

            target.Matrix.TerrainMatrix[destination.X, destination.Y].TileLoc = Tile.TileLocation.Ladder;

            town.Entities.Add(town.DownwardLadder);
            target.Entities.Add(target.UpwardLadder);

            target.UpwardLadder.ForegroundColor = Color4.DarkGoldenrod;

            return town;
        }

        private static char getGroundCharacter()
        {
            int result = Engine.RNG.Next(0, 100);

            if (result <= 70)
                return groundTextureTokens[0];
            else if (result <= 80)
                return groundTextureTokens[1];
            else if (result <= 90)
                return groundTextureTokens[2];
            return groundTextureTokens[3];
        }
        private static Color4 getGroundColor()
        {
            return groundColors[Engine.RNG.Next(0, groundColors.Length)];
        }
    }
}
