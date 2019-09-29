using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Roguelike.Engine
{
    public class ContentManager : IDisposable
    {
        private Dictionary<string, Texture2D> textures; // <path, texture>

        public ContentManager()
        {
            textures = new Dictionary<string, Texture2D>();
        }

        // TODO: Implement proper disposing pattern
        public void Dispose()
        {
            foreach (var texture in textures)
            {
                texture.Value.Delete();
            }
        }

        public T Load<T>(params string[] paths)
        {
            foreach (var path in paths)
                checkIfValidPath(path);

            object result = null;
            if (typeof(T) == typeof(Texture2D)) { result = loadTexture(paths[0]); }
            else { throw new NotSupportedException($"Content Type ({typeof(T)}) not supported."); }

            return (T)result;
        }
        public void Unload()
        {
            Dispose();
        }

        public Texture2D CreateTextureFromPtrData(int width, int height, IntPtr dataPtr)
        {
            // Create OpenGL Texture Object
            int textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexStorage2D(TextureTarget2d.Texture2D, 1, SizedInternalFormat.Rgba8, width, height);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, dataPtr);

            // OpenGL wrapping and filter options
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            // Create Texture2D object
            return new Texture2D(textureID, width, height);
        }

        private Texture2D loadTexture(string path)
        {
            // Ensure we haven't already loaded this texture
            Texture2D texture2D;
            if (textures.TryGetValue(path, out texture2D))
                return texture2D;

            using (var bitmap = new Bitmap(path))
            {
                // Load texture information from file
                var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                var dataPtr = data.Scan0;

                texture2D = CreateTextureFromPtrData(bitmap.Width, bitmap.Height, dataPtr);

                bitmap.UnlockBits(data);
            }

            return texture2D;
        }

        private void checkIfValidPath(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Cannot load content file.  File does not exist", Path.GetFileName(path));
            }
        }
    }
}
