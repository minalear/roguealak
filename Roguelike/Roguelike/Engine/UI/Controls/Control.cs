using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Engine.UI.Controls
{
    public class Control
    {
        protected Control parent;
        protected Point position, size;
        protected bool isAbsolute;
        protected List<Control> children;

        public Control Parent { get { return this.parent; } }
        public Point Position { get { return this.getPosition(); } set { this.setPosition(value); } }
        public Point Size { get { return this.size; } set { this.size = value; } }
        public List<Control> Children { get { return this.children; } set { this.children = value; } }
        public bool IsVisible { get; set; }

        public Control()
        {
            this.isAbsolute = true;
            this.IsVisible = true;

            this.children = new List<Control>();
        }
        public Control(Control parent)
        {
            this.isAbsolute = false;
            this.IsVisible = true;

            this.parent = parent;
            this.parent.Children.Add(this);
            this.children = new List<Control>();
        }

        public virtual void DrawStep()
        {
            GraphicConsole.ResetColor();

            for (int i = 0; i < this.children.Count; i++)
            {
                if (this.children[i].IsVisible)
                    this.children[i].DrawStep();
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.children.Count; i++)
            {
                if (this.children[i].IsVisible)
                    this.children[i].Update(gameTime);
            }
        }
        public virtual void UpdateStep()
        {
            for (int i = 0; i < this.children.Count; i++)
            {
                if (this.children[i].IsVisible)
                    this.children[i].UpdateStep();
            }
        }

        protected virtual bool isMouseHover()
        {
            Point mouse = GraphicConsole.GetTilePosition(InputManager.GetCurrentMousePosition());

            if (mouse.X >= this.Position.X && mouse.X < this.Position.X + this.Size.X &&
                mouse.Y >= this.Position.Y && mouse.Y < this.Position.Y + this.Size.Y)
            {
                return true;
            }
            return false;
        }
        protected virtual bool wasHover()
        {
            Point mouse = GraphicConsole.GetTilePosition(InputManager.GetPriorMousePosition());

            if (mouse.X >= this.Position.X && mouse.X < this.Position.X + this.Size.X &&
                mouse.Y >= this.Position.Y && mouse.Y < this.Position.Y + this.Size.Y)
            {
                return true;
            }
            return false;
        }
        protected virtual void clearArea()
        {
            GraphicConsole.ResetColor();
            DrawingUtilities.DrawRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);
        }
        protected Point getPosition()
        {
            if (this.isAbsolute)
                return this.position;
            else
            {
                Point parentPos = this.Parent.Position;
                return new Point(this.position.X + parentPos.X, this.position.Y + parentPos.Y);
            }
        }
        protected void setPosition(Point point)
        {
            if (!this.isAbsolute)
                this.position = point;
            else
            {
                Point parentPos = this.Parent.Position;

                this.position.X = point.X - parentPos.X;
                this.position.Y = point.Y - parentPos.Y;
            }
        }
    }
}
