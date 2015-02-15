using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Roguelike.Engine.Game;
using Roguelike.Engine.UI.Interfaces;

namespace Roguelike.Engine.UI
{
    public static class InterfaceManager
    {
        private static Interface activeInterface;
        private static Dictionary<GameStates, Interface> interfaces;

        public static void Initialize()
        {
            interfaces = new Dictionary<GameStates, Interface>();

            interfaces.Add(GameStates.Game, new GameInterface());
            interfaces.Add(GameStates.MainMenu, new MainMenuInterface());
            interfaces.Add(GameStates.InterfaceTest, new InterfaceTest());
            interfaces.Add(GameStates.CharacterCreation, new CharacterCreation());
            interfaces.Add(GameStates.Dead, new DeadInterface());
            interfaces.Add(GameStates.Map, new MapInterface());
            interfaces.Add(GameStates.Options, new OptionsInterface());
            interfaces.Add(GameStates.ItemTesting, new ItemTestingInterface());
            interfaces.Add(GameStates.About, new AboutInterface());
            interfaces.Add(GameStates.Win, new WinInterface());
        }

        public static void SwitchInterface(GameStates gameState)
        {
            GraphicConsole.Clear();

            activeInterface = interfaces[gameState];
            activeInterface.OnCall();
        }

        public static void DrawStep()
        {
            GraphicConsole.Clear();
            activeInterface.DrawStep();
        }

        public static void UpdateStep()
        {
            activeInterface.UpdateStep();
        }

        public static void Update(GameTime gameTime)
        {
            activeInterface.Update(gameTime);
        }
    }
}
