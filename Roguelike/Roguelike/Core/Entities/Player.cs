using System;
using OpenTK;
using OpenTK.Input;
using Roguelike.Core.Stats;
using Roguelike.Engine;

namespace Roguelike.Core.Entities
{
    public class Player : Entity
    {
        private PlayerStats playerStats;
        public PlayerStats PlayerStats { get { return playerStats; } set { playerStats = value; } }
        public override StatsPackage StatsPackage { get { return playerStats; } set { playerStats = (PlayerStats)value; } }

        public Player(Level parent)
            : base(parent)
        {
            EntityType = EntityTypes.Player;
            isSolid = true;
        }

        public override void DrawStep(Rectangle viewport)
        {
            base.DrawStep(viewport);
        }
        public override void UpdateStep()
        {
            //addOccupationTags();
            clearMapVisibility();
            revealSightRadius();
            clearEntitiesInSight();

            base.UpdateStep();
            GameManager.Player.StatsPackage = Items.Inventory.CalculateStats(GameManager.Player.StatsPackage);
        }
        public override void Update(GameTime gameTime)
        {
            checkPlayerInput(gameTime);

            base.Update(gameTime);
        }
        public override void MoveToTile(int x, int y)
        {
            X = x;
            Y = y;

            PlayerStats.OnMove();
            Items.Inventory.OnMove();

            GameManager.Step();
        }
        public void TeleportPlayer(Level level, Point destination)
        {
            parentLevel.Entities.Remove(this);

            parentLevel = level;
            MoveToTile(destination.X, destination.Y);

            GameManager.CurrentLevel = parentLevel;
            GameManager.SetCameraOffset();

            parentLevel.Entities.Add(this);
        }
        public override void OnDeath()
        {
            GameManager.ChangeGameState(GameStates.Dead);
            base.OnDeath();
        }

        private Point getDestination()
        {
            Point destination = new Point(X, Y);

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
            if (destination == new Point(X, Y))
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
            for (int y = 0; y < parentLevel.Matrix.Height; y++)
            {
                for (int x = 0; x < parentLevel.Matrix.Width; x++)
                {
                    parentLevel.Matrix.TerrainMatrix[x, y].IsVisible = false;
                }
            }
        }
        private void clearEntitiesInSight()
        {
            for (int y = 0; y < ParentLevel.Matrix.Height; y++)
            {
                for (int x = 0; x < ParentLevel.Matrix.Width; x++)
                {
                    if (ParentLevel.Matrix.TerrainMatrix[x, y].IsVisible)
                    {
                        parentLevel.SetToken(MatrixLevels.Entity, x, y, ' ');
                    }
                }
            }
        }
        private void addOccupationTags()
        {
            for (int i = 0; i < parentLevel.Entities.Count; i++)
            {
                if (parentLevel.Matrix.TerrainMatrix[parentLevel.Entities[i].X, parentLevel.Entities[i].Y].IsVisible)
                {
                    parentLevel.SetToken(MatrixLevels.Entity, parentLevel.Entities[i].X, parentLevel.Entities[i].Y, parentLevel.Entities[i].Token);
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

                    parentLevel.RevealTile(x, y);
                    if (parentLevel.IsTileSolid(x, y) || parentLevel.IsBlockedByEntity(x, y))
                    {
                        if (this.x == x && this.y == y)
                            continue;
                        else
                            break;
                    }
                }
            }
        }
        private void checkPlayerInput(GameTime gameTime)
        {
            Point destination = getDestination();
            if (destination.X != X || destination.Y != Y)
            {
                if (parentLevel.CanMoveTo(destination.X, destination.Y))
                {
                    int xDif = destination.X - X;
                    int yDif = destination.Y - Y;

                    GameManager.CameraOffset.X += xDif;
                    GameManager.CameraOffset.Y += yDif;

                    MoveToTile(destination.X, destination.Y);
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

        private const Key UP_KEY = Key.W;
        private const Key DOWN_KEY = Key.S;
        private const Key LEFT_KEY = Key.A;
        private const Key RIGHT_KEY = Key.D;

        private const Key UP_KEY_ALT = Key.Keypad8;
        private const Key DOWN_KEY_ALT = Key.Keypad2;
        private const Key LEFT_KEY_ALT = Key.Keypad4;
        private const Key RIGHT_KEY_ALT = Key.Keypad6;

        private const Key UP_RIGHT_KEY_ALT = Key.Keypad9;
        private const Key UP_LEFT_KEY_ALT = Key.Keypad7;
        private const Key DOWN_RIGHT_KEY_ALT = Key.Keypad3;
        private const Key DOWN_LEFT_KEY_ALT = Key.Keypad1;

        private const Key WAIT_KEY = Key.Number5;

        private static TimeSpan MOVEMENT_DELAY = new TimeSpan(0, 0, 0, 0, 150);
    }
}
