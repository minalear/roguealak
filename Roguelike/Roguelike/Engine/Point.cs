﻿using System;

namespace Roguelike.Engine
{
    public struct Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Point))
                return (X == ((Point)obj).X && Y == ((Point)obj).Y);
            return false;
        }
        public override int GetHashCode()
        {
            return (X + Y).GetHashCode();
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Point a, Point b)
        {
            return !a.Equals(b);
        }

        public static Point Zero = new Point(0, 0);

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
