using System;

namespace Roguelike.Engine
{
    public static class GraphicConsole
    {
        public const int CHAR_WIDTH = 8;
        public const int CHAR_HEIGHT = 12;

        const string CHAR_SET_STRING =
            " ☺☻♥♦♣♠•◘○◙♂♀♪♫☼" +
            "►◄↕‼¶§▬↨↑↓→←∟↔▲▼" +
            " !\"#$%&'()*+,-./" +
            "0123456789:;<=>?" +
            "@ABCDEFGHIJKLMNO" +
            "PQRSTUVWXYZ[\\]^_" +
            "`abcdefghijklmno" +
            "pqrstuvwxyz{|}~⌂" +
            "ÇüéâäàåçêëèïîìÄÅ" +
            "ÉæÆôöòûùÿÖÜ¢£¥₧ƒ" +
            "áíóúñÑªº¿⌐¬½¼¡«»" +
            "░▒▓│┤╡╢╖╕╣║╗╝╜╛┐" +
            "└┴┬├─┼╞╟╚╔╩╦ⁿ═╬╧" +
            "╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀" +
            "αßΓπΣσµτΦΘΩδ∞φε∩" +
            "≡±≥≤⌠⌡÷≈°∙·√ⁿ²■╠";

        static GraphicsDeviceManager _graphics;
        static SpriteBatch _spriteBatch;
        static GameWindow _gameWindow;

        static string _title;

        static Color _backgroundColor;
        static Color _foregroundColor;

        static int _bufferWidth = 80;
        static int _bufferHeight = 25;
        static Vector2 _scale = new Vector2(1f, 1f);
        static int _windowWidth;
        static int _windowHeight;

        static int _cursorLeft;
        static int _cursorTop;

        static CharManager _manager;
        static CharTexture[,] _charMatrix;
        static Texture2D _charSheet;

        //static Effect _consoleEffect;
        static RenderTarget2D _renderTarget;

        static bool _cursorOn = false;
        static bool _displayCursor = true;
        static double _cursorDelay = 500.0;
        static double _elapsedTime = 0.0;

        #region Properties
        public static string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                _gameWindow.Title = value;
            }
        }
        public static Color ForegroundColor
        {
            get { return _foregroundColor; }
            set { _foregroundColor = value; }
        }
        public static Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        public static int BufferWidth
        {
            get { return _bufferWidth; }
            set { _bufferWidth = value; setWindowToBuffer(); }
        }
        public static int BufferHeight
        {
            get { return _bufferHeight; }
            set { _bufferHeight = value; setWindowToBuffer(); }
        }
        public static Vector2 Scale
        {
            get { return _scale; }
            set { _scale = value; setWindowToBuffer(); }
        }
        public static int WindowWidth
        {
            get { return _windowWidth; }
            //set { _windowWidth = value; }
        }
        public static int WindowHeight
        {
            get { return _windowHeight; }
            //set { _windowHeight = value; }
        }

        public static int CursorLeft
        {
            get { return _cursorLeft; }
            set
            {
                _cursorLeft = value;

                if (_cursorLeft < 0)
                    _cursorLeft = 0;
                else if (_cursorLeft >= _bufferWidth)
                    _cursorLeft = _bufferWidth - 1;

                _cursorOn = true;
            }
        }
        public static int CursorTop
        {
            get { return _cursorTop; }
            set
            {
                _cursorTop = value;

                if (_cursorTop < 0)
                    _cursorTop = 0;
                else if (_cursorTop >= _bufferHeight)
                    _cursorTop = _bufferHeight - 1;

                _cursorOn = true;
            }
        }
        public static bool DisplayCursor
        {
            get { return _displayCursor; }
            set { _displayCursor = value; }
        }
        /*public static Effect ConsoleEffect
        {
            get { return _consoleEffect; }
        }*/
        #endregion

        public static void Initialize(GraphicsDeviceManager graphics, GameWindow window)
        {
            _graphics = graphics;
            _gameWindow = window;

            _title = "Graphic Console";

            _backgroundColor = Color.Black;
            _foregroundColor = Color.White;

            _windowWidth = _bufferWidth * CHAR_WIDTH;
            _windowHeight = _bufferHeight * CHAR_HEIGHT;

            _cursorLeft = 0;
            _cursorTop = 0;

            _graphics.PreferredBackBufferWidth = _windowWidth;
            _graphics.PreferredBackBufferHeight = _windowHeight;
        }

        public static void LoadSpriteBatch(SpriteBatch spriteBatch, ContentManager content)
        {
            _spriteBatch = spriteBatch;

            _charSheet = content.Load<Texture2D>(@"Textures/charset");
            //_consoleEffect = content.Load<Effect>(@"ConsoleShader");
            _charMatrix = new CharTexture[_bufferWidth, _bufferHeight];

            _manager = new CharManager();

            _renderTarget = new RenderTarget2D(_graphics.GraphicsDevice, _windowWidth, _windowHeight);
        }

        public static void WriteLine(string line)
        {
            Write(line + '\n');
        }
        public static void Write(string line)
        {
            for (int i = 0; i < line.Length; i++)
                Write(line[i]);
        }
        public static void Write(char ch)
        {
            if (ch == '\n' || ch == '\r')
            {
                CursorTop++;
                _cursorLeft = 0;
            }
            else
            {
                _charMatrix[_cursorLeft, _cursorTop] = new CharTexture();
                _charMatrix[_cursorLeft, _cursorTop].DrawColor = _foregroundColor;
                _charMatrix[_cursorLeft, _cursorTop].BackgroundColor = _backgroundColor;
                _charMatrix[_cursorLeft, _cursorTop].TextureSheetLocation = _manager.GetTextureLocation(ch);
                _charMatrix[_cursorLeft, _cursorTop].Token = ch;

                CursorLeft++;
            }

            _elapsedTime = 0.0;
        }

        public static void Clear()
        {
            _foregroundColor = Color.White;
            _backgroundColor = Color.Black;

            for (int y = 0; y < _bufferHeight; y++)
            {
                for (int x = 0; x < _bufferWidth; x++)
                {
                    _charMatrix[x, y] = new CharTexture();
                    _charMatrix[x, y].Token = ' ';
                    _charMatrix[x, y].TextureSheetLocation = new Point();
                    _charMatrix[x, y].DrawColor = _foregroundColor;
                    _charMatrix[x, y].BackgroundColor = _backgroundColor;
                }
            }

            _cursorLeft = 0;
            _cursorTop = 0;
        }

        public static void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.SetRenderTarget(_renderTarget);
            _graphics.GraphicsDevice.Clear(_backgroundColor);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.DepthRead, RasterizerState.CullNone);

            for (int y = 0; y < _bufferHeight; y++)
            {
                for (int x = 0; x < _bufferWidth; x++)
                {
                    _spriteBatch.Draw(_charSheet,
                        new Vector2((x * CHAR_WIDTH + 1) * _scale.X, (y * CHAR_HEIGHT + 1) * _scale.Y),
                        new Rectangle(_charMatrix[x, y].TextureSheetLocation.X, _charMatrix[x, y].TextureSheetLocation.Y, CHAR_WIDTH, CHAR_HEIGHT),
                        _charMatrix[x, y].DrawColor, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0.9f);

                    _spriteBatch.Draw(_charSheet,
                        new Vector2(x * CHAR_WIDTH, y * CHAR_HEIGHT),
                        new Rectangle(11 * CHAR_WIDTH, 13 * CHAR_HEIGHT, CHAR_WIDTH, CHAR_HEIGHT),
                        _charMatrix[x, y].BackgroundColor, 0f, Vector2.Zero, _scale, SpriteEffects.None, 1f);
                }
            }

            if (_cursorOn && _displayCursor)
            {
                CharTexture cursor = new CharTexture();
                cursor.TextureSheetLocation = _manager.GetTextureLocation('█');

                _spriteBatch.Draw(_charSheet,
                        new Vector2((_cursorLeft * CHAR_WIDTH + 1) * _scale.X, (_cursorTop * CHAR_HEIGHT + 1) * _scale.Y),
                        new Rectangle(cursor.TextureSheetLocation.X, cursor.TextureSheetLocation.Y, CHAR_WIDTH, CHAR_HEIGHT),
                        _foregroundColor, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0.9f);
            }

            _spriteBatch.End();

            _graphics.GraphicsDevice.SetRenderTarget(null);
            _graphics.GraphicsDevice.Clear(_backgroundColor);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            //_consoleEffect.CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);

            _spriteBatch.End();
        }

        public static void Update(GameTime gameTime)
        {
            if (_displayCursor)
            {
                _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                if (_elapsedTime >= _cursorDelay)
                {
                    _elapsedTime = 0.0;
                    _cursorOn = !_cursorOn;
                }
            }
        }

        public static void SetCursor(int left, int top)
        {
            CursorLeft = left;
            CursorTop = top;
        }

        public static void SetCursor(Point position)
        {
            CursorLeft = position.X;
            CursorTop = position.Y;
        }

        public static void ResetColor()
        {
            _foregroundColor = Color.White;
            _backgroundColor = Color.Black;
        }

        public static void SetColors(Color fore, Color back)
        {
            _foregroundColor = fore;
            _backgroundColor = back;
        }

        public static void Put(char ch)
        {
            if (ch != '\n' && ch != '\r')
            {
                _charMatrix[_cursorLeft, _cursorTop] = new CharTexture();
                _charMatrix[_cursorLeft, _cursorTop].DrawColor = _foregroundColor;
                _charMatrix[_cursorLeft, _cursorTop].BackgroundColor = _backgroundColor;
                _charMatrix[_cursorLeft, _cursorTop].TextureSheetLocation = _manager.GetTextureLocation(ch);
                _charMatrix[_cursorLeft, _cursorTop].Token = ch;
            }
        }

        public static void Put(char ch, int x, int y)
        {
            SetCursor(x, y);

            if (ch != '\n' && ch != '\r')
            {
                _charMatrix[_cursorLeft, _cursorTop] = new CharTexture();
                _charMatrix[_cursorLeft, _cursorTop].DrawColor = _foregroundColor;
                _charMatrix[_cursorLeft, _cursorTop].BackgroundColor = _backgroundColor;
                _charMatrix[_cursorLeft, _cursorTop].TextureSheetLocation = _manager.GetTextureLocation(ch);
                _charMatrix[_cursorLeft, _cursorTop].Token = ch;
            }
        }

        public static Point GetTilePosition(Point position)
        {
            return new Point((int)(Math.Round(position.X / CHAR_WIDTH * _scale.X)), (int)(Math.Round(position.Y / CHAR_HEIGHT * _scale.Y)));
        }

        public static Point GetTilePosition(int x, int y)
        {
            return new Point((int)(x / CHAR_WIDTH * _scale.X), (int)(y / CHAR_HEIGHT * _scale.Y));
        }

        public static Color[] GetColorsAtTile(int x, int y)
        {
            if (x >= 0 && x < BufferWidth && y >= 0 && y < BufferHeight)
                return new Color[] { _charMatrix[x, y].DrawColor, _charMatrix[x, y].BackgroundColor };

            return new Color[] { Color.White, Color.Black };
        }

        private static string _clipboardText = " ";
        public static string GetClipboardText()
        {
            /*Thread t = new Thread(getClipboardText);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            while (t.IsAlive) { }*/

            return _clipboardText;
        }

        private static void getClipboardText()
        {
            if (System.Windows.Forms.Clipboard.ContainsText())
                _clipboardText = System.Windows.Forms.Clipboard.GetText();
            else
                _clipboardText = " ";
        }

        private static void setWindowToBuffer()
        {
            _windowWidth = (int)((_bufferWidth * CHAR_WIDTH) * _scale.X) + 2;
            _windowHeight = (int)((_bufferHeight * CHAR_HEIGHT) * _scale.Y);

            _graphics.PreferredBackBufferWidth = _windowWidth;
            _graphics.PreferredBackBufferHeight = _windowHeight;

            _charMatrix = new CharTexture[_bufferWidth, _bufferHeight];
            Clear();
        }

        struct CharTexture
        {
            public char Token;
            public Point TextureSheetLocation;
            public Color DrawColor;
            public Color BackgroundColor;
        }
        class CharManager
        {
            charTextureReference[] references;

            public CharManager()
            {
                references = new charTextureReference[CHAR_SET_STRING.Length];
                int i = 0;
                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++, i++)
                    {
                        references[i] = new charTextureReference();
                        references[i].charToken = CHAR_SET_STRING[i];
                        references[i].charSheetLocation = new Point(x * CHAR_WIDTH, y * CHAR_HEIGHT);
                    }
                }
            }
            public Point GetTextureLocation(char token)
            {
                for (int i = 0; i < references.Length; i++)
                {
                    if (references[i].charToken == token)
                        return references[i].charSheetLocation;
                }

                return Point.Zero;
            }

            struct charTextureReference
            {
                public char charToken;
                public Point charSheetLocation;
            }
        }
    }
}
