using Snake.Models;
using System.Windows;

namespace Snake.AIs
{
    public class SnakeAI : ISnakeAI
    {
        SnakeGameModel _model;

        public SnakeAI(SnakeGameModel model)
        {
            _model = model;
        }

        public Direction NextMove()
        {
            Point Head = _model.Head;
            Point Food = new Point(_model.FoodX, _model.FoodY);

            if (Head.X < Food.X && FoodOrFree((int)Head.X + 1, (int)Head.Y))
                return Direction.Right;

            if (Head.X > Food.X && FoodOrFree((int)Head.X - 1, (int)Head.Y))
                return Direction.Left;

            if (Head.Y > Food.Y && FoodOrFree((int)Head.X, (int)Head.Y - 1))
                return Direction.Up;

            if (Head.Y < Food.Y && FoodOrFree((int)Head.X, (int)Head.Y + 1))
                return Direction.Down;

            if (FoodOrFree((int)Head.X + 1, (int)Head.Y))
                return Direction.Right;

            if (FoodOrFree((int)Head.X - 1, (int)Head.Y))
                return Direction.Left;

            if (FoodOrFree((int)Head.X, (int)Head.Y + 1))
                return Direction.Down;

            if (FoodOrFree((int)Head.X, (int)Head.Y - 1))
                return Direction.Up;

            return Direction.Left;
        }

        private bool FoodOrFree(int x, int y)
        {
            return _model.MapField(x, y) == FieldTypes.Food || _model.MapField(x, y) == FieldTypes.Free;
        }
    }
}
