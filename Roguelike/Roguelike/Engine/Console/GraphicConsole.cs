using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Roguelike.Engine.Console
{
    public class GraphicConsole
    {
        public static GraphicConsole Instance;

        private Game game;
        private Shader consoleShader;
        private Charset charset;

        private int left, top, cursorLeft, cursorTop;
        private int bufferWidth, bufferHeight;

        private CharToken[,] characterMatrix;
        private float[] vertices;
        private int vao, vbo;

        private Color4 foregroundColor, backgroundColor;

        public const int CHAR_WIDTH = 8;
        public const int CHAR_HEIGHT = 12;

        public GraphicConsole(Game game) : this(game, 80, 30) { }
        public GraphicConsole(Game game, int bWidth, int bHeight)
        {
            this.game = game;
            left = top = 0;

            bufferWidth = bWidth;
            bufferHeight = bHeight;

            foregroundColor = Color4.White;
            backgroundColor = Color4.Black;

            charset = new Charset(game.Content, CHAR_WIDTH, CHAR_HEIGHT);

            initShader();

            characterMatrix = new CharToken[bufferWidth, bufferHeight];
            for (int y = 0; y < bufferHeight; y++)
            {
                for (int x = 0; x < bufferWidth; x++)
                {
                    characterMatrix[x, y] = new CharToken()
                    {
                        X = x,
                        Y = y,
                        Token = ' ',
                        TextureCoords = new Vector2(0, 0),
                        ForegroundColor = Color4.White,
                        BackgroundColor = Color4.Black
                    };
                }
            }

            setVertexBuffer();
            game.Window.MouseMove += Window_MouseMove;

            // Initialize OpenGL drawing settings
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, charset.TextureID);
        }

        public void Write(string text)
        {
            foreach (var ch in text)
                Write(ch);
        }
        public void Write(char ch)
        {
            if (ch == '\n' || ch == '\r')
            {
                Top++;
                Left = 0;
            }
            else if (ch == '\t')
            {
                Write("    ");
            }
            else
            {
                Put(ch, Left, top);
                Left++;
            }
        }
        public void Write(object obj)
        {
            Write(obj.ToString());
        }
        public void WriteLine(string text)
        {
            Write(text + "\n");
        }
        public void WriteLine(object obj)
        {
            Write(obj.ToString() + "\n");
        }

        public void Put(char token, int x, int y)
        {
            if (x >= 0 && x < bufferWidth && y >= 0 && y < bufferHeight)
                Put(token, x, y);
        }

        private void put(char token, int x, int y)
        {
            characterMatrix[x, y].Token = token;
            characterMatrix[x, y].TextureCoords = charset.CalculateTextureCoords(charset.GetID(token));
            characterMatrix[x, y].ForegroundColor = foregroundColor;
            characterMatrix[x, y].BackgroundColor = backgroundColor;
        }

        public void SetColors(Color4 foreground, Color4 background)
        {
            foregroundColor = foreground;
            backgroundColor = background;
        }
        public void SetCursor(Point point)
        {
            SetCursor(point.X, point.Y);
        }
        public void SetCursor(int left, int top)
        {
            Left = left;
            Top = top;
        }
        public void Clear()
        {
            ClearColor();

            for (int y = 0; y < bufferHeight; y++)
            {
                for (int x = 0; x < bufferWidth; x++)
                {
                    characterMatrix[x, y].Token = ' ';
                    characterMatrix[x, y].TextureCoords = new Vector2(0.0f, 0.0f);
                    characterMatrix[x, y].ForegroundColor = Color4.White;
                    characterMatrix[x, y].BackgroundColor = Color4.Black;
                }
            }
        }
        public void ClearColor()
        {
            foregroundColor = Color4.White;
            backgroundColor = Color4.Black;
        }

        public void RenderFrame()
        {
            consoleShader.Use();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, charset.TextureID);

            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Quads, 0, 4 * bufferWidth * bufferHeight);
            GL.BindVertexArray(0);

            //GL.Flush();
            updateVertexBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StreamDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void UpdateFrame(GameTime gameTime) { }

        public Tuple<Color4, Color4> GetColorsAtTile(int x, int y)
        {
            if (x >= 0 && x < bufferWidth && y >= 0 && y < bufferHeight)
                return new Tuple<Color4, Color4>(characterMatrix[x, y].ForegroundColor, characterMatrix[x, y].BackgroundColor);
            return new Tuple<Color4, Color4>(Color4.White, Color4.Black);
        }
        public Point GetTilePosition(Point position)
        {
            return new Point((int)(Math.Round(position.X / (float)CHAR_WIDTH)), (int)(Math.Round(position.Y / (float)CHAR_HEIGHT)));
        }
        public Point GetTilePosition(int x, int y)
        {
            return new Point(x / CHAR_WIDTH, y / CHAR_HEIGHT);
        }

        private void Window_MouseMove(object sender, MouseMoveEventArgs e)
        {
            cursorLeft = e.Position.X / charset.CharWidth;
            cursorTop = e.Position.Y / charset.CharHeight;
        }
        private void initShader()
        {
            var vertexSource = game.Content.StreamShaderSource("Dauntless.Resources.vertexShader.glsl");
            var fragmentSource = game.Content.StreamShaderSource("Dauntless.Resources.fragmentShader.glsl");

            consoleShader = new Shader(vertexSource, fragmentSource);
            consoleShader.Use();

            var orthoProj = Matrix4.CreateOrthographicOffCenter(0.0f, game.Window.Width, game.Window.Height, 0.0f, -1.0f, 1.0f);
            consoleShader.SetInteger("font", 0); //Set sampler2D to 0
            consoleShader.SetMatrix4("proj", orthoProj);

            var model = Matrix4.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f));
            consoleShader.SetMatrix4("model", model);
        }
        private void setVertexBuffer()
        {
            vertices = new float[40 * bufferWidth * bufferHeight];
            System.Console.WriteLine(vertices.Length * sizeof(float));
            updateVertexBuffer();

            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StreamDraw);

            consoleShader.Use();
            GL.BindVertexArray(this.vao);

            int posAttrib = GL.GetAttribLocation(consoleShader.ID, "position");
            GL.EnableVertexAttribArray(posAttrib);
            GL.VertexAttribPointer(posAttrib, 2, VertexAttribPointerType.Float, false, 10 * sizeof(float), 0);

            int texAttrib = GL.GetAttribLocation(consoleShader.ID, "texcoords");
            GL.EnableVertexAttribArray(texAttrib);
            GL.VertexAttribPointer(texAttrib, 2, VertexAttribPointerType.Float, false, 10 * sizeof(float), 2 * sizeof(float));

            int foreAttrib = GL.GetAttribLocation(consoleShader.ID, "foreColor");
            GL.EnableVertexAttribArray(foreAttrib);
            GL.VertexAttribPointer(foreAttrib, 3, VertexAttribPointerType.Float, false, 10 * sizeof(float), 4 * sizeof(float));

            int backAttrib = GL.GetAttribLocation(consoleShader.ID, "backColor");
            GL.EnableVertexAttribArray(backAttrib);
            GL.VertexAttribPointer(backAttrib, 3, VertexAttribPointerType.Float, false, 10 * sizeof(float), 7 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }
        private void updateVertexBuffer()
        {
            float tw = charset.CharWidth / 128.0f;
            float th = charset.CharHeight / 192.0f;

            int i = 0;
            for (int y = 0; y < bufferHeight; y++)
            {
                for (int x = 0; x < bufferWidth; x++, i++)
                {
                    #region Vertex Info
                    //Coord 0
                    vertices[0 + i * 40] = x * charset.CharWidth; //0
                    vertices[1 + i * 40] = y * charset.CharHeight; //0

                    vertices[2 + i * 40] = characterMatrix[x, y].TextureCoords.X;
                    vertices[3 + i * 40] = characterMatrix[x, y].TextureCoords.Y;

                    //Foreground Color
                    vertices[4 + i * 40] = characterMatrix[x, y].ForegroundColor.R / 255.0f;
                    vertices[5 + i * 40] = characterMatrix[x, y].ForegroundColor.G / 255.0f;
                    vertices[6 + i * 40] = characterMatrix[x, y].ForegroundColor.B / 255.0f;

                    //Background Color
                    vertices[7 + i * 40] = characterMatrix[x, y].BackgroundColor.R / 255.0f;
                    vertices[8 + i * 40] = characterMatrix[x, y].BackgroundColor.G / 255.0f;
                    vertices[9 + i * 40] = characterMatrix[x, y].BackgroundColor.B / 255.0f;


                    //Coord 1
                    vertices[10 + i * 40] = x * charset.CharWidth; //0
                    vertices[11 + i * 40] = y * charset.CharHeight + charset.CharHeight; //1

                    vertices[12 + i * 40] = characterMatrix[x, y].TextureCoords.X;
                    vertices[13 + i * 40] = characterMatrix[x, y].TextureCoords.Y + th;

                    //Foreground Color
                    vertices[14 + i * 40] = characterMatrix[x, y].ForegroundColor.R / 255.0f;
                    vertices[15 + i * 40] = characterMatrix[x, y].ForegroundColor.G / 255.0f;
                    vertices[16 + i * 40] = characterMatrix[x, y].ForegroundColor.B / 255.0f;

                    //Background Color
                    vertices[17 + i * 40] = characterMatrix[x, y].BackgroundColor.R / 255.0f;
                    vertices[18 + i * 40] = characterMatrix[x, y].BackgroundColor.G / 255.0f;
                    vertices[19 + i * 40] = characterMatrix[x, y].BackgroundColor.B / 255.0f;


                    //Coord 2
                    vertices[20 + i * 40] = x * charset.CharWidth + charset.CharWidth; //1
                    vertices[21 + i * 40] = y * charset.CharHeight + charset.CharHeight; //1

                    vertices[22 + i * 40] = characterMatrix[x, y].TextureCoords.X + tw; ;
                    vertices[23 + i * 40] = characterMatrix[x, y].TextureCoords.Y + th;

                    //Foreground Color
                    vertices[24 + i * 40] = characterMatrix[x, y].ForegroundColor.R / 255.0f;
                    vertices[25 + i * 40] = characterMatrix[x, y].ForegroundColor.G / 255.0f;
                    vertices[26 + i * 40] = characterMatrix[x, y].ForegroundColor.B / 255.0f;

                    //Background Color
                    vertices[27 + i * 40] = characterMatrix[x, y].BackgroundColor.R / 255.0f;
                    vertices[28 + i * 40] = characterMatrix[x, y].BackgroundColor.G / 255.0f;
                    vertices[29 + i * 40] = characterMatrix[x, y].BackgroundColor.B / 255.0f;


                    //Coord 3
                    vertices[30 + i * 40] = x * charset.CharWidth + charset.CharWidth; //1
                    vertices[31 + i * 40] = y * charset.CharHeight; //0

                    vertices[32 + i * 40] = characterMatrix[x, y].TextureCoords.X + tw;
                    vertices[33 + i * 40] = characterMatrix[x, y].TextureCoords.Y;

                    //Foreground Color
                    vertices[34 + i * 40] = characterMatrix[x, y].ForegroundColor.R / 255.0f;
                    vertices[35 + i * 40] = characterMatrix[x, y].ForegroundColor.G / 255.0f;
                    vertices[36 + i * 40] = characterMatrix[x, y].ForegroundColor.B / 255.0f;

                    //Background Color
                    vertices[37 + i * 40] = characterMatrix[x, y].BackgroundColor.R / 255.0f;
                    vertices[38 + i * 40] = characterMatrix[x, y].BackgroundColor.G / 255.0f;
                    vertices[39 + i * 40] = characterMatrix[x, y].BackgroundColor.B / 255.0f;
                    #endregion
                }
            }
        }

        public int Left
        {
            get { return left; }
            set
            {
                left = value;

                if (left < 0)
                    left = 0;
                else if (left >= bufferWidth)
                {
                    left = 0;
                    top++;
                }
            }
        }
        public int Top
        {
            get { return top; }
            set
            {
                top = value;

                if (top < 0)
                    top = 0;
                else if (top >= bufferHeight)
                    top = bufferHeight - 1;
            }
        }
        public int CursorLeft { get { return cursorLeft; } }
        public int CursorTop { get { return cursorTop; } }
        public int BufferWidth { get { return bufferWidth; } }
        public int BufferHeight { get { return bufferHeight; } }
        public Color4 ForegroundColor { get { return foregroundColor; } }
        public Color4 BackgroundColor { get { return backgroundColor; } }
    }
}
