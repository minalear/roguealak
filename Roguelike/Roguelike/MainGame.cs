using System;
using OpenTK;
using Roguelike.Engine;
using Roguelike.Core;
using Roguelike.Engine.UI;
using Roguelike.Engine.Console;

namespace Roguelike
{
    public class MainGame : Engine.Game
    {
        public MainGame()
        {
            Content = new ContentManager();

            GraphicConsole.Instance = new GraphicConsole(this, 125, 50);

            //GraphicConsole.Instance.Scale = Vector2.One;
            //GraphicConsole.Instance.DisplayCursor = false;
        }

        public override void Initialize()
        {
            Engine.Factories.NameGenerator.Initialize();
            CombatManager.Initialize();
            InputManager.Initialize(this);
            InterfaceManager.Initialize();

            base.Initialize();
        }

        public override void LoadContent()
        {
            GameManager.Initialize();

            GameManager.UpdateStep();
            GameManager.DrawStep();
        }

        public override void UnloadContent()
        {

        }

        public override void UpdateFrame(GameTime gameTime)
        {
            GraphicConsole.Instance.UpdateFrame(gameTime);
            InterfaceManager.Update(gameTime);
            InputManager.Update(gameTime);

            base.UpdateFrame(gameTime);
        }

        public override void RenderFrame(GameTime gameTime)
        {
            GraphicConsole.Instance.RenderFrame();

            base.RenderFrame(gameTime);
        }
    }
}
