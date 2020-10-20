using System;
using System.Collections.Generic;
using Roguelike.Engine.Console;

namespace Roguelike.Engine.UI.Controls
{
    public class Control
    {
        protected Control parent;
        protected Point position, size;
        protected bool isAbsolute;
        protected List<Control> children;

        public Control Parent { get { return parent; } }
        public Point Position { get { return getPosition(); } set { setPosition(value); } }
        public Point Size { get { return size; } set { size = value; } }
        public List<Control> Children { get { return children; } set { children = value; } }
        public bool IsVisible { get; set; }

        public Control()
        {
            isAbsolute = true;
            IsVisible = true;

            children = new List<Control>();
        }
        public Control(Control parent)
        {
            isAbsolute = false;
            IsVisible = true;

            this.parent = parent;
            parent.Children.Add(this);
            children = new List<Control>();
        }

        public virtual void DrawStep()
        {
            GraphicConsole.Instance.ClearColor();

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].IsVisible)
                    children[i].DrawStep();
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].IsVisible)
                    children[i].Update(gameTime);
            }
        }
        public virtual void UpdateStep()
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].IsVisible)
                    children[i].UpdateStep();
            }
        }

        protected virtual bool isMouseHover()
        {
            Point mouse = GraphicConsole.Instance.GetTilePosition(InputManager.GetCurrentMousePosition());
            System.Console.WriteLine(mouse);

            if (mouse.X >= Position.X && mouse.X < Position.X + Size.X &&
                mouse.Y >= Position.Y && mouse.Y < Position.Y + Size.Y)
            {
                return true;
            }
            return false;
        }
        protected virtual bool wasHover()
        {
            Point mouse = GraphicConsole.Instance.GetTilePosition(InputManager.GetPriorMousePosition());

            if (mouse.X >= Position.X && mouse.X < Position.X + Size.X &&
                mouse.Y >= Position.Y && mouse.Y < Position.Y + Size.Y)
            {
                return true;
            }
            return false;
        }
        protected virtual void clearArea()
        {
            GraphicConsole.Instance.ClearColor();
            DrawingUtilities.DrawRect(Position.X, Position.Y, Size.X, Size.Y, ' ', true);
        }
        protected Point getPosition()
        {
            if (isAbsolute)
                return position;
            else
            {
                Point parentPos = Parent.Position;
                return new Point(position.X + parentPos.X, position.Y + parentPos.Y);
            }
        }
        protected void setPosition(Point point)
        {
            if (!isAbsolute)
                position = point;
            else
            {
                Point parentPos = Parent.Position;

                position.X = point.X - parentPos.X;
                position.Y = point.Y - parentPos.Y;
            }
        }
    }
}
