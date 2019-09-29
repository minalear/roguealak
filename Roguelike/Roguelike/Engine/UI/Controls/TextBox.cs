using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            GraphicConsole.SetColors(textColor, fillColor);
            DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);

            if (!string.IsNullOrEmpty(text))
            {
                if (scroll)
                {
                    //Scroll Bar Rail
                    GraphicConsole.SetColors(scrollRailColor, fillColor);
                    for (int h = Position.Y; h < Size.Y + Position.Y; h++)
                    {
                        GraphicConsole.SetCursor(Position.X + Size.X, h);
                        GraphicConsole.Write(scrollRail);
                    }

                    //Scroll Bar
                    GraphicConsole.SetColors(scrollBarColor, fillColor);
                    GraphicConsole.SetCursor(Position.X + Size.X, (int)(scrollValue / 100f * Size.Y) + Position.Y);
                    GraphicConsole.Write(scrollBar);

                    string[] lines = text.Split('\n');
                    lineCount = lines.Length;

                    int line = (int)(scrollValue / 100f * (lines.Length - Size.Y + 1));
                    if (line < 0)
                        line = 0;

                    GraphicConsole.SetColors(textColor, fillColor);
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
            GraphicConsole.SetCursor(x, y);
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
                        GraphicConsole.SetColors(TextUtilities.GetColor(formatTag), fillColor);
                    }
                    else if (formatTag == "<color>") //End Custom Color
                    {
                        //Reset colors back to normal
                        GraphicConsole.SetColors(textColor, fillColor);
                    }
                }
                GraphicConsole.Write(line[i]);
            }

            GraphicConsole.Write('\n');
        }

        private string text;
        private bool scroll;

        private float scrollValue = 0f;
        private int lineCount = 0;

        private char scrollRail = '║';
        private char scrollBar = '▓';

        private Color textColor = Color.White;
        private Color fillColor = Color.Black;
        private Color scrollRailColor = Color.Gray;
        private Color scrollBarColor = Color.LightGray;

        #region Properties
        public string Text { get { return text; } set { setText(value); } }
        public float ScrollValue { get { return scrollValue; } set { scrollValue = value; } }
        public Color TextColor { get { return textColor; } set { textColor = value; } }
        public Color FillColor { get { return fillColor; } set { fillColor = value; } }
        public Color ScrollRailColor { get { return scrollRailColor; } set { scrollRailColor = value; } }
        public Color ScrollBarColor { get { return scrollBarColor; } set { scrollBarColor = value; } }
        public char ScrollRail { get { return scrollRail; } set { scrollRail = value; } }
        public char ScrollBar { get { return scrollBar; } set { scrollBar = value; } } 
        #endregion
    }
}
