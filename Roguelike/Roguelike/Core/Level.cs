using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Core.Entities;
using Roguelike.Core.Items;

namespace Roguelike.Core
{
    /// <summary>
    /// A single level in a dungeon.  It is one dimension in terms of depth, but can vary in terms of width and height.
    /// </summary>
    public class Level
    {
        public Level(int width, int height)
        {
            this.entities = new List<Entity>();
            this.rooms = new List<Room>();
            this.floorItems = new List<Item>();

            this.levelMatrix = new LevelMatrix(width, height);
        }

        public void DrawLevel(Rectangle viewport)
        {
            this.levelMatrix.DrawStep(viewport);

            for (int i = 0; i < this.floorItems.Count; i++)
                this.floorItems[i].DrawStep(viewport);

            for (int i = 0; i < this.entities.Count; i++)
                this.entities[i].DrawStep(viewport);
        }
        public void DrawLevel(Rectangle viewport, Point offset)
        {
            this.levelMatrix.DrawStep(viewport, offset);
        }
        public void Update(GameTime gameTime)
        {
            levelMatrix.Update(gameTime);
            for (int i = 0; i < this.entities.Count; i++)
                this.entities[i].Update(gameTime);
        }
        public void UpdateStep()
        {
            this.ClearLayer(MatrixLevels.Effect);
            GameManager.Player.UpdateStep();

            for (int i = 0; i < this.entities.Count; i++)
            {
                if (this.entities[i].EntityType != Entity.EntityTypes.Player)
                {
                    this.entities[i].UpdateStep();
                    if (this.entities[i].DoPurge)
                    {
                        this.entities.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        if (this.entities[i].Tile.IsVisible)
                            this.levelMatrix.EntityMatrix[this.entities[i].X, this.entities[i].Y].Token = this.entities[i].Token;
                    }
                }
            }

            levelMatrix.UpdateStep();
        }

        public void RevealTile(int x, int y)
        {
            if (!this.IsOutOfBounds(x, y))
            {
                this.levelMatrix.TerrainMatrix[x, y].IsVisible = true;
                this.levelMatrix.TerrainMatrix[x, y].WasVisible = true;
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
                        this.RevealTile(position.X, position.Y);
                        revealedTiles.Add(position);
                    }
                }
            }
        }
        public void HideTile(int x, int y)
        {
            if (!this.IsOutOfBounds(x, y))
            {
                this.levelMatrix.TerrainMatrix[x, y].IsVisible = false;
                this.levelMatrix.TerrainMatrix[x, y].WasVisible = false;
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
                        this.HideTile(position.X, position.Y);
                        hidTiles.Add(position);
                    }
                }
            }
        }
        public void StainTile(int x, int y, Color color)
        {
            if (!this.IsOutOfBounds(x, y))
            {
                Color tileColor = this.levelMatrix.TerrainMatrix[x, y].ForegroundColor;
                Color average = new Color()
                {
                    R = (byte)((color.R + tileColor.R) / 2),
                    G = (byte)((color.G + tileColor.G) / 2),
                    B = (byte)((color.B + tileColor.B) / 2),
                    A = 255
                };
                this.levelMatrix.TerrainMatrix[x, y].ForegroundColor = average;
            }
        }
        public void StainTile(Circle area, Color color)
        {
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
                        this.StainTile(position.X, position.Y, color);
                        stainedTiles.Add(position);
                    }
                }
            }
        }
        public void StainTile(Rectangle area, Color color)
        {
            for (int y = area.Top; y < area.Bottom; y++)
            {
                for (int x = area.Left; x < area.Right; x++)
                {
                    this.StainTile(x, y, color);
                }
            }
        }
        public void DamageTile(int x, int y)
        {
            if (!this.IsOutOfBounds(x, y))
                this.levelMatrix.TerrainMatrix[x, y].DamageTile();
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
                        this.DamageTile(position.X, position.Y);
                        damagedTiles.Add(position);
                    }
                }
            }
        }
        private void breakTile(int x, int y)
        {
            if (x >= 0 && x < this.levelMatrix.Width && y >= 0 && y < this.levelMatrix.Height)
            {
                if (this.levelMatrix.TerrainMatrix[x, y].IsSolid)
                {
                    if (RNG.Next(0, 5) == 0) //Generate Rubble
                    {
                        this.levelMatrix.TerrainMatrix[x, y].Token = TokenReference.FLOOR_RUBBLE;
                        this.levelMatrix.TerrainMatrix[x, y].IsSolid = false;
                    }
                    else                     //Leave empty space
                    {
                        this.levelMatrix.TerrainMatrix[x, y].Token = TokenReference.FLOOR_EMPTY;
                        this.levelMatrix.TerrainMatrix[x, y].IsSolid = false;
                    }
                }
            }
        }

        public bool IsOutOfBounds(int x, int y)
        {
            if (x < 0 || x >= this.levelMatrix.Width || y < 0 || y >= this.levelMatrix.Height)
                return true;
            return false;
        }
        public bool IsTileSolid(int x, int y)
        {
            if (this.IsOutOfBounds(x, y))
                return true;
            else if (this.levelMatrix.TerrainMatrix[x, y].IsSolid)
                return true;

            return false;
        }
        public bool IsBlockedByEntity(int x, int y)
        {
            for (int i = 0; i < this.entities.Count; i++)
            {
                if (this.entities[i].X == x && this.entities[i].Y == y && this.entities[i].IsSolid)
                    return true;
            }

            return false;
        }
        public Entity GetEntity(int x, int y)
        {
            for (int i = 0; i < this.entities.Count; i++)
            {
                if (this.entities[i].X == x && this.entities[i].Y == y)
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
                        Entity entity = this.GetEntity(position.X, position.Y);
                        if (entity != null)
                            entityList.Add(entity);
                    }
                }
            }

            return entityList;
        }
        public List<Entity> GetEntities(Rectangle area)
        {
            List<Entity> entityList = new List<Entity>();

            for (int y = area.Top; y < area.Bottom; y++)
            {
                for (int x = area.Left; x < area.Right; x++)
                {
                    Entity entity = this.GetEntity(x, y);
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
                    if (this.IsTileSolid(y, x))
                        return false;
                }
                else
                {
                    if (this.IsTileSolid(x, y))
                        return false;
                }

                err = err - dY;
                if (err < 0) { y += ystep; err += dX; }
            }

            return true;
        }

        public void SetToken(MatrixLevels level, int x, int y, char token)
        {
            if (!this.IsOutOfBounds(x, y))
            {
                if (level == MatrixLevels.Terrain)
                {
                    this.levelMatrix.TerrainMatrix[x, y].Token = token;
                }
                else if (level == MatrixLevels.Effect)
                {
                    this.levelMatrix.EffectMatrix[x, y].Token = token;
                }
                else if (level == MatrixLevels.Entity)
                {
                    this.levelMatrix.EntityMatrix[x, y].Token = token;
                }
            }
        }
        public void SetToken(MatrixLevels level, int x, int y, char token, Color fore, Color back)
        {
            if (!this.IsOutOfBounds(x, y))
            {
                if (level == MatrixLevels.Terrain)
                {
                    this.levelMatrix.TerrainMatrix[x, y].Token = token;
                    this.levelMatrix.TerrainMatrix[x, y].ForegroundColor = fore;
                    this.levelMatrix.TerrainMatrix[x, y].BackgroundColor = back;
                }
                else if (level == MatrixLevels.Effect)
                {
                    this.levelMatrix.EffectMatrix[x, y].Token = token;
                    this.levelMatrix.EffectMatrix[x, y].ForegroundColor = fore;
                    this.levelMatrix.EffectMatrix[x, y].BackgroundColor = back;
                }
                else if (level == MatrixLevels.Entity)
                {
                    this.levelMatrix.EntityMatrix[x, y].Token = token;
                    this.levelMatrix.EntityMatrix[x, y].ForegroundColor = fore;
                    this.levelMatrix.EntityMatrix[x, y].BackgroundColor = back;
                }
            }
        }
        public void ClearLayer(MatrixLevels level)
        {
            if (level == MatrixLevels.Effect)
            {
                for (int y = 0; y < this.levelMatrix.Height; y++)
                {
                    for (int x = 0; x < this.levelMatrix.Width; x++)
                    {
                        this.levelMatrix.EffectMatrix[x, y].ForegroundColor = Color.White;
                        this.levelMatrix.EffectMatrix[x, y].BackgroundColor = Color.Black;
                        this.levelMatrix.EffectMatrix[x, y].Token = ' ';
                    }
                }
            }
            else if (level == MatrixLevels.Entity)
            {
                for (int y = 0; y < this.levelMatrix.Height; y++)
                {
                    for (int x = 0; x < this.levelMatrix.Width; x++)
                    {
                        this.levelMatrix.EntityMatrix[x, y].ForegroundColor = Color.White;
                        this.levelMatrix.EntityMatrix[x, y].BackgroundColor = Color.Black;
                        this.levelMatrix.EntityMatrix[x, y].Token = ' ';
                    }
                }
            }
        }
        public void DrawCircle(MatrixLevels level, Circle area, char token, Color fore, Color back, bool solid)
        {
            if (!solid)
            {
                #region NonSolid Circle
                int x = area.Radius, y = 0;
                int radiusError = 1 - x;

                while (x >= y)
                {
                    this.SetToken(level, x + area.X, y + area.Y, token, fore, back);
                    this.SetToken(level, y + area.X, x + area.Y, token, fore, back);
                    this.SetToken(level, -x + area.X, y + area.Y, token, fore, back);
                    this.SetToken(level, -y + area.X, x + area.Y, token, fore, back);
                    this.SetToken(level, -x + area.X, -y + area.Y, token, fore, back);
                    this.SetToken(level, -y + area.X, -x + area.Y, token, fore, back);
                    this.SetToken(level, x + area.X, -y + area.Y, token, fore, back);
                    this.SetToken(level, y + area.X, -x + area.Y, token, fore, back);

                    this.SetToken(level, x + area.X, y + area.Y, token);

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
                            this.SetToken(level, position.X, position.Y, token, fore, back);
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
        public void DrawRectangle(MatrixLevels level, Rectangle rectangle, char token, bool solid)
        {
            if (!solid)
            {
                for (int y = rectangle.Top; y < rectangle.Bottom; y++)
                {
                    this.SetToken(level, rectangle.Left, y, token);
                    this.SetToken(level, rectangle.Right - 1, y, token);
                }
                for (int x = rectangle.Left; x < rectangle.Right; x++)
                {
                    this.SetToken(level, x, rectangle.Top, token);
                    this.SetToken(level, x, rectangle.Bottom - 1, token);
                }
            }
            else
            {
                for (int y = rectangle.Top; y < rectangle.Bottom; y++)
                {
                    for (int x = rectangle.Left; x < rectangle.Right; x++)
                    {
                        this.SetToken(level, x, y, token);
                    }
                }
            }
        }
        public void DrawRectangle(MatrixLevels level, Rectangle rectangle, char token, bool solid, Color fore, Color back)
        {
            if (!solid)
            {
                for (int y = rectangle.Top; y < rectangle.Bottom; y++)
                {
                    this.SetToken(level, rectangle.Left, y, token, fore, back);
                    this.SetToken(level, rectangle.Right - 1, y, token, fore, back);
                }
                for (int x = rectangle.Left; x < rectangle.Right; x++)
                {
                    this.SetToken(level, x, rectangle.Top, token, fore, back);
                    this.SetToken(level, x, rectangle.Bottom - 1, token, fore, back);
                }
            }
            else
            {
                for (int y = rectangle.Top; y < rectangle.Bottom; y++)
                {
                    for (int x = rectangle.Left; x < rectangle.Right; x++)
                    {
                        this.SetToken(level, x, y, token, fore, back);
                    }
                }
            }
        }

        public void PickupItem(Item item)
        {
            this.floorItems.Remove(item);
        }
        public void DropItem(Item item, int x, int y)
        {
            item.ParentLevel = this;
            item.Position = new Point(x, y);

            this.floorItems.Add(item);
        }

        private string name = "[Level]";
        private LevelMatrix levelMatrix;
        private List<Entity> entities;
        private List<Room> rooms;
        private List<Item> floorItems;

        public string Name { get { return this.name; } set { this.name = value; } }
        public LevelMatrix Matrix { get { return this.levelMatrix; } set { this.levelMatrix = value; } }
        public List<Entity> Entities { get { return this.entities; } set { this.entities = value; } }
        public List<Room> Rooms { get { return this.rooms; } set { this.rooms = value; } }
        public List<Item> FloorItems { get { return this.floorItems; } set { this.floorItems = value; } }

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

            this.isSolid = false;
            this.isVisible = false;
            this.wasVisible = false;

            this.token = ' ';
            this.tileHealth = 10;

            this.foregroundColor = Color.White;
            this.backgroundColor = Color.Black;

            this.tileLoc = TileLocation.Wall;
        }

        public void Draw(int coordX, int coordY)
        {
            if (this.isVisible)
            {
                GraphicConsole.SetColors(this.foregroundColor, this.backgroundColor);
                GraphicConsole.Put(this.token, coordX, coordY);
            }
            else if (this.wasVisible)
            {
                Color halfColor = this.foregroundColor;
                halfColor.A = 125;

                GraphicConsole.SetColors(halfColor, this.backgroundColor);
                GraphicConsole.Put(this.token, coordX, coordY);
            }
        }
        public void DamageTile()
        {
            if (this.TileLoc == TileLocation.Wall)
            {
                this.tileHealth--;
                if (this.tileHealth < 0)
                {
                    this.tileHealth = 0;
                    return;
                }

                if (this.tileHealth >= 4)
                    this.token = TokenReference.WALL_4;
                else if (this.tileHealth == 3)
                    this.token = TokenReference.WALL_3;
                else if (this.tileHealth == 2)
                    this.token = TokenReference.WALL_2;
                else if (this.tileHealth == 1)
                    this.token = TokenReference.WALL_1;
                else if (this.tileHealth == 0)
                {
                    this.token = TokenReference.FLOOR_EMPTY;
                    this.isSolid = false;
                }
            }
        }

        private int x, y;
        private bool isSolid;
        private bool isVisible;
        private bool wasVisible;

        private char token;
        private int tileHealth;

        private Color foregroundColor;
        private Color backgroundColor;

        private TileLocation tileLoc;

        #region Properties
        public int X { get { return this.x; } set { this.x = value; } }
        public int Y { get { return this.y; } set { this.y = value; } }

        public bool IsSolid { get { return this.isSolid; } set { this.isSolid = value; } }
        public bool IsVisible { get { return this.isVisible; } set { this.isVisible = value; } }
        public bool WasVisible { get { return this.wasVisible; } set { this.wasVisible = value; } }

        public char Token { get { return this.token; } set { this.token = value; } }
        
        public Color ForegroundColor { get { return this.foregroundColor; } set { this.foregroundColor = value; } }
        public Color BackgroundColor { get { return this.backgroundColor; } set { this.backgroundColor = value; } }
        
        public TileLocation TileLoc { get { return this.tileLoc; } set { this.tileLoc = value; } }
        #endregion

        public enum TileLocation { Wall, Solid, Corridor, Room, Door, Ladder }
    }
    public struct TokenTile
    {
        public int X, Y;
        public char Token;
        public Color ForegroundColor, BackgroundColor;

        public TokenTile(int x, int y)
        {
            this.X = x;
            this.Y = y;

            this.Token = ' ';
            this.ForegroundColor = Color.White;
            this.BackgroundColor = Color.Black;
        }

        public void Draw(int coordX, int coordY)
        {
            if (this.Token != ' ')
            {
                GraphicConsole.SetColors(this.ForegroundColor, this.BackgroundColor);
                GraphicConsole.Put(this.Token, coordX, coordY);
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
            if (x >= this.Left && x < this.Right && y >= this.Top && y < this.Bottom)
                return true;
            return false;
        }

        public int Left { get { return this.X; } }
        public int Right { get { return this.X + this.Width; } }
        public int Top { get { return this.Y; } }
        public int Bottom { get { return this.Y + this.Height; } }
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

            this.terrainMatrix = new Tile[width, height];
            this.effectMatrix = new TokenTile[width, height];
            this.entityMatrix = new TokenTile[width, height];

            this.initializeMatrix();
        }

        public void DrawStep(Rectangle viewport)
        {
            for (int y = viewport.Top; y < viewport.Bottom; y++)
            {
                for (int x = viewport.Left; x < viewport.Right; x++)
                {
                    int coordX = GameManager.CameraOffset.X + x - viewport.X;
                    int coordY = GameManager.CameraOffset.Y + y - viewport.Y;

                    if (coordX >= 0 && coordX < this.width && coordY >= 0 && coordY < this.height)
                    {
                        terrainMatrix[coordX, coordY].Draw(x, y);

                        if (displayEffects)
                            effectMatrix[coordX, coordY].Draw(x, y);

                        entityMatrix[coordX, coordY].Draw(x, y);
                    }
                }
            }
        }
        public void DrawStep(Rectangle viewport, Point offset)
        {
            for (int y = viewport.Top; y < viewport.Bottom; y++)
            {
                for (int x = viewport.Left; x < viewport.Right; x++)
                {
                    int coordX = offset.X + x - viewport.X;
                    int coordY = offset.Y + y - viewport.Y;

                    if (coordX >= 0 && coordX < this.width && coordY >= 0 && coordY < this.height)
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
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer >= 740.0)
            {
                displayEffects = !displayEffects;
                timer = 0.0;

                UI.InterfaceManager.DrawStep();
            }
        }

        private void initializeMatrix()
        {
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    terrainMatrix[x, y] = new Tile(x, y);
                    effectMatrix[x, y] = new TokenTile(x, y);
                    entityMatrix[x, y] = new TokenTile(x, y) { ForegroundColor = new Color(255, 255, 255, 125) };
                }
            }
        }

        public int Width { get { return this.width; } set { this.width = value; } }
        public int Height { get { return this.height; } set { this.height = value; } }
        public Tile[,] TerrainMatrix { get { return this.terrainMatrix; } }
        public TokenTile[,] EffectMatrix { get { return this.effectMatrix; } }
        public TokenTile[,] EntityMatrix { get { return this.entityMatrix; } }
    }
    public enum MatrixLevels { Terrain, Effect, Entity }
}