using System;
using OpenTK.Graphics.OpenGL4;

namespace Roguelike.Engine
{
    public sealed class Texture2D
    {
        public Texture2D(int id, int width, int height)
        {
            ID = id;
            Width = width;
            Height = height;
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, ID);
        }
        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public void Delete()
        {
            GL.DeleteTexture(ID);
        }

        public int ID { get; }
        public int Width { get; }
        public int Height { get; }
    }
}
