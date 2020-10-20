using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Roguelike.Engine;
using Roguelike.Core;
using Roguelike.Engine.UI;
using Roguelike.Engine.Console;

namespace Roguelike
{
    public class MainGame : Engine.Game
    {
        public MainGame() : base(800, 450)
        {

        }

        public override void Initialize()
        {
            Content = new ContentManager();
            GraphicConsole.Instance = new GraphicConsole(this, 125, 50);

            Engine.Factories.NameGenerator.Initialize();
            CombatManager.Initialize();
            InputManager.Initialize(this);
            InterfaceManager.Initialize();
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
        }

        public override void RenderFrame(GameTime gameTime)
        {
            GraphicConsole.Instance.RenderFrame();
        }
    }
}
