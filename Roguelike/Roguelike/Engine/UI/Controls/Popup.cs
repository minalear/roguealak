using System;
using System.Linq;
using OpenTK.Graphics;

namespace Roguelike.Engine.UI.Controls
{
    public class Popup : Control
    {
        public Popup(Control parent)
            : base(parent)
        {
            visible = false;
            elapsedTime = 0.0;
        }

        public override void DrawStep()
        {
            if (visible)
            {
                GraphicConsole.SetColors(Color.Transparent, fillColor);
                DrawingUtilities.DrawRect(position.X, position.Y, size.X, size.Y, ' ', true);

                GraphicConsole.SetColors(borderColor, fillColor);
                DrawingUtilities.DrawRect(position.X, position.Y, size.X, size.Y, borderToken, false);

                if (isMultilined)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        GraphicConsole.SetColors(textColor, fillColor);
                        GraphicConsole.SetCursor((GraphicConsole.BufferWidth / 2) - (size.X - 4) / 2, position.Y + 2 + i);
                        GraphicConsole.Write(lines[i]);
                    }
                }
                else
                {
                    GraphicConsole.SetColors(textColor, fillColor);
                    GraphicConsole.SetCursor((GraphicConsole.BufferWidth / 2) - (size.X - 4) / 2, GraphicConsole.BufferHeight / 2);
                    GraphicConsole.Write(message);
                }
            }

            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (visible)
            {
                elapsedTime += gameTime.ElapsedTime.Milliseconds;

                if (elapsedTime >= duration)
                {
                    duration = 0.0;
                    visible = false;

                    InterfaceManager.DrawStep();
                }

                if (InputManager.MouseButtonIsDown(MouseButtons.Left))
                    elapsedTime = duration;
            }
        }

        public void DisplayMessage(string message, double duration)
        {
            visible = true;

            this.message = message;
            this.duration = duration;

            elapsedTime = 0.0;

            setSize();
            InterfaceManager.DrawStep();
        }
        public void DisplayMessage(string message)
        {
            visible = true;

            this.message = message;
            duration = message.Length * timePerCharacter;

            elapsedTime = 0.0;

            setSize();
            InterfaceManager.DrawStep();
        }

        private void setSize()
        {
            if (message.Contains('\n'))
            {
                int longestWidth = 0;

                lines = message.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Length > longestWidth)
                        longestWidth = lines[i].Length;
                }

                position.X = (GraphicConsole.BufferWidth / 2) - longestWidth / 2 - 2;
                position.Y = (GraphicConsole.BufferHeight / 2) - 2;

                size.X = longestWidth + 4;
                size.Y = lines.Length + 4; //Line Count + spacing + border

                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = TextUtilities.CenterTextPadding(lines[i], longestWidth, ' ');
                }

                isMultilined = true;
            }
            else
            {
                position.X = (GraphicConsole.BufferWidth / 2) - message.Length / 2 - 2;
                position.Y = (GraphicConsole.BufferHeight / 2) - 2;

                size.X = message.Length + 4;
                size.Y = 5;

                isMultilined = false;
            }
        }

        private bool visible = false;
        private string message = "Display Message";
        private double duration = 0.0;
        private double elapsedTime = 0.0;

        private Color4 textColor = Color4.White;
        private Color4 fillColor = Color4.Black;
        private Color4 borderColor = Color4.Red;
        private Color4 borderFill = Color4.Black;
        private char borderToken = '∙';

        private bool isMultilined = false;
        private string[] lines;

        private double timePerCharacter = 100.0;

        public Color4 TextColor { get { return textColor; } set { textColor = value; } }
        public Color4 FillColor { get { return fillColor; } set { fillColor = value; } }
        public Color4 BorderColor { get { return borderColor; } set { borderColor = value; } }
        public char BorderToken { get { return borderToken; } set { borderToken = value; } }
    }
}
