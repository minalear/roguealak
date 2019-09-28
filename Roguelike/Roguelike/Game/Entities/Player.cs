using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Roguelike.Engine.Game.Stats;

namespace Roguelike.Engine.Game.Entities
{
    public class Player : Entity
    {
        private PlayerStats playerStats;
        public PlayerStats PlayerStats { get { return this.playerStats; } set { this.playerStats = value; } }
        public override StatsPackage StatsPackage { get { return this.playerStats; } set { this.playerStats = (PlayerStats)value; } }

        public Player(Level parent)
            : base(parent)
        {
            this.EntityType = EntityTypes.Player;
            this.isSolid = true;
        }

        public override void DrawStep(Rectangle viewport)
        {
            base.DrawStep(viewport);
        }
        public override void UpdateStep()
        {
            //this.addOccupationTags();
            this.clearMapVisibility();
            this.revealSightRadius();
            this.clearEntitiesInSight();

            base.UpdateStep();
            GameManager.Player.StatsPackage = Items.Inventory.CalculateStats(GameManager.Player.StatsPackage);
        }
        public override void Update(GameTime gameTime)
        {
            this.checkPlayerInput(gameTime);

            base.Update(gameTime);
        }
        public override void MoveToTile(int x, int y)
        {
            this.X = x;
            this.y = y;

            this.PlayerStats.OnMove();
            Items.Inventory.OnMove();

            GameManager.Step();
        }
        public void TeleportPlayer(Level level, Point destination)
        {
            this.parentLevel.Entities.Remove(this);

            this.parentLevel = level;
            this.MoveToTile(destination.X, destination.Y);

            GameManager.CurrentLevel = this.parentLevel;
            GameManager.SetCameraOffset();

            this.parentLevel.Entities.Add(this);
        }
        public override void OnDeath()
        {
            GameManager.ChangeGameState(GameStates.Dead);
            base.OnDeath();
        }

        private Point getDestination()
        {
            Point destination = new Point(this.X, this.Y);

            #region Normal Keys
            if (InputManager.KeyWasPressedFor(UP_KEY, MOVEMENT_DELAY) || InputManager.KeyWasPressed(UP_KEY))
            {
                destination.Y--;
                InputManager.ResetTimeHeld(UP_KEY);
            }
            else if (InputManager.KeyWasPressedFor(DOWN_KEY, MOVEMENT_DELAY) || InputManager.KeyWasPressed(DOWN_KEY))
            {
                destination.Y++;
                InputManager.ResetTimeHeld(DOWN_KEY);
            }

            if (InputManager.KeyWasPressedFor(LEFT_KEY, MOVEMENT_DELAY) || InputManager.KeyWasPressed(LEFT_KEY))
            {
                destination.X--;
                InputManager.ResetTimeHeld(LEFT_KEY);
            }
            else if (InputManager.KeyWasPressedFor(RIGHT_KEY, MOVEMENT_DELAY) || InputManager.KeyWasPressed(RIGHT_KEY))
            {
                destination.X++;
                InputManager.ResetTimeHeld(RIGHT_KEY);
            }
            #endregion

            #region Numpad Cardinal
            if (InputManager.KeyWasPressedFor(UP_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(UP_KEY_ALT))
            {
                destination.Y--;
                InputManager.ResetTimeHeld(UP_KEY_ALT);
            }
            else if (InputManager.KeyWasPressedFor(DOWN_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(DOWN_KEY_ALT))
            {
                destination.Y++;
                InputManager.ResetTimeHeld(DOWN_KEY_ALT);
            }

            if (InputManager.KeyWasPressedFor(LEFT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(LEFT_KEY_ALT))
            {
                destination.X--;
                InputManager.ResetTimeHeld(LEFT_KEY_ALT);
            }
            else if (InputManager.KeyWasPressedFor(RIGHT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(RIGHT_KEY_ALT))
            {
                destination.X++;
                InputManager.ResetTimeHeld(RIGHT_KEY_ALT);
            }
            #endregion
            #region Numpad Diagonal
            if (destination == new Point(this.X, this.Y))
            {
                if (InputManager.KeyWasPressedFor(UP_RIGHT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(UP_RIGHT_KEY_ALT))
                {
                    destination.X++;
                    destination.Y--;
                    InputManager.ResetTimeHeld(UP_RIGHT_KEY_ALT);
                }
                else if (InputManager.KeyWasPressedFor(UP_LEFT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(UP_LEFT_KEY_ALT))
                {
                    destination.X--;
                    destination.Y--;
                    InputManager.ResetTimeHeld(UP_LEFT_KEY_ALT);
                }

                if (InputManager.KeyWasPressedFor(DOWN_RIGHT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(DOWN_RIGHT_KEY_ALT))
                {
                    destination.X++;
                    destination.Y++;
                    InputManager.ResetTimeHeld(DOWN_RIGHT_KEY_ALT);
                }
                else if (InputManager.KeyWasPressedFor(DOWN_LEFT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(DOWN_LEFT_KEY_ALT))
                {
                    destination.X--;
                    destination.Y++;
                    InputManager.ResetTimeHeld(DOWN_LEFT_KEY_ALT);
                }
            }
            #endregion

            return destination;
        }
        private void clearMapVisibility()
        {
            for (int y = 0; y < this.parentLevel.Matrix.Height; y++)
            {
                for (int x = 0; x < this.parentLevel.Matrix.Width; x++)
                {
                    this.parentLevel.Matrix.TerrainMatrix[x, y].IsVisible = false;
                }
            }
        }
        private void clearEntitiesInSight()
        {
            for (int y = 0; y < this.ParentLevel.Matrix.Height; y++)
            {
                for (int x = 0; x < this.ParentLevel.Matrix.Width; x++)
                {
                    if (this.ParentLevel.Matrix.TerrainMatrix[x, y].IsVisible)
                    {
                        this.parentLevel.SetToken(MatrixLevels.Entity, x, y, ' ');
                    }
                }
            }
        }
        private void addOccupationTags()
        {
            for (int i = 0; i < this.parentLevel.Entities.Count; i++)
            {
                if (this.parentLevel.Matrix.TerrainMatrix[this.parentLevel.Entities[i].X, this.parentLevel.Entities[i].Y].IsVisible)
                {
                    this.parentLevel.SetToken(MatrixLevels.Entity, this.parentLevel.Entities[i].X, this.parentLevel.Entities[i].Y, this.parentLevel.Entities[i].Token);
                }
            }
        }
        private void revealSightRadius()
        {
            int radius = 10;

            for (int angle = 0; angle <= 360; angle += 1)
            {
                for (int r = 0; r < radius; r++)
                {
                    int x = (int)(this.x + 0.5 + r * Math.Cos(angle));
                    int y = (int)(this.y + 0.5 + r * Math.Sin(angle));

                    this.parentLevel.RevealTile(x, y);
                    if (this.parentLevel.IsTileSolid(x, y) || this.parentLevel.IsBlockedByEntity(x, y))
                    {
                        if (x == this.x && y == this.y)
                            continue;
                        else
                            break;
                    }
                }
            }
        }
        private void checkPlayerInput(GameTime gameTime)
        {
            Point destination = this.getDestination();
            if (destination.X != this.X || destination.Y != this.Y)
            {
                if (this.parentLevel.CanMoveTo(destination.X, destination.Y))
                {
                    int xDif = destination.X - this.X;
                    int yDif = destination.Y - this.Y;

                    GameManager.CameraOffset.X += xDif;
                    GameManager.CameraOffset.Y += yDif;

                    this.MoveToTile(destination.X, destination.Y);
                }

                //Check for entities for interaction
                for (int i = 0; i < parentLevel.Entities.Count; i++)
                {
                    if (parentLevel.Entities[i].X == destination.X && parentLevel.Entities[i].Y == destination.Y && parentLevel.Entities[i] != this)
                    {
                        if (parentLevel.Entities[i].EntityType == EntityTypes.Enemy)
                        {
                            parentLevel.Entities[i].Attack(this, Combat.SpellBook.DefaultAction);
                        }
                        else
                        {
                            parentLevel.Entities[i].OnInteract(this);
                        }

                        GameManager.Step();
                    }
                }
            }

            if (InputManager.KeyWasPressedFor(WAIT_KEY, MOVEMENT_DELAY) || InputManager.KeyWasPressed(WAIT_KEY))
            {
                GameManager.Step();
                InputManager.ResetTimeHeld(WAIT_KEY);
            }
        }

        private const Keys UP_KEY = Keys.W;
        private const Keys DOWN_KEY = Keys.S;
        private const Keys LEFT_KEY = Keys.A;
        private const Keys RIGHT_KEY = Keys.D;

        private const Keys UP_KEY_ALT = Keys.NumPad8;
        private const Keys DOWN_KEY_ALT = Keys.NumPad2;
        private const Keys LEFT_KEY_ALT = Keys.NumPad4;
        private const Keys RIGHT_KEY_ALT = Keys.NumPad6;

        private const Keys UP_RIGHT_KEY_ALT = Keys.NumPad9;
        private const Keys UP_LEFT_KEY_ALT = Keys.NumPad7;
        private const Keys DOWN_RIGHT_KEY_ALT = Keys.NumPad3;
        private const Keys DOWN_LEFT_KEY_ALT = Keys.NumPad1;

        private const Keys WAIT_KEY = Keys.NumPad5;

        private static TimeSpan MOVEMENT_DELAY = new TimeSpan(0, 0, 0, 0, 150);
    }
}
