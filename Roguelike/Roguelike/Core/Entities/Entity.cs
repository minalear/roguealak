using System;
using OpenTK;
using OpenTK.Graphics;
using Roguelike.Core.Stats;
using Roguelike.Core.Combat;
using Roguelike.Engine;

namespace Roguelike.Core.Entities
{
    public class Entity
    {
        public Entity(Level parentLevel)
        {
            this.parentLevel = parentLevel;
            EntityType = EntityTypes.NPC;

            statsPackage = new StatsPackage(this);
        }

        public virtual void DrawStep(Box2 viewport)
        {
            int pointX = x - GameManager.CameraOffset.X + (int)viewport.Left;
            int pointY = y - GameManager.CameraOffset.Y + (int)viewport.Top;

            if (pointX >= viewport.Left && pointX < viewport.Right && pointY >= viewport.Top && pointY < viewport.Bottom)
            {
                if (!parentLevel.IsOutOfBounds(x, y) && parentLevel.Matrix.TerrainMatrix[x, y].IsVisible)
                {
                    GraphicConsole.SetColors(foregroundColor, backgroundColor);
                    GraphicConsole.Put(token, pointX, pointY);
                }
            }
        }
        public virtual void UpdateStep()
        {
            foregroundColor = baseColor;
            StatsPackage.UpdateStep();
        }
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void OnInteract(Entity entity)
        {

        }
        public virtual void MoveToTile(int x, int y)
        {
            if (parentLevel.CanMoveTo(x, y))
            {
                X = x;
                Y = y;
            }
            else if (parentLevel.IsBlockedByEntity(x, y))
            {
                parentLevel.GetEntity(x, y).OnInteract(this);
            }
        }

        public virtual void Attack(Entity attacker, Ability ability)
        {
            CombatManager.PerformAbility(attacker.StatsPackage, StatsPackage, ability);
        }
        public virtual void BlendColor(Color4 color)
        {
            foregroundColor.R = (byte)((foregroundColor.R + color.R) / 2);
            foregroundColor.G = (byte)((foregroundColor.G + color.G) / 2);
            foregroundColor.B = (byte)((foregroundColor.B + color.B) / 2);
        }

        public virtual void OnDeath()
        {
            DoPurge = true;
        }
        public virtual void OnSpawn()
        {
            statsPackage.OnSpawn();
        }
        public virtual void OnMove()
        {
            statsPackage.OnMove();
        }

        #region Variables
        protected int x, y;
        protected char token = 'D';
        protected bool isSolid = false;
        protected Level parentLevel;
        
        private Color4 foregroundColor = Color4.White;
        private Color4 baseColor = Color4.White;
        private Color4 backgroundColor = Color4.Black;
        protected StatsPackage statsPackage;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public char Token { get { return token; } set { token = value; } }
        public bool IsSolid { get { return isSolid; } set { isSolid = value; } }
        public Level ParentLevel { get { return parentLevel; } set { parentLevel = value; } }
        public Color4 ForegroundColor { get { return foregroundColor; } set { baseColor = value; } }
        public Color4 BackgroundColor { get { return backgroundColor; } set { backgroundColor = value; } }
        public virtual StatsPackage StatsPackage { get { return statsPackage; } set { statsPackage = value; } }
        public EntityTypes EntityType { get; set; }
        public Tile Tile { get { return parentLevel.Matrix.TerrainMatrix[X, Y]; } }
        public bool DoPurge = false;
        #endregion

        public enum EntityTypes { Player, Door, Ladder, NPC, Enemy, Chest, Other }
        public enum Interactions { Hostile, Friendly }
    }
}
