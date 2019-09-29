using System;
using OpenTK.Graphics;
using Roguelike.Core;
using Roguelike.Core.Entities;
using Roguelike.Core.Items;
using Roguelike.Core.Entities.Static;

namespace Roguelike.Engine.Factories
{
    public static class LevelGenerator
    {
        private const int InitialChance = 0;
        private static int levelCounter = 0;
        private static Rectangle center;

        public static Level GenerateLevel(int difficulty, bool exportImage)
        {
            int width = 256;
            int height = 256;

            Level level = new Level(width, height);

            level = carveOutCenterCorridors(level);
            level = generateCorridors(level);
            level = generateRooms(level);
            level = populateDoors(level);
            level = generateItems(level);

            for (int y = 0; y < level.Matrix.Height; y++)
            {
                for (int x = 0; x < level.Matrix.Width; x++)
                {
                    if (level.Matrix.TerrainMatrix[x, y].TileLoc == Tile.TileLocation.Corridor)
                        level.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = TokenReference.FLOOR_EMPTY, ForegroundColor = level.Matrix.TerrainMatrix[x, y].ForegroundColor, TileLoc = Tile.TileLocation.Corridor };
                    else if (level.Matrix.TerrainMatrix[x, y].TileLoc == Tile.TileLocation.Room)
                        level.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = TokenReference.FLOOR_EMPTY, ForegroundColor = level.Matrix.TerrainMatrix[x, y].ForegroundColor, TileLoc = Tile.TileLocation.Room };
                    else if (level.Matrix.TerrainMatrix[x, y].TileLoc == Tile.TileLocation.Wall)
                        level.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = true, Token = TokenReference.WALL, ForegroundColor = level.Matrix.TerrainMatrix[x, y].ForegroundColor, TileLoc = Tile.TileLocation.Wall };
                    else if (level.Matrix.TerrainMatrix[x, y].TileLoc == Tile.TileLocation.Door)
                    {
                        level.Matrix.TerrainMatrix[x, y] = new Tile(x, y) { IsSolid = false, Token = TokenReference.FLOOR_EMPTY, TileLoc = Tile.TileLocation.Door };
                        Door door = new Door(level) { X = x, Y = y };
                        level.Entities.Add(door);
                    }
                }
            }
            
            if (exportImage)
                ExportPNG(level);

            return level;
        }

        private static Level carveOutCenterCorridors(Level level)
        {
            center = new Rectangle(0, 0, level.Matrix.Width / 3, level.Matrix.Height / 3);
            center.X += center.Width; center.Y += center.Height;

            if (center.Left % 2 == 0)
                center.X++;
            if (center.Top % 2 == 0)
                center.Y++;

            if (center.Right % 2 == 0)
                center.Width++;
            if (center.Bottom % 2 == 0)
                center.Height++;

            for (int y = center.Top; y < center.Bottom; y++)
            {
                level.Matrix.TerrainMatrix[center.Left, y].TileLoc = Tile.TileLocation.Corridor;
                level.Matrix.TerrainMatrix[center.Right, y].TileLoc = Tile.TileLocation.Corridor;
            }
            for (int x = center.Left; x < center.Right; x++)
            {
                level.Matrix.TerrainMatrix[x, center.Top].TileLoc = Tile.TileLocation.Corridor;
                level.Matrix.TerrainMatrix[x, center.Bottom].TileLoc = Tile.TileLocation.Corridor;
            }

            return level;
        }
        private static Level generateCorridors(Level level)
        {
            level = generateCorridorWing(level, new Point(center.Left, center.Top));
            level = generateCorridorWing(level, new Point(center.Right, center.Top));
            level = generateCorridorWing(level, new Point(center.Left, center.Bottom));
            level = generateCorridorWing(level, new Point(center.Right, center.Bottom));

            level = generateCorridorWing(level, new Point(center.Left + center.Width / 2, center.Top));
            level = generateCorridorWing(level, new Point(center.Right, center.Top + center.Height / 2));
            level = generateCorridorWing(level, new Point(center.Left, center.Top + center.Height / 2));
            level = generateCorridorWing(level, new Point(center.Left + center.Width / 2, center.Top + center.Height / 2));

            return level;
        }
        private static Level generateCorridorWing(Level level, Point startPoint)
        {
            if (startPoint.X % 2 == 0)
                startPoint.X++;
            if (startPoint.Y % 2 == 0)
                startPoint.Y++;

            Point currentPoint = startPoint;
            Point destination = new Point(startPoint.X, startPoint.Y - 2);

            Direction currentDirection = Direction.Down;

            int chanceToChange = InitialChance;
            for (int i = 0; i < 100; i++)
            {
                #region DrawSegment
                if (currentDirection == Direction.Up)
                {
                    destination.Y = currentPoint.Y - 2;
                    if (level.IsOutOfBounds(destination.X, destination.Y - 1))
                    {
                        i--;
                        chanceToChange = 100;
                        destination = currentPoint;
                    }
                    else
                    {
                        level.Matrix.TerrainMatrix[currentPoint.X, currentPoint.Y].TileLoc = Tile.TileLocation.Corridor;
                        level.Matrix.TerrainMatrix[currentPoint.X, currentPoint.Y - 1].TileLoc = Tile.TileLocation.Corridor;
                        level.Matrix.TerrainMatrix[currentPoint.X, currentPoint.Y - 2].TileLoc = Tile.TileLocation.Corridor;
                    }
                }
                else if (currentDirection == Direction.Down)
                {
                    destination.Y = currentPoint.Y + 2;
                    if (level.IsOutOfBounds(destination.X, destination.Y + 1))
                    {
                        i--;
                        chanceToChange = 100;
                        destination = currentPoint;
                    }
                    else
                    {
                        level.Matrix.TerrainMatrix[currentPoint.X, currentPoint.Y].TileLoc = Tile.TileLocation.Corridor;
                        level.Matrix.TerrainMatrix[currentPoint.X, currentPoint.Y + 1].TileLoc = Tile.TileLocation.Corridor;
                        level.Matrix.TerrainMatrix[currentPoint.X, currentPoint.Y + 2].TileLoc = Tile.TileLocation.Corridor;
                    }
                }
                else if (currentDirection == Direction.Left)
                {
                    destination.X = currentPoint.X - 2;
                    if (level.IsOutOfBounds(destination.X - 1, destination.Y))
                    {
                        i--;
                        chanceToChange = 100;
                        destination = currentPoint;
                    }
                    else
                    {
                        level.Matrix.TerrainMatrix[currentPoint.X, currentPoint.Y].TileLoc = Tile.TileLocation.Corridor;
                        level.Matrix.TerrainMatrix[currentPoint.X - 1, currentPoint.Y].TileLoc = Tile.TileLocation.Corridor;
                        level.Matrix.TerrainMatrix[currentPoint.X - 2, currentPoint.Y].TileLoc = Tile.TileLocation.Corridor;
                    }
                }
                else if (currentDirection == Direction.Right)
                {
                    destination.X = currentPoint.X + 2;
                    if (level.IsOutOfBounds(destination.X + 1, destination.Y))
                    {
                        i--;
                        chanceToChange = 100;
                        destination = currentPoint;
                    }
                    else
                    {
                        level.Matrix.TerrainMatrix[currentPoint.X, currentPoint.Y].TileLoc = Tile.TileLocation.Corridor;
                        level.Matrix.TerrainMatrix[currentPoint.X + 1, currentPoint.Y].TileLoc = Tile.TileLocation.Corridor;
                        level.Matrix.TerrainMatrix[currentPoint.X + 2, currentPoint.Y].TileLoc = Tile.TileLocation.Corridor;
                    }
                }
                #endregion

                //Do we change direction?
                int rand = Engine.RNG.Next(1, 101);
                if (rand >= 100 - chanceToChange)
                {
                    chanceToChange = InitialChance;
                    currentDirection = getPerpDirection(currentDirection);
                }
                else
                {
                    chanceToChange += 2;
                }

                currentPoint = destination;
            }

            return level;
        }
        private static Level generateRooms(Level level)
        {
            int roomsToGenerate = 25;
            for (int i = 0; i < roomsToGenerate; i++)
            {
                var roomRect = new Rectangle();

                do
                {
                    roomRect.Width = Engine.RNG.Next(5, 15);
                    roomRect.Height = Engine.RNG.Next(5, 15);

                    if (roomRect.Width % 2 == 0)
                        roomRect.Width++;
                    if (roomRect.Height % 2 == 0)
                        roomRect.Height++;

                    roomRect.X = Engine.RNG.Next(1, level.Matrix.Width - roomRect.Width - 2);
                    roomRect.Y = Engine.RNG.Next(1, level.Matrix.Height - roomRect.Height - 2);

                    if (roomRect.X % 2 == 0)
                        roomRect.X++;
                    if (roomRect.Y % 2 == 0)
                        roomRect.Y++;

                } while (!isRoomSafe(level, roomRect));

                for (int y = roomRect.Top; y < roomRect.Bottom; y++)
                {
                    for (int x = roomRect.Left; x < roomRect.Right; x++)
                    {
                        level.Matrix.TerrainMatrix[x, y].TileLoc = Tile.TileLocation.Room;
                    }
                }

                var room = new Room() { X = roomRect.X, Y = roomRect.Y, Width = roomRect.Width, Height = roomRect.Height };
                level.Rooms.Add(room);
            }

            return level;
        }
        private static Level populateDoors(Level level)
        {
            for (int i = 0; i < level.Rooms.Count; i++)
            {
                //Search along the top/bottom of the room
                for (int x = level.Rooms[i].X - 1; x <= level.Rooms[i].X + level.Rooms[i].Width; x++)
                {
                    if (!level.IsOutOfBounds(x, level.Rooms[i].Y - 1))
                    {
                        if (level.Matrix.TerrainMatrix[x, level.Rooms[i].Y - 1].TileLoc == Tile.TileLocation.Corridor)
                            level.Matrix.TerrainMatrix[x, level.Rooms[i].Y - 1].TileLoc = Tile.TileLocation.Door;
                    }
                    if (!level.IsOutOfBounds(x, level.Rooms[i].Y + level.Rooms[i].Height))
                    {
                        if (level.Matrix.TerrainMatrix[x, level.Rooms[i].Y + level.Rooms[i].Height].TileLoc == Tile.TileLocation.Corridor)
                            level.Matrix.TerrainMatrix[x, level.Rooms[i].Y + level.Rooms[i].Height].TileLoc = Tile.TileLocation.Door;
                    }
                }

                //Search along the left/right of the room
                for (int y = level.Rooms[i].Y - 1; y <= level.Rooms[i].Y + level.Rooms[i].Height; y++)
                {
                    if (!level.IsOutOfBounds(level.Rooms[i].X - 1, y))
                    {
                        if (level.Matrix.TerrainMatrix[level.Rooms[i].X - 1, y].TileLoc == Tile.TileLocation.Corridor)
                            level.Matrix.TerrainMatrix[level.Rooms[i].X - 1, y].TileLoc = Tile.TileLocation.Door;
                    }
                    if (!level.IsOutOfBounds(level.Rooms[i].X + level.Rooms[i].Width, y))
                    {
                        if (level.Matrix.TerrainMatrix[level.Rooms[i].X + level.Rooms[i].Width, y].TileLoc == Tile.TileLocation.Corridor)
                            level.Matrix.TerrainMatrix[level.Rooms[i].X + level.Rooms[i].Width, y].TileLoc = Tile.TileLocation.Door;
                    }
                }
            }

            return level;
        }
        private static Level generateItems(Level level)
        {
            int roomNumber = level.Rooms.Count;
            int items = roomNumber * 2 + Engine.RNG.Next(-10, 10);

            for (int i = 0; i < items; i++)
            {
                int room = Engine.RNG.Next(0, roomNumber);
                Point position = new Point(Engine.RNG.Next(level.Rooms[room].Left, level.Rooms[room].Right), Engine.RNG.Next(level.Rooms[room].Top, level.Rooms[room].Bottom));

                Item item = ItemGenerator.GenerateRandomItem();
                item.Position = position;
                item.ParentLevel = level;

                level.FloorItems.Add(item);
            }

            return level;
        }

        public static void GenerateLadders(Level source, Level target)
        {
            //Find a safe spot for the ladder on the source level
            Point position = new Point();
            Point destination = new Point();

            int sourceRoom = Engine.RNG.Next(0, source.Rooms.Count);
            position.X = Engine.RNG.Next(source.Rooms[sourceRoom].Left + 1, source.Rooms[sourceRoom].Right - 1);
            position.Y = Engine.RNG.Next(source.Rooms[sourceRoom].Top + 1, source.Rooms[sourceRoom].Bottom - 1);

            int targetRoom = Engine.RNG.Next(0, target.Rooms.Count);
            destination.X = Engine.RNG.Next(target.Rooms[targetRoom].Left + 1, target.Rooms[targetRoom].Right - 1);
            destination.Y = Engine.RNG.Next(target.Rooms[targetRoom].Top + 1, target.Rooms[targetRoom].Bottom - 1);

            source.DownwardLadder = new Ladder(source, target, destination) { X = position.X, Y = position.Y, Token = TokenReference.LADDER_DOWN };
            target.UpwardLadder = new Ladder(target, source, position) { X = destination.X, Y = destination.Y, Token = TokenReference.LADDER_UP };

            source.Matrix.TerrainMatrix[position.X, position.Y].TileLoc = Tile.TileLocation.Ladder;
            target.Matrix.TerrainMatrix[destination.X, destination.Y].TileLoc = Tile.TileLocation.Ladder;

            source.Entities.Add(source.DownwardLadder);
            target.Entities.Add(target.UpwardLadder);

            target.UpwardLadder.ForegroundColor = Color4.DarkGoldenrod;
        }
        public static Level GenerateWinShrine(Level level)
        {
            Point position = new Point(level.UpwardLadder.X, level.UpwardLadder.Y);
            int ladderRoom = -1;

            for (int i = 0; i < level.Rooms.Count; i++)
            {
                if (level.Rooms[i].Contains(position.X, position.Y))
                {
                    ladderRoom = i;
                    break;
                }
            }

            int targetRoom = -1;
            do
            {
                targetRoom = Engine.RNG.Next(0, level.Rooms.Count);
            } while (targetRoom == ladderRoom);

            position.X = Engine.RNG.Next(level.Rooms[targetRoom].Left, level.Rooms[targetRoom].Right);
            position.Y = Engine.RNG.Next(level.Rooms[targetRoom].Top, level.Rooms[targetRoom].Bottom);

            WinShrine shrine = new WinShrine(level) { X = position.X, Y = position.Y };
            level.Entities.Add(shrine);

            return level;
        }

        //Call Generate Level => carveOutCenterCorridors => generateCorridors => generateRooms => populateDoors

        //Utility Methods
        public static void ExportPNG(Level level)
        {
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(level.Matrix.Width, level.Matrix.Height))
            {
                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap))
                {
                    graphics.Clear(System.Drawing.Color.Black);

                    for (int y = 0; y < level.Matrix.Height; y++)
                    {
                        for (int x = 0; x < level.Matrix.Width; x++)
                        {
                            if (level.Matrix.TerrainMatrix[x, y].TileLoc == Tile.TileLocation.Corridor)
                                graphics.FillRectangle(System.Drawing.Brushes.White, x, y, 1, 1);
                            else if (level.Matrix.TerrainMatrix[x, y].TileLoc == Tile.TileLocation.Room)
                                graphics.FillRectangle(System.Drawing.Brushes.Green, x, y, 1, 1);
                            else if (level.Matrix.TerrainMatrix[x, y].TileLoc == Tile.TileLocation.Door)
                                graphics.FillRectangle(System.Drawing.Brushes.Red, x, y, 1, 1);
                            else if (level.Matrix.TerrainMatrix[x, y].TileLoc == Tile.TileLocation.Ladder)
                                graphics.FillRectangle(System.Drawing.Brushes.Purple, x, y, 1, 1);
                        }
                    }
                }

                System.IO.Directory.CreateDirectory("Export");
                bitmap.Save("Export/dungeon_" + levelCounter.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                levelCounter++;
            }
        }
        private static Direction getPerpDirection(Direction direction)
        {
            int rand = Engine.RNG.Next(0, 11);

            if (direction == Direction.Down || direction == Direction.Up)
            {
                if (rand >= 5)
                    return Direction.Left;
                return Direction.Right;
            }
            else if (direction == Direction.Left || direction == Direction.Right)
            {
                if (rand >= 5)
                    return Direction.Up;
                return Direction.Down;
            }

            return Direction.Right;
        }
        private static bool isRoomSafe(Level level, Rectangle rect)
        {
            bool isConnectedToCorridor = false;

            for (int y = rect.Top; y < rect.Bottom; y++)
            {
                for (int x = rect.Left; x < rect.Right; x++)
                {
                    if (level.Matrix.TerrainMatrix[x, y].TileLoc == Tile.TileLocation.Corridor)
                        isConnectedToCorridor = true;
                    if (level.Matrix.TerrainMatrix[x, y].TileLoc == Tile.TileLocation.Room)
                        return false;
                }
            }

            return isConnectedToCorridor;
        }

        private enum Direction { Up, Down, Left, Right }
    }
}
