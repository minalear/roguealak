using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roguelike.Engine.UI;
using Roguelike.Engine.UI.Controls;
using Roguelike.Engine.Game;

namespace Roguelike.Engine.UI.Interfaces
{
    public class MapInterface : Interface
    {
        Title title;
        Button backButton;

        private Point offset;

        public MapInterface()
        {
            this.title = new Title(this, "Map", GraphicConsole.BufferWidth / 2, 1, Title.TextAlignModes.Center);
            this.backButton = new Button(this, "X", GraphicConsole.BufferWidth - 2, 1, 1, 1) { KeyShortcut = Keys.Escape };
            this.backButton.Click += backButton_Pressed;
        }

        void backButton_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.Game);
        }

        public override void OnCall()
        {
            offset = new Point(GameManager.CameraOffset.X, GameManager.CameraOffset.Y);
            this.title.Text = GameManager.CurrentDungeon.DungeonName;

            base.OnCall();
        }

        public override void DrawStep()
        {
            GameManager.DrawGameWorld(offset);
            this.drawInterfaceBars();

            base.DrawStep();
        }

        public override void Update(GameTime gameTime)
        {
            getKeyboardInput();
            
            base.Update(gameTime);
        }

        private void drawInterfaceBars()
        {
            GraphicConsole.ResetColor();

            //Header Bar
            DrawingUtilities.DrawLine(1, 1, GraphicConsole.BufferWidth - 2, 1, ' ');
            DrawingUtilities.DrawLine(0, 0, GraphicConsole.BufferWidth, 0, '═');
            DrawingUtilities.DrawLine(0, 2, GraphicConsole.BufferWidth, 2, '═');

            //Left Bar
            DrawingUtilities.DrawLine(0, 1, 0, GraphicConsole.BufferHeight - 2, '│');

            //Right Bar
            DrawingUtilities.DrawLine(GraphicConsole.BufferWidth, 1, GraphicConsole.BufferWidth, GraphicConsole.BufferHeight - 2, '│');

            //Bottom Bar
            DrawingUtilities.DrawLine(1, GraphicConsole.BufferHeight - 2, GraphicConsole.BufferWidth - 2, GraphicConsole.BufferHeight - 2, ' ');
            DrawingUtilities.DrawLine(0, GraphicConsole.BufferHeight, GraphicConsole.BufferWidth, GraphicConsole.BufferHeight, '─');
            DrawingUtilities.DrawLine(0, GraphicConsole.BufferHeight - 3, GraphicConsole.BufferWidth, GraphicConsole.BufferHeight - 3, '─');

            //Bottom Left Corner
            GraphicConsole.Put('├', 0, GraphicConsole.BufferHeight - 3);
            GraphicConsole.Put('└', 0, GraphicConsole.BufferHeight);

            //Bottom Right Corner
            GraphicConsole.Put('┤', GraphicConsole.BufferWidth, GraphicConsole.BufferHeight - 3);
            GraphicConsole.Put('┘', GraphicConsole.BufferWidth, GraphicConsole.BufferHeight);

            //Top Left Corner
            GraphicConsole.Put('╒', 0, 0);
            GraphicConsole.Put('╞', 0, 2);

            //Top Right Corner
            GraphicConsole.Put('╕', GraphicConsole.BufferWidth, 0);
            GraphicConsole.Put('╡', GraphicConsole.BufferWidth, 2);
        }

        private void getKeyboardInput()
        {
            Point oldOffset = new Point(offset.X, offset.Y);

            #region Normal Keys
            if (InputManager.KeyWasPressedFor(UP_KEY, MOVEMENT_DELAY) || InputManager.KeyWasPressed(UP_KEY))
            {
                offset.Y--;
                InputManager.ResetTimeHeld(UP_KEY);
            }
            else if (InputManager.KeyWasPressedFor(DOWN_KEY, MOVEMENT_DELAY) || InputManager.KeyWasPressed(DOWN_KEY))
            {
                offset.Y++;
                InputManager.ResetTimeHeld(DOWN_KEY);
            }

            if (InputManager.KeyWasPressedFor(LEFT_KEY, MOVEMENT_DELAY) || InputManager.KeyWasPressed(LEFT_KEY))
            {
                offset.X--;
                InputManager.ResetTimeHeld(LEFT_KEY);
            }
            else if (InputManager.KeyWasPressedFor(RIGHT_KEY, MOVEMENT_DELAY) || InputManager.KeyWasPressed(RIGHT_KEY))
            {
                offset.X++;
                InputManager.ResetTimeHeld(RIGHT_KEY);
            }
            #endregion

            #region Numpad Cardinal
            if (InputManager.KeyWasPressedFor(UP_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(UP_KEY_ALT))
            {
                offset.Y--;
                InputManager.ResetTimeHeld(UP_KEY_ALT);
            }
            else if (InputManager.KeyWasPressedFor(DOWN_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(DOWN_KEY_ALT))
            {
                offset.Y++;
                InputManager.ResetTimeHeld(DOWN_KEY_ALT);
            }

            if (InputManager.KeyWasPressedFor(LEFT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(LEFT_KEY_ALT))
            {
                offset.X--;
                InputManager.ResetTimeHeld(LEFT_KEY_ALT);
            }
            else if (InputManager.KeyWasPressedFor(RIGHT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(RIGHT_KEY_ALT))
            {
                offset.X++;
                InputManager.ResetTimeHeld(RIGHT_KEY_ALT);
            }
            #endregion
            #region Numpad Diagonal
            if (InputManager.KeyWasPressedFor(UP_RIGHT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(UP_RIGHT_KEY_ALT))
            {
                offset.X++;
                offset.Y--;
                InputManager.ResetTimeHeld(UP_RIGHT_KEY_ALT);
            }
            else if (InputManager.KeyWasPressedFor(UP_LEFT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(UP_LEFT_KEY_ALT))
            {
                offset.X--;
                offset.Y--;
                InputManager.ResetTimeHeld(UP_LEFT_KEY_ALT);
            }

            if (InputManager.KeyWasPressedFor(DOWN_RIGHT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(DOWN_RIGHT_KEY_ALT))
            {
                offset.X++;
                offset.Y++;
                InputManager.ResetTimeHeld(DOWN_RIGHT_KEY_ALT);
            }
            else if (InputManager.KeyWasPressedFor(DOWN_LEFT_KEY_ALT, MOVEMENT_DELAY) || InputManager.KeyWasPressed(DOWN_LEFT_KEY_ALT))
            {
                offset.X--;
                offset.Y++;
                InputManager.ResetTimeHeld(DOWN_LEFT_KEY_ALT);
            }
            #endregion

            if (offset != oldOffset)
            {
                this.UpdateStep();
                this.DrawStep();
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

        private static TimeSpan MOVEMENT_DELAY = new TimeSpan(0, 0, 0, 0, 50);
    }
}
