using System.Collections.Generic;
using OpenTK;

namespace Roguelike.Engine.Console
{
    public class Charset
    {
        public Texture2D Texture { get; private set; }
        public int CharWidth { get; private set; }
        public int CharHeight { get; private set; }

        Dictionary<char, int> characterIndex;
        const string CHARSET_STRING =
            " ☺☻♥♦♣♠•◘○◙♂♀♪♫☼" +
            "►◄↕‼¶§▬↨↑↓→←∟↔▲▼" +
            "!\"#$%&'()*+,-./0" +
            "123456789:;<=>?@" +
            "ABCDEFGHIJKLMNOP" +
            "QRSTUVWXYZ[\\]^_`" +
            "abcdefghijklmnop" +
            "qrstuvwxyz{|}~⌂Ç" +
            "üéâäàåçêëèïîìÄÅÉ" +
            "æÆôöòûùÿÖÜ¢£¥₧ƒá" +
            "íóúñÑªº¿⌐¬½¼¡«»░" +
            "▒▓│┤╡╢╖╕╣║╗╝╜╛┐└" +
            "┴┬├─┼╞╟╚╔╩╦╠═╬╧╨" +
            "╤╥╙╘╒╓╫╪┘┌█▄▌▐▀α" +
            "ßΓπΣσµτΦΘΩδ∞φε∩≡" +
            "±≥≤⌠⌡÷≈°∙·√ⁿ²■";

        public Charset(ContentManager contentManager, int charWidth, int charHeight)
        {
            Texture = contentManager.Load<Texture2D>("Content/charset.png");

            CharWidth = charWidth;
            CharHeight = charHeight;

            characterIndex = new Dictionary<char, int>();

            for (int i = 0; i < CHARSET_STRING.Length; i++)
            {
                characterIndex.Add(CHARSET_STRING[i], i);
            }

            //Escape Characters
            characterIndex.Add('\n', 0);
            characterIndex.Add('\r', 0);
            characterIndex.Add('\t', 0);
        }

        public int GetID(char ch)
        {
            return characterIndex[ch];
        }
        public Vector2 CalculateTextureCoords(int id)
        {
            return new Vector2((id % 16) * CharWidth / 128f, (id / 16) * CharHeight / 192f);
        }
    }
}
