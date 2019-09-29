using System;
using OpenTK.Graphics;
using Roguelike.Engine.Console;

namespace Roguelike.Engine
{
    public static class TextUtilities
    {
        public static string WordWrap(string unformattedString, int width)
        {
            string returnString = "";
            unformattedString = ApplyFormatting(unformattedString);

            string[] groups = unformattedString.Split('\n');

            for (int group = 0; group < groups.Length; group++)
            {
                string[] words = groups[group].Split(' ');

                string line = "";
                for (int word = 0; word < words.Length; word++)
                {
                    if (line.Length + words[word].Length <= width)
                    {
                        returnString += words[word] + " ";
                        line += words[word] + " ";
                    }
                    else
                    {
                        returnString += "\n" + words[word] + " ";
                        line = words[word] + " ";
                    }
                }

                returnString += "\n";
            }

            return returnString;
        }

        public static string StripFormatting(string text)
        {
            text = text.Replace("\n", string.Empty);

            return text;
        }

        public static string CenterTextPadding(string unformattedString, int width, char padToken)
        {
            if (unformattedString.Length < width)
            {
                int padAmount = (width - unformattedString.Length) / 2;

                string format = "";
                for (int i = 0; i < padAmount; i++)
                    format += padToken;
                format += unformattedString;
                for (int i = 0; i < padAmount; i++)
                    format += padToken;

                return format;
            }

            return unformattedString;
        }

        public static string ApplyFormatting(string text)
        {
            //Strip C# Formatting
            text = text.Replace("\n", "<br>");
            text = text.Replace("\r", string.Empty);

            //Apply HTML-F Formatting (F is for Fuck)
            text = text.Replace("<br>", "\n");
            text = text.Replace("<tb>", "     ");

            return text;
        }

        public static Color4 GetColor(string colorName)
        {
            colorName = colorName.ToLower();

            if (colorName == "red")
                return Color4.Red;
            if (colorName == "blue")
                return new Color4(80, 109, 255, 255);
            if (colorName == "yellow")
                return Color4.Yellow;
            if (colorName == "green")
                return Color4.Green;
            if (colorName == "orange")
                return Color4.Orange;
            if (colorName == "purple")
                return Color4.Purple;
            if (colorName == "cyan")
                return Color4.Cyan;
            if (colorName == "pink")
                return Color4.Pink;
            if (colorName == "gray")
                return Color4.Gray;
            if (colorName == "darkgray")
                return Color4.DarkGray;
            if (colorName == "transparent" || colorName == "clear")
                return Color4.Transparent;

            return Color4.White;
        }
    }
    public static class DrawingUtilities
    {
        public static void DrawRect(int x, int y, int width, int height, char token, bool solid)
        {
            if (solid)
            {
                for (int l = y; l < y + height; l++)
                {
                    for (int k = x; k < x + width; k++)
                    {
                        GraphicConsole.Instance.Put(token, k, l);
                    }
                }
            }
            else
            {
                height--;
                width--;

                for (int l = y; l <= y + height; l++)
                {
                    GraphicConsole.Instance.Put(token, x, l);
                    GraphicConsole.Instance.Put(token, x + width, l);
                }

                for (int k = x; k <= x + width; k++)
                {
                    GraphicConsole.Instance.Put(token, k, y);
                    GraphicConsole.Instance.Put(token, k, y + height);
                }
            }
        }
        public static void DrawLine(int x0, int y0, int x1, int y1, char token)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep) { Swap<int>(ref x0, ref y0); Swap<int>(ref x1, ref y1); }
            if (x0 > x1) { Swap<int>(ref x0, ref x1); Swap<int>(ref y0, ref y1); }
            int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

            for (int x = x0; x <= x1; ++x)
            {
                /*if (!(steep ? plot(y, x) : plot(x, y))) return;*/
                if (steep)
                    GraphicConsole.Instance.Put(token, y, x);
                else
                    GraphicConsole.Instance.Put(token, x, y);

                err = err - dY;
                if (err < 0) { y += ystep; err += dX; }
            }
        }
        private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

        public static void DrawCircle(int xp, int yp, int r, char token)
        {
            int x = r, y = 0;
            int radiusError = 1 - x;

            while (x >= y)
            {
                GraphicConsole.Instance.Put(token, x + xp, y + yp);
                GraphicConsole.Instance.Put(token, y + xp, x + yp);
                GraphicConsole.Instance.Put(token, -x + xp, y + yp);
                GraphicConsole.Instance.Put(token, -y + xp, x + yp);
                GraphicConsole.Instance.Put(token, -x + xp, -y + yp);
                GraphicConsole.Instance.Put(token, -y + xp, -x + yp);
                GraphicConsole.Instance.Put(token, x + xp, -y + yp);
                GraphicConsole.Instance.Put(token, y + xp, -x + yp);
                y++;
                if (radiusError < 0)
                {
                    radiusError += 2 * y + 1;
                }
                else
                {
                    x--;
                    radiusError += 2 * (y - x + 1);
                }
            }
        }

        public static Color4 BlendColor(Color4 colorOne, Color4 colorTwo, int intensity)
        {
            return new Color4((colorOne.R + colorTwo.R) / 2, (colorOne.G + colorTwo.G) / 2, (colorOne.B + colorTwo.B) / 2, 255);
        }

        public static Point GetScreenPositionFromWorld(Point point)
        {
            return new Point(
                point.X - GameManager.CameraOffset.X + GameManager.Viewport.X,
                point.Y - GameManager.CameraOffset.Y + GameManager.Viewport.Y
                );
        }
        public static Point GetWorldPositionFromScreen(Point point)
        {
            return new Point(
                point.X + GameManager.CameraOffset.X - GameManager.Viewport.X,
                point.Y + GameManager.CameraOffset.Y - GameManager.Viewport.Y
                );
        }
    }
    public static class Extensions
    {
        public static float Truncate(this float f, int digits)
        {
            double mult = Math.Pow(10.0, digits);
            double result = Math.Truncate(mult * f) / mult;
            return (float)result;
        }

        public static double Truncate(this double f, int digits)
        {
            double mult = Math.Pow(10.0, digits);
            double result = Math.Truncate(mult * f) / mult;
            return result;
        }
    }

    /// <summary>
    /// Static Class providing random number functionality.
    /// </summary>
    public static class RNG
    {
        private static Random randomGenerator = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Returns a random integer number.
        /// </summary>
        public static int Next()
        {
            return randomGenerator.Next();
        }

        /// <summary>
        /// Returns a random integer number with an upper limit.
        /// </summary>
        public static int Next(int max)
        {
            return randomGenerator.Next(max);
        }

        /// <summary>
        /// Returns a random integer number between two values.
        /// </summary>
        public static int Next(int min, int max)
        {
            return randomGenerator.Next(min, max);
        }

        /// <summary>
        /// Returns a random double number.
        /// </summary>
        public static double NextDouble()
        {
            return randomGenerator.NextDouble();
        }

        /// <summary>
        /// Returns a random double number between two values.
        /// </summary>
        public static double NextDouble(double min, double max)
        {
            return NextDouble() * (max - min) + min;
        }
    }

    public static class TokenReference
    {
        public const char DOOR_CLOSED = '≡';
        public const char DOOR_OPEN = '∩';
        public const char PLAYER = '@';
        public const char WALL = '█';
        public const char WALL_4 = '█';
        public const char WALL_3 = '▓';
        public const char WALL_2 = '▒';
        public const char WALL_1 = '░';
        public const char FLOOR_EMPTY = '·';
        public const char FLOOR_OBJECT = 'Θ';
        public const char FLOOR_RUBBLE = '•';
        public const char LADDER_UP = '¥';
        public const char LADDER_DOWN = '¥';
        public const char CHEST_CLOSED = '⌂';
        public const char CHEST_OPEN = '^';
    }
}
