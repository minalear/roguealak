using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Engine.Game;
using Microsoft.Xna.Framework;
using DeenGames.Utils.AStarPathFinder;

namespace Roguelike.Engine.Pathing
{
    public static class PathCalculator
    {
        private static List<Point> optimizedPath;
        private static bool isGridInitialized = false;

        private static int width, height;
        private static byte[,] grid;

        private static bool doCacheLevel = false;
        private static Level levelToCache;

        public static List<Point> CalculatePath(Point start, Point destination, Level level)
        {
            if (!isGridInitialized)
                CacheLevel(level);

            optimizedPath = new List<Point>();

            List<PathFinderNode> path = new PathFinderFast(grid).FindPath(new DeenGames.Utils.Point(start.X, start.Y), new DeenGames.Utils.Point(destination.X, destination.Y));

            if (path != null)
                buildPath(path);

            return optimizedPath;
        }
        public static void UpdateStep()
        {
            if (doCacheLevel)
                cacheLevel();
        }
        public static void CacheLevel(Level level)
        {
            levelToCache = level;
            doCacheLevel = true;
        }

        private static void cacheLevel()
        {
            width = levelToCache.Matrix.Width; height = levelToCache.Matrix.Height;
            grid = new byte[levelToCache.Matrix.Width, levelToCache.Matrix.Height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (levelToCache.IsTileSolid(x, y))
                        grid[x, y] = PathFinderHelper.BLOCKED_TILE;
                    else
                        grid[x, y] = PathFinderHelper.EMPTY_TILE;
                }
            }

            isGridInitialized = true;
            doCacheLevel = false;
        }
        private static void buildPath(List<PathFinderNode> path)
        {
            for (int i = path.Count - 1; i >= 0; i--)
            {
                optimizedPath.Add(new Point(path[i].X, path[i].Y));
            }
        }
    }
}
