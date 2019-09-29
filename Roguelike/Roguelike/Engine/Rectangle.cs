using System;

namespace Roguelike.Engine
{
    public struct Rectangle
    {
        public int X, Y, Width, Height;

        public int Top { get { return Y; } }
        public int Left { get { return X; } }
        public int Bottom { get { return Y + Height; } }
        public int Right { get { return X + Width; } }

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool Contains(Point point)
        {
            return (point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom);
        }
    }
}
