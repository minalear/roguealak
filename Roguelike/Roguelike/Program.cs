using System;
using Roguelike.Engine;

namespace Roguelike
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (var game = new MainGame())
            {
                game.Run();
            }
        }
    }
}

