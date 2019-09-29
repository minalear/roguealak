using System;
using OpenTK.Input;
using Roguelike.Engine.Console;
using Roguelike.Engine.UI.Controls;
using Roguelike.Core;

namespace Roguelike.Engine.UI.Interfaces
{
    public class MapInterface : Interface
    {
        Title title;
        Button backButton;

        private Point offset;

        public MapInterface()
        {
            title = new Title(this, "Map", GraphicConsole.Instance.BufferWidth / 2, 1, Title.TextAlignModes.Center);
            backButton = new Button(this, "X", GraphicConsole.Instance.BufferWidth - 2, 1, 1, 1) { KeyShortcut = Key.Escape };
            backButton.Click += backButton_Pressed;
        }

        void backButton_Pressed(object sender, MouseButtons button)
        {
            GameManager.ChangeGameState(GameStates.Game);
        }

        public override void OnCall()
        {
            offset = new Point(GameManager.CameraOffset.X, GameManager.CameraOffset.Y);
            title.Text = GameManager.CurrentDungeon.DungeonName;

            base.OnCall();
        }

        public override void DrawStep()
        {
            GameManager.DrawGameWorld(offset);
            drawInterfaceBars();

            base.DrawStep();
        }

        public override void Update(GameTime gameTime)
        {
            getKeyboardInput();
            
            base.Update(gameTime);
        }

        private void drawInterfaceBars()
        {
            GraphicConsole.Instance.ClearColor();

            //Header Bar
            DrawingUtilities.DrawLine(1, 1, GraphicConsole.Instance.BufferWidth - 2, 1, ' ');
            DrawingUtilities.DrawLine(0, 0, GraphicConsole.Instance.BufferWidth, 0, '═');
            DrawingUtilities.DrawLine(0, 2, GraphicConsole.Instance.BufferWidth, 2, '═');

            //Left Bar
            DrawingUtilities.DrawLine(0, 1, 0, GraphicConsole.Instance.BufferHeight - 2, '│');

            //Right Bar
            DrawingUtilities.DrawLine(GraphicConsole.Instance.BufferWidth, 1, GraphicConsole.Instance.BufferWidth, GraphicConsole.Instance.BufferHeight - 2, '│');

            //Bottom Bar
            DrawingUtilities.DrawLine(1, GraphicConsole.Instance.BufferHeight - 2, GraphicConsole.Instance.BufferWidth - 2, GraphicConsole.Instance.BufferHeight - 2, ' ');
            DrawingUtilities.DrawLine(0, GraphicConsole.Instance.BufferHeight, GraphicConsole.Instance.BufferWidth, GraphicConsole.Instance.BufferHeight, '─');
            DrawingUtilities.DrawLine(0, GraphicConsole.Instance.BufferHeight - 3, GraphicConsole.Instance.BufferWidth, GraphicConsole.Instance.BufferHeight - 3, '─');

            //Bottom Left Corner
            GraphicConsole.Instance.Put('├', 0, GraphicConsole.Instance.BufferHeight - 3);
            GraphicConsole.Instance.Put('└', 0, GraphicConsole.Instance.BufferHeight);

            //Bottom Right Corner
            GraphicConsole.Instance.Put('┤', GraphicConsole.Instance.BufferWidth, GraphicConsole.Instance.BufferHeight - 3);
            GraphicConsole.Instance.Put('┘', GraphicConsole.Instance.BufferWidth, GraphicConsole.Instance.BufferHeight);

            //Top Left Corner
            GraphicConsole.Instance.Put('╒', 0, 0);
            GraphicConsole.Instance.Put('╞', 0, 2);

            //Top Right Corner
            GraphicConsole.Instance.Put('╕', GraphicConsole.Instance.BufferWidth, 0);
            GraphicConsole.Instance.Put('╡', GraphicConsole.Instance.BufferWidth, 2);
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
                UpdateStep();
                DrawStep();
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

        private static TimeSpan MOVEMENT_DELAY = new TimeSpan(0, 0, 0, 0, 50);
    }
}
