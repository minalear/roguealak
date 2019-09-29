using System;
using OpenTK.Graphics;
using Roguelike.Engine.Console;

namespace Roguelike.Engine.UI.Controls
{
    public class TextBox : Control
    {
        public TextBox(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            Position = new Point(x, y);
            Size = new Point(width, height);

            setText(" ");
        }

        public override void DrawStep()
        {
            GraphicConsole.Instance.SetColors(textColor, fillColor);
            DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

            if (!string.IsNullOrEmpty(text))
            {
                if (scroll)
                {
                    //Scroll Bar Rail
                    GraphicConsole.Instance.SetColors(scrollRailColor, fillColor);
                    for (int h = Position.Y; h < Size.Y + Position.Y; h++)
                    {
                        GraphicConsole.Instance.SetCursor(Position.X + Size.X, h);
                        GraphicConsole.Instance.Write(scrollRail);
                    }

                    //Scroll Bar
                    GraphicConsole.Instance.SetColors(scrollBarColor, fillColor);
                    GraphicConsole.Instance.SetCursor(Position.X + Size.X, (int)(scrollValue / 100f * Size.Y) + Position.Y);
                    GraphicConsole.Instance.Write(scrollBar);

                    string[] lines = text.Split('\n');
                    lineCount = lines.Length;

                    int line = (int)(scrollValue / 100f * (lines.Length - Size.Y + 1));
                    if (line < 0)
                        line = 0;

                    GraphicConsole.Instance.SetColors(textColor, fillColor);
                    for (int y = 0; y < Size.Y && y < lines.Length; y++)
                    {
                        if (line < lines.Length)
                        {
                            writeLine(lines[line], Position.X, Position.Y + y);
                        }

                        line++;
                    }
                }
                else
                {
                    string[] lines = text.Split('\n');
                    for (int line = 0; line < lines.Length && line < Size.Y; line++)
                    {
                        writeLine(lines[line], Position.X, Position.Y + line);
                    }
                }

                base.DrawStep();
            }
        } 
        public override void Update(GameTime gameTime)
        {
            if (isMouseHover() && scroll)
            {
                int differenceValue = InputManager.GetDistanceScrolled();

                if (differenceValue != 0)
                {
                    scrollValue -= differenceValue / (lineCount / 2);

                    if (scrollValue < 0f)
                        scrollValue = 0f;
                    else if (scrollValue >= 100f)
                        scrollValue = 99f;

                    InterfaceManager.DrawStep();
                }
            }
        }

        private void setText(string t)
        {
            text = TextUtilities.WordWrap(t, Size.X - 1);

            string[] lines = text.Split('\n');

            if (lines.Length > Size.Y)
                scroll = true;
            else
                scroll = false;
        }
        private void writeLine(string line, int x, int y)
        {
            GraphicConsole.Instance.SetCursor(x, y);
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '<')
                {
                    int k = i;
                    string formatTag = "";
                    while (k < line.Length && line[k] != '>')
                    {
                        formatTag += line[k];
                        k++;
                    }
                    formatTag += ">";

                    line = line.Remove(i, formatTag.Length);

                    if (formatTag.Contains("<color ")) //Start Custom Color
                    {
                        //Get the color specified
                        formatTag = formatTag.Remove(0, 7);
                        formatTag = formatTag.Remove(formatTag.Length - 1);

                        //Retrieve the color and apply it
                        GraphicConsole.Instance.SetColors(TextUtilities.GetColor(formatTag), fillColor);
                    }
                    else if (formatTag == "<color>") //End Custom Color
                    {
                        //Reset colors back to normal
                        GraphicConsole.Instance.SetColors(textColor, fillColor);
                    }
                }
                GraphicConsole.Instance.Write(line[i]);
            }

            GraphicConsole.Instance.Write('\n');
        }

        private string text;
        private bool scroll;

        private float scrollValue = 0f;
        private int lineCount = 0;

        private char scrollRail = '║';
        private char scrollBar = '▓';

        private Color4 textColor = Color4.White;
        private Color4 fillColor = Color4.Black;
        private Color4 scrollRailColor = Color4.Gray;
        private Color4 scrollBarColor = Color4.LightGray;

        #region Properties
        public string Text { get { return text; } set { setText(value); } }
        public float ScrollValue { get { return scrollValue; } set { scrollValue = value; } }
        public Color4 TextColor { get { return textColor; } set { textColor = value; } }
        public Color4 FillColor { get { return fillColor; } set { fillColor = value; } }
        public Color4 ScrollRailColor { get { return scrollRailColor; } set { scrollRailColor = value; } }
        public Color4 ScrollBarColor { get { return scrollBarColor; } set { scrollBarColor = value; } }
        public char ScrollRail { get { return scrollRail; } set { scrollRail = value; } }
        public char ScrollBar { get { return scrollBar; } set { scrollBar = value; } } 
        #endregion
    }
}
