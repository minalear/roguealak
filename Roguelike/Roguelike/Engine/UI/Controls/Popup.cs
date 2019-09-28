using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roguelike.Engine.UI;

namespace Roguelike.Engine.UI.Controls
{
    public class Popup : Control
    {
        public Popup(Control parent)
            : base(parent)
        {
            this.visible = false;
            this.elapsedTime = 0.0;
        }

        public override void DrawStep()
        {
            if (this.visible)
            {
                GraphicConsole.SetColors(Color.Transparent, this.fillColor);
                DrawingUtilities.DrawRect(this.position.X, this.position.Y, this.size.X, this.size.Y, ' ', true);

                GraphicConsole.SetColors(this.borderColor, this.fillColor);
                DrawingUtilities.DrawRect(this.position.X, this.position.Y, this.size.X, this.size.Y, this.borderToken, false);

                if (this.isMultilined)
                {
                    for (int i = 0; i < this.lines.Length; i++)
                    {
                        GraphicConsole.SetColors(this.textColor, this.fillColor);
                        GraphicConsole.SetCursor((GraphicConsole.BufferWidth / 2) - (this.size.X - 4) / 2, this.position.Y + 2 + i);
                        GraphicConsole.Write(this.lines[i]);
                    }
                }
                else
                {
                    GraphicConsole.SetColors(this.textColor, this.fillColor);
                    GraphicConsole.SetCursor((GraphicConsole.BufferWidth / 2) - (this.size.X - 4) / 2, GraphicConsole.BufferHeight / 2);
                    GraphicConsole.Write(this.message);
                }
            }

            base.DrawStep();
        }
        public override void Update(GameTime gameTime)
        {
            if (this.visible)
            {
                this.elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (this.elapsedTime >= duration)
                {
                    this.duration = 0.0;
                    this.visible = false;

                    InterfaceManager.DrawStep();
                }

                if (InputManager.MouseButtonIsDown(MouseButtons.Left))
                    this.elapsedTime = this.duration;
            }
        }

        public void DisplayMessage(string message, double duration)
        {
            this.visible = true;

            this.message = message;
            this.duration = duration;

            this.elapsedTime = 0.0;

            this.setSize();
            InterfaceManager.DrawStep();
        }
        public void DisplayMessage(string message)
        {
            this.visible = true;

            this.message = message;
            this.duration = message.Length * timePerCharacter;

            this.elapsedTime = 0.0;

            this.setSize();
            InterfaceManager.DrawStep();
        }

        private void setSize()
        {
            if (this.message.Contains('\n'))
            {
                int longestWidth = 0;

                this.lines = this.message.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Length > longestWidth)
                        longestWidth = lines[i].Length;
                }

                this.position.X = (GraphicConsole.BufferWidth / 2) - longestWidth / 2 - 2;
                this.position.Y = (GraphicConsole.BufferHeight / 2) - 2;

                this.size.X = longestWidth + 4;
                this.size.Y = lines.Length + 4; //Line Count + spacing + border

                for (int i = 0; i < lines.Length; i++)
                {
                    this.lines[i] = TextUtilities.CenterTextPadding(this.lines[i], longestWidth, ' ');
                }

                this.isMultilined = true;
            }
            else
            {
                this.position.X = (GraphicConsole.BufferWidth / 2) - this.message.Length / 2 - 2;
                this.position.Y = (GraphicConsole.BufferHeight / 2) - 2;

                this.size.X = this.message.Length + 4;
                this.size.Y = 5;

                this.isMultilined = false;
            }
        }

        private bool visible = false;
        private string message = "Display Message";
        private double duration = 0.0;
        private double elapsedTime = 0.0;

        private Color textColor = Color.White;
        private Color fillColor = Color.Black;
        private Color borderColor = Color.Red;
        private Color borderFill = Color.Black;
        private char borderToken = '∙';

        private bool isMultilined = false;
        private string[] lines;

        private double timePerCharacter = 100.0;

        public Color TextColor { get { return this.textColor; } set { this.textColor = value; } }
        public Color FillColor { get { return this.fillColor; } set { this.fillColor = value; } }
        public Color BorderColor { get { return this.borderColor; } set { this.borderColor = value; } }
        public char BorderToken { get { return this.borderToken; } set { this.borderToken = value; } }
    }
}
