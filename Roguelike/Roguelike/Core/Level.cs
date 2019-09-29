using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using Roguelike.Core.Entities;
using Roguelike.Core.Items;
using Roguelike.Engine;

namespace Roguelike.Core
{
    /// <summary>
    /// A single level in a dungeon.  It is one dimension in terms of depth, but can vary in terms of width and height.
    /// </summary>
    public class Level
    {
        public Level(int width, int height)
        {
            entities = new List<Entity>();
            rooms = new List<Room>();
            floorItems = new List<Item>();

            levelMatrix = new LevelMatrix(width, height);
        }

        public void DrawLevel(Box2 viewport)
        {
            levelMatrix.DrawStep(viewport);

            for (int i = 0; i < floorItems.Count; i++)
                floorItems[i].DrawStep(viewport);

            for (int i = 0; i < entities.Count; i++)
                entities[i].DrawStep(viewport);
        }
        public void DrawLevel(Box2 viewport, Point offset)
        {
            levelMatrix.DrawStep(viewport, offset);
        }
        public void Update(GameTime gameTime)
        {
            levelMatrix.Update(gameTime);
            for (int i = 0; i < entities.Count; i++)
                entities[i].Update(gameTime);
        }
        public void UpdateStep()
        {
            ClearLayer(MatrixLevels.Effect);
            GameManager.Player.UpdateStep();

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].EntityType != Entity.EntityTypes.Player)
                {
                    entities[i].UpdateStep();
                    if (entities[i].DoPurge)
                    {
                        entities.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        if (entities[i].Tile.IsVisible)
                            levelMatrix.EntityMatrix[entities[i].X, entities[i].Y].Token = entities[i].Token;
                    }
                }
            }

            levelMatrix.UpdateStep();
        }

        public void RevealTile(int x, int y)
        {
            if (!IsOutOfBounds(x, y))
            {
                levelMatrix.TerrainMatrix[x, y].IsVisible = true;
                levelMatrix.TerrainMatrix[x, y].WasVisible = true;
            }
        }
        public void RevealTile(Circle area)
        {
            List<Point> revealedTiles = new List<Point>();
            for (int angle = 0; angle < 360; angle += 1)
            {
                for (int r = 0; r < area.Radius; r++)
                {
                    Point position = new Point();
                    position.X = (int)(area.X + 0.5 + r * Math.Cos(angle));
                    position.Y = (int)(area.Y + 0.5 + r * Math.Sin(angle));

                    if (!revealedTiles.Contains(position))
                    {
                        RevealTile(position.X, position.Y);
                        revealedTiles.Add(position);
                    }
                }
            }
        }
        public void HideTile(int x, int y)
        {
            if (!IsOutOfBounds(x, y))
            {
                levelMatrix.TerrainMatrix[x, y].IsVisible = false;
                levelMatrix.TerrainMatrix[x, y].WasVisible = false;
            }
        }
        public void HideTile(Circle area)
        {
            List<Point> hidTiles = new List<Point>();
            for (int angle = 0; angle < 360; angle += 1)
            {
                for (int r = 0; r < area.Radius; r++)
                {
                    Point position = new Point();
                    position.X = (int)(area.X + 0.5 + r * Math.Cos(angle));
                    position.Y = (int)(area.Y + 0.5 + r * Math.Sin(angle));

                    if (!hidTiles.Contains(position))
                    {
                        HideTile(position.X, position.Y);
                        hidTiles.Add(position);
                    }
                }
            }
        }
        public void StainTile(int x, int y, Color4 color)
        {
            if (!IsOutOfBounds(x, y))
            {
                var tileColor = levelMatrix.TerrainMatrix[x, y].ForegroundColor;
                var average = new Color4()
                {
                    R = (byte)((color.R + tileColor.R) / 2),
                    G = (byte)((color.G + tileColor.G) / 2),
                    B = (byte)((color.B + tileColor.B) / 2),
                    A = 255
                };
                levelMatrix.TerrainMatrix[x, y].ForegroundColor = average;
            }
        }
        public void StainTile(Circle area, Color4 color)
        {
            var stainedTiles = new List<Point>();
            for (int angle = 0; angle < 360; angle += 1)
            {
                for (int r = 0; r < area.Radius; r++)
                {
                    Point position = new Point();
                    position.X = (int)(area.X + 0.5 + r * Math.Cos(angle));
                    position.Y = (int)(area.Y + 0.5 + r * Math.Sin(angle));

                    if (!stainedTiles.Contains(position))
                    {
                        StainTile(position.X, position.Y, color);
                        stainedTiles.Add(position);
                    }
                }
            }
        }
        public void StainTile(Box2 area, Color4 color)
        {
            for (int y = (int)area.Top; y < area.Bottom; y++)
            {
                for (int x = (int)area.Left; x < area.Right; x++)
                {
                    StainTile(x, y, color);
                }
            }
        }
        public void DamageTile(int x, int y)
        {
            if (!IsOutOfBounds(x, y))
                levelMatrix.TerrainMatrix[x, y].DamageTile();
        }
        public void DamateTile(Circle area)
        {
            List<Point> damagedTiles = new List<Point>();
            for (int angle = 0; angle < 360; angle += 1)
            {
                for (int r = 0; r < area.Radius; r++)
                {
                    Point position = new Point();
                    position.X = (int)(area.X + 0.5 + r * Math.Cos(angle));
                    position.Y = (int)(area.Y + 0.5 + r * Math.Sin(angle));

                    if (!damagedTiles.Contains(position))
                    {
                        DamageTile(position.X, position.Y);
                        damagedTiles.Add(position);
                    }
                }
            }
        }
        private void breakTile(int x, int y)
        {
            if (x >= 0 && x < levelMatrix.Width && y >= 0 && y < levelMatrix.Height)
            {
                if (levelMatrix.TerrainMatrix[x, y].IsSolid)
                {
                    if (RNG.Next(0, 5) == 0) //Generate Rubble
                    {
                        levelMatrix.TerrainMatrix[x, y].Token = TokenReference.FLOOR_RUBBLE;
                        levelMatrix.TerrainMatrix[x, y].IsSolid = false;
                    }
                    else                     //Leave empty space
                    {
                        levelMatrix.TerrainMatrix[x, y].Token = TokenReference.FLOOR_EMPTY;
                        levelMatrix.TerrainMatrix[x, y].IsSolid = false;
                    }
                }
            }
        }

        public bool IsOutOfBounds(int x, int y)
        {
            if (x < 0 || x >= levelMatrix.Width || y < 0 || y >= levelMatrix.Height)
                return true;
            return false;
        }
        public bool IsTileSolid(int x, int y)
        {
            if (IsOutOfBounds(x, y))
                return true;
            else if (levelMatrix.TerrainMatrix[x, y].IsSolid)
                return true;

            return false;
        }
        public bool IsBlockedByEntity(int x, int y)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].X == x && entities[i].Y == y && entities[i].IsSolid)
                    return true;
            }

            return false;
        }
        public Entity GetEntity(int x, int y)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].X == x && entities[i].Y == y)
                    return entities[i];
            }

            return null;
        }
        public List<Entity> GetEntities(Circle area)
        {
            List<Entity> entityList = new List<Entity>();

            List<Point> scannedTiles = new List<Point>();
            for (int angle = 0; angle < 360; angle += 1)
            {
                for (int r = 0; r < area.Radius; r++)
                {
                    Point position = new Point();
                    position.X = (int)(area.X + 0.5 + r * Math.Cos(angle));
                    position.Y = (int)(area.Y + 0.5 + r * Math.Sin(angle));

                    if (!scannedTiles.Contains(position))
                    {
                        Entity entity = GetEntity(position.X, position.Y);
                        if (entity != null)
                            entityList.Add(entity);
                    }
                }
            }

            return entityList;
        }
        public List<Entity> GetEntities(Box2 area)
        {
            var entityList = new List<Entity>();

            for (int y = (int)area.Top; y < area.Bottom; y++)
            {
                for (int x = (int)area.Left; x < area.Right; x++)
                {
                    Entity entity = GetEntity(x, y);
                    if (entity != null)
                        entityList.Add(entity);
                }
            }

            return entityList;
        }
        public bool CanMoveTo(int x, int y)
        {
            if (IsTileSolid(x, y))
                return false;
            if (IsBlockedByEntity(x, y))
                return false;

            return true;
        }
        public bool IsLineOfSight(Point point0, Point point1)
        {
            bool steep = Math.Abs(point1.Y - point0.Y) > Math.Abs(point1.X - point0.X);
            if (steep) { Swap<int>(ref point0.X, ref point0.Y); Swap<int>(ref point1.X, ref point1.Y); }
            if (point0.X > point1.X) { Swap<int>(ref point0.X, ref point1.X); Swap<int>(ref point0.Y, ref point1.Y); }
            int dX = (point1.X - point0.X), dY = Math.Abs(point1.Y - point0.Y), err = (dX / 2), ystep = (point0.Y < point1.Y ? 1 : -1), y = point0.Y;

            for (int x = point0.X; x <= point1.X; ++x)
            {
                /*if (!(steep ? plot(y, x) : plot(x, y))) return;*/
                if (steep)
                {
                    if (IsTileSolid(y, x))
                        return false;
                }
                else
                {
                    if (IsTileSolid(x, y))
                        return false;
                }

                err = err - dY;
                if (err < 0) { y += ystep; err += dX; }
            }

            return true;
        }

        public void SetToken(MatrixLevels level, int x, int y, char token)
        {
            if (!IsOutOfBounds(x, y))
            {
                if (level == MatrixLevels.Terrain)
                {
                    levelMatrix.TerrainMatrix[x, y].Token = token;
                }
                else if (level == MatrixLevels.Effect)
                {
                    levelMatrix.EffectMatrix[x, y].Token = token;
                }
                else if (level == MatrixLevels.Entity)
                {
                    levelMatrix.EntityMatrix[x, y].Token = token;
                }
            }
        }
        public void SetToken(MatrixLevels level, int x, int y, char token, Color4 fore, Color4 back)
        {
            if (!IsOutOfBounds(x, y))
            {
                if (level == MatrixLevels.Terrain)
                {
                    levelMatrix.TerrainMatrix[x, y].Token = token;
                    levelMatrix.TerrainMatrix[x, y].ForegroundColor = fore;
                    levelMatrix.TerrainMatrix[x, y].BackgroundColor = back;
                }
                else if (level == MatrixLevels.Effect)
                {
                    levelMatrix.EffectMatrix[x, y].Token = token;
                    levelMatrix.EffectMatrix[x, y].ForegroundColor = fore;
                    levelMatrix.EffectMatrix[x, y].BackgroundColor = back;
                }
                else if (level == MatrixLevels.Entity)
                {
                    levelMatrix.EntityMatrix[x, y].Token = token;
                    levelMatrix.EntityMatrix[x, y].ForegroundColor = fore;
                    levelMatrix.EntityMatrix[x, y].BackgroundColor = back;
                }
            }
        }
        public void ClearLayer(MatrixLevels level)
        {
            if (level == MatrixLevels.Effect)
            {
                for (int y = 0; y < levelMatrix.Height; y++)
                {
                    for (int x = 0; x < levelMatrix.Width; x++)
                    {
                        levelMatrix.EffectMatrix[x, y].ForegroundColor = Color4.White;
                        levelMatrix.EffectMatrix[x, y].BackgroundColor = Color4.Black;
                        levelMatrix.EffectMatrix[x, y].Token = ' ';
                    }
                }
            }
            else if (level == MatrixLevels.Entity)
            {
                for (int y = 0; y < levelMatrix.Height; y++)
                {
                    for (int x = 0; x < levelMatrix.Width; x++)
                    {
                        levelMatrix.EntityMatrix[x, y].ForegroundColor = Color4.White;
                        levelMatrix.EntityMatrix[x, y].BackgroundColor = Color4.Black;
                        levelMatrix.EntityMatrix[x, y].Token = ' ';
                    }
                }
            }
        }
        public void DrawCircle(MatrixLevels level, Circle area, char token, Color4 fore, Color4 back, bool solid)
        {
            if (!solid)
            {
                #region NonSolid Circle
                int x = area.Radius, y = 0;
                int radiusError = 1 - x;

                while (x >= y)
                {
                    SetToken(level, x + area.X, y + area.Y, token, fore, back);
                    SetToken(level, y + area.X, x + area.Y, token, fore, back);
                    SetToken(level, -x + area.X, y + area.Y, token, fore, back);
                    SetToken(level, -y + area.X, x + area.Y, token, fore, back);
                    SetToken(level, -x + area.X, -y + area.Y, token, fore, back);
                    SetToken(level, -y + area.X, -x + area.Y, token, fore, back);
                    SetToken(level, x + area.X, -y + area.Y, token, fore, back);
                    SetToken(level, y + area.X, -x + area.Y, token, fore, back);

                    SetToken(level, x + area.X, y + area.Y, token);

                    y++;
                    if (radiusError < 0)
                    {
                        radiusError += 2 * y + 1;
                    }
                    else
                    {
                        x--;
                        radiusError += 2 * (y - x + 1);
                    }
                }
                #endregion
            }
            else
            {
                #region SolidCircle
                List<Point> stainedTiles = new List<Point>();
                for (int angle = 0; angle < 360; angle += 1)
                {
                    for (int r = 0; r < area.Radius; r++)
                    {
                        Point position = new Point();
                        position.X = (int)(area.X + 0.5 + r * Math.Cos(angle));
                        position.Y = (int)(area.Y + 0.5 + r * Math.Sin(angle));

                        if (!stainedTiles.Contains(position))
                        {
                            SetToken(level, position.X, position.Y, token, fore, back);
                            stainedTiles.Add(position);
                        }
                    }
                }
                #endregion
            }
        }
        public void DrawLine(MatrixLevels level, Point point0, Point point1, char token)
        {
            bool steep = Math.Abs(point1.Y - point0.Y) > Math.Abs(point1.X - point0.X);
            if (steep) { Swap<int>(ref point0.X, ref point0.Y); Swap<int>(ref point1.X, ref point1.Y); }
            if (point0.X > point1.X) { Swap<int>(ref point0.X, ref point1.X); Swap<int>(ref point0.Y, ref point1.Y); }
            int dX = (point1.X - point0.X), dY = Math.Abs(point1.Y - point0.Y), err = (dX / 2), ystep = (point0.Y < point1.Y ? 1 : -1), y = point0.Y;

            for (int x = point0.X; x <= point1.X; ++x)
            {
                /*if (!(steep ? plot(y, x) : plot(x, y))) return;*/
                if (steep)
                    GraphicConsole.Put(token, y, x);
                else
                    GraphicConsole.Put(token, x, y);

                err = err - dY;
                if (err < 0) { y += ystep; err += dX; }
            }
        }
        private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }
        public void DrawRectangle(MatrixLevels level, Box2 rectangle, char token, bool solid)
        {
            if (!solid)
            {
                for (int y = (int)rectangle.Top; y < rectangle.Bottom; y++)
                {
                    SetToken(level, (int)rectangle.Left, y, token);
                    SetToken(level, (int)rectangle.Right - 1, y, token);
                }
                for (int x = (int)rectangle.Left; x < rectangle.Right; x++)
                {
                    SetToken(level, x, (int)rectangle.Top, token);
                    SetToken(level, x, (int)rectangle.Bottom - 1, token);
                }
            }
            else
            {
                for (int y = (int)rectangle.Top; y < rectangle.Bottom; y++)
                {
                    for (int x = (int)rectangle.Left; x < rectangle.Right; x++)
                    {
                        SetToken(level, x, y, token);
                    }
                }
            }
        }
        public void DrawRectangle(MatrixLevels level, Box2 rectangle, char token, bool solid, Color4 fore, Color4 back)
        {
            if (!solid)
            {
                for (int y = (int)rectangle.Top; y < rectangle.Bottom; y++)
                {
                    SetToken(level, (int)rectangle.Left, y, token, fore, back);
                    SetToken(level, (int)rectangle.Right - 1, y, token, fore, back);
                }
                for (int x = (int)rectangle.Left; x < rectangle.Right; x++)
                {
                    SetToken(level, x, (int)rectangle.Top, token, fore, back);
                    SetToken(level, x, (int)rectangle.Bottom - 1, token, fore, back);
                }
            }
            else
            {
                for (int y = (int)rectangle.Top; y < rectangle.Bottom; y++)
                {
                    for (int x = (int)rectangle.Left; x < rectangle.Right; x++)
                    {
                        SetToken(level, x, y, token, fore, back);
                    }
                }
            }
        }

        public void PickupItem(Item item)
        {
            floorItems.Remove(item);
        }
        public void DropItem(Item item, int x, int y)
        {
            item.ParentLevel = this;
            item.Position = new Point(x, y);

            floorItems.Add(item);
        }

        private string name = "[Level]";
        private LevelMatrix levelMatrix;
        private List<Entity> entities;
        private List<Room> rooms;
        private List<Item> floorItems;

        public string Name { get { return name; } set { name = value; } }
        public LevelMatrix Matrix { get { return levelMatrix; } set { levelMatrix = value; } }
        public List<Entity> Entities { get { return entities; } set { entities = value; } }
        public List<Room> Rooms { get { return rooms; } set { rooms = value; } }
        public List<Item> FloorItems { get { return floorItems; } set { floorItems = value; } }

        public Ladder UpwardLadder, DownwardLadder;
    }

    /// <summary>
    /// Singular tile in a level with a X,Y Coordinate
    /// </summary>
    public struct Tile
    {
        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;

            isSolid = false;
            isVisible = false;
            wasVisible = false;

            token = ' ';
            tileHealth = 10;

            foregroundColor = Color4.White;
            backgroundColor = Color4.Black;

            tileLoc = TileLocation.Wall;
        }

        public void Draw(int coordX, int coordY)
        {
            if (isVisible)
            {
                GraphicConsole.SetColors(foregroundColor, backgroundColor);
                GraphicConsole.Put(token, coordX, coordY);
            }
            else if (wasVisible)
            {
                var halfColor = foregroundColor;
                halfColor.A = 125;

                GraphicConsole.SetColors(halfColor, backgroundColor);
                GraphicConsole.Put(token, coordX, coordY);
            }
        }
        public void DamageTile()
        {
            if (TileLoc == TileLocation.Wall)
            {
                tileHealth--;
                if (tileHealth < 0)
                {
                    tileHealth = 0;
                    return;
                }

                if (tileHealth >= 4)
                    token = TokenReference.WALL_4;
                else if (tileHealth == 3)
                    token = TokenReference.WALL_3;
                else if (tileHealth == 2)
                    token = TokenReference.WALL_2;
                else if (tileHealth == 1)
                    token = TokenReference.WALL_1;
                else if (tileHealth == 0)
                {
                    token = TokenReference.FLOOR_EMPTY;
                    isSolid = false;
                }
            }
        }

        private int x, y;
        private bool isSolid;
        private bool isVisible;
        private bool wasVisible;

        private char token;
        private int tileHealth;

        private Color4 foregroundColor;
        private Color4 backgroundColor;

        private TileLocation tileLoc;

        #region Properties
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }

        public bool IsSolid { get { return isSolid; } set { isSolid = value; } }
        public bool IsVisible { get { return isVisible; } set { isVisible = value; } }
        public bool WasVisible { get { return wasVisible; } set { wasVisible = value; } }

        public char Token { get { return token; } set { token = value; } }
        
        public Color4 ForegroundColor { get { return foregroundColor; } set { foregroundColor = value; } }
        public Color4 BackgroundColor { get { return backgroundColor; } set { backgroundColor = value; } }
        
        public TileLocation TileLoc { get { return tileLoc; } set { tileLoc = value; } }
        #endregion

        public enum TileLocation { Wall, Solid, Corridor, Room, Door, Ladder }
    }
    public struct TokenTile
    {
        public int X, Y;
        public char Token;
        public Color4 ForegroundColor, BackgroundColor;

        public TokenTile(int x, int y)
        {
            X = x;
            Y = y;

            Token = ' ';
            ForegroundColor = Color4.White;
            BackgroundColor = Color4.Black;
        }

        public void Draw(int coordX, int coordY)
        {
            if (Token != ' ')
            {
                GraphicConsole.SetColors(ForegroundColor, BackgroundColor);
                GraphicConsole.Put(Token, coordX, coordY);
            }
        }
    }
    public struct Room
    {
        public int X, Y;
        public int Width, Height;
        public string RoomType;

        public bool Contains(int x, int y)
        {
            if (x >= Left && x < Right && y >= Top && y < Bottom)
                return true;
            return false;
        }

        public int Left { get { return X; } }
        public int Right { get { return X + Width; } }
        public int Top { get { return Y; } }
        public int Bottom { get { return Y + Height; } }
    }

    public struct Circle
    {
        public int X, Y;
        public int Radius;
    }
    public struct Line
    {
        public int X0, Y0;
        public int X1, Y1;
    }

    public class LevelMatrix
    {
        private int width, height;
        private Tile[,] terrainMatrix;
        private TokenTile[,] effectMatrix, entityMatrix;
        private double timer = 0.0;
        private bool displayEffects = true;

        public LevelMatrix(int width, int height)
        {
            this.width = width;
            this.height = height;

            terrainMatrix = new Tile[width, height];
            effectMatrix = new TokenTile[width, height];
            entityMatrix = new TokenTile[width, height];

            initializeMatrix();
        }

        public void DrawStep(Box2 viewport)
        {
            for (int y = (int)viewport.Top; y < viewport.Bottom; y++)
            {
                for (int x = (int)viewport.Left; x < viewport.Right; x++)
                {
                    int coordX = GameManager.CameraOffset.X + x - (int)viewport.Left;
                    int coordY = GameManager.CameraOffset.Y + y - (int)viewport.Top;

                    if (coordX >= 0 && coordX < width && coordY >= 0 && coordY < height)
                    {
                        terrainMatrix[coordX, coordY].Draw(x, y);

                        if (displayEffects)
                            effectMatrix[coordX, coordY].Draw(x, y);

                        entityMatrix[coordX, coordY].Draw(x, y);
                    }
                }
            }
        }
        public void DrawStep(Box2 viewport, Point offset)
        {
            for (int y = (int)viewport.Top; y < viewport.Bottom; y++)
            {
                for (int x = (int)viewport.Left; x < viewport.Right; x++)
                {
                    int coordX = offset.X + x - (int)viewport.Left;
                    int coordY = offset.Y + y - (int)viewport.Top;

                    if (coordX >= 0 && coordX < width && coordY >= 0 && coordY < height)
                    {
                        terrainMatrix[coordX, coordY].Draw(x, y);
                    }
                }
            }
        }
        public void UpdateStep()
        {
            timer = 0.0;
            displayEffects = true;
        }
        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedTime.Milliseconds;
            if (timer >= 740.0)
            {
                displayEffects = !displayEffects;
                timer = 0.0;

                Engine.UI.InterfaceManager.DrawStep();
            }
        }

        private void initializeMatrix()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    terrainMatrix[x, y] = new Tile(x, y);
                    effectMatrix[x, y] = new TokenTile(x, y);
                    entityMatrix[x, y] = new TokenTile(x, y) { ForegroundColor = new Color4(255, 255, 255, 125) };
                }
            }
        }

        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public Tile[,] TerrainMatrix { get { return terrainMatrix; } }
        public TokenTile[,] EffectMatrix { get { return effectMatrix; } }
        public TokenTile[,] EntityMatrix { get { return entityMatrix; } }
    }
    public enum MatrixLevels { Terrain, Effect, Entity }
}