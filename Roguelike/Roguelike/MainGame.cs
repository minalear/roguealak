using System;
using Roguelike.Engine;
using Roguelike.Core;
using Roguelike.Engine.UI;

namespace Roguelike
{
    public class MainGame : Engine.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            GraphicConsole.Initialize(graphics, Window);

            GraphicConsole.BufferWidth = 125;
            GraphicConsole.BufferHeight = 50;

            GraphicConsole.Scale = Vector2.One;
            GraphicConsole.DisplayCursor = false;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Engine.Factories.NameGenerator.Initialize();
            CombatManager.Initialize();
            InputManager.Initialize();
            InterfaceManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicConsole.LoadSpriteBatch(spriteBatch, Content);
            GameManager.Initialize();

            GameManager.UpdateStep();
            GameManager.DrawStep();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            GraphicConsole.Update(gameTime);
            InterfaceManager.Update(gameTime);
            InputManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicConsole.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
