using System;

namespace Roguelike
{
    static class Program
    {
        static MainGame game;
        static void Main(string[] args)
        {
            using (game = new MainGame())
            {
                game.Run();
            }
        }

        public static void Exit()
        {
            game.Exit();
        }

        public static MainGame Game { get { return game; } }
        public static ContentManager Content { get { return game.Content; } }
    }
}

