using System;
using Roguelike.Core.Stats;
using Roguelike.Core.Combat;

namespace Roguelike.Core.Entities
{
    public class Entity
    {
        public Entity(Level parentLevel)
        {
            this.parentLevel = parentLevel;
            this.EntityType = EntityTypes.NPC;

            this.statsPackage = new StatsPackage(this);
        }

        public virtual void DrawStep(Rectangle viewport)
        {
            int pointX = this.x - GameManager.CameraOffset.X + viewport.X;
            int pointY = this.y - GameManager.CameraOffset.Y + viewport.Y;

            if (pointX >= viewport.Left && pointX < viewport.Right && pointY >= viewport.Top && pointY < viewport.Bottom)
            {
                if (!this.parentLevel.IsOutOfBounds(this.x, this.y) && this.parentLevel.Matrix.TerrainMatrix[this.x, this.y].IsVisible)
                {
                    GraphicConsole.SetColors(this.foregroundColor, this.backgroundColor);
                    GraphicConsole.Put(this.token, pointX, pointY);
                }
            }
        }
        public virtual void UpdateStep()
        {
            this.foregroundColor = this.baseColor;
            this.StatsPackage.UpdateStep();
        }
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void OnInteract(Entity entity)
        {

        }
        public virtual void MoveToTile(int x, int y)
        {
            if (this.parentLevel.CanMoveTo(x, y))
            {
                this.X = x;
                this.Y = y;
            }
            else if (this.parentLevel.IsBlockedByEntity(x, y))
            {
                this.parentLevel.GetEntity(x, y).OnInteract(this);
            }
        }

        public virtual void Attack(Entity attacker, Ability ability)
        {
            CombatManager.PerformAbility(attacker.StatsPackage, this.StatsPackage, ability);
        }
        public virtual void BlendColor(Color color)
        {
            this.foregroundColor.R = (byte)((this.foregroundColor.R + color.R) / 2);
            this.foregroundColor.G = (byte)((this.foregroundColor.G + color.G) / 2);
            this.foregroundColor.B = (byte)((this.foregroundColor.B + color.B) / 2);
        }

        public virtual void OnDeath()
        {
            this.DoPurge = true;
        }
        public virtual void OnSpawn()
        {
            this.statsPackage.OnSpawn();
        }
        public virtual void OnMove()
        {
            this.statsPackage.OnMove();
        }

        #region Variables
        protected int x, y;
        protected char token = 'D';
        protected bool isSolid = false;
        protected Level parentLevel;
        
        private Color foregroundColor = Color.White;
        private Color baseColor = Color.White;
        private Color backgroundColor = Color.Black;
        protected StatsPackage statsPackage;

        public int X { get { return this.x; } set { this.x = value; } }
        public int Y { get { return this.y; } set { this.y = value; } }
        public char Token { get { return this.token; } set { this.token = value; } }
        public bool IsSolid { get { return this.isSolid; } set { this.isSolid = value; } }
        public Level ParentLevel { get { return this.parentLevel; } set { this.parentLevel = value; } }
        public Color ForegroundColor { get { return this.foregroundColor; } set { this.baseColor = value; } }
        public Color BackgroundColor { get { return this.backgroundColor; } set { this.backgroundColor = value; } }
        public virtual StatsPackage StatsPackage { get { return this.statsPackage; } set { this.statsPackage = value; } }
        public EntityTypes EntityType { get; set; }
        public Tile Tile { get { return this.parentLevel.Matrix.TerrainMatrix[this.X, this.Y]; } }
        public bool DoPurge = false;
        #endregion

        public enum EntityTypes { Player, Door, Ladder, NPC, Enemy, Chest, Other }
        public enum Interactions { Hostile, Friendly }
    }
}
