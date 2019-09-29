using System;
using OpenTK;
using OpenTK.Graphics;

namespace Roguelike.Engine.Console
{
    public struct CharToken
    {
        public char Token;
        public Vector2 TextureCoords;
        public int X, Y;
        public Color4 ForegroundColor;
        public Color4 BackgroundColor;
    }
}
