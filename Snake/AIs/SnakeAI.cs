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

            if (Head.X < Food.X && FoodOrFree(Head.X + 1, Head.Y))
                return Direction.Right;

            if (Head.X > Food.X && FoodOrFree(Head.X - 1, Head.Y))
                return Direction.Left;

            if (Head.Y > Food.Y && FoodOrFree(Head.X, Head.Y - 1))
                return Direction.Up;

            if (Head.Y < Food.Y && FoodOrFree(Head.X, Head.Y + 1))
                return Direction.Down;

            if (FoodOrFree(Head.X + 1, Head.Y))
                return Direction.Right;

            if (FoodOrFree(Head.X - 1, Head.Y))
                return Direction.Left;

            if (FoodOrFree(Head.X, Head.Y + 1))
                return Direction.Down;

            if (FoodOrFree(Head.X, Head.Y - 1))
                return Direction.Up;

            return Direction.Left;
        }

        private bool FoodOrFree(double x, double y)
        {
            return _model.MapField((int)x, (int)y) == FieldTypes.Food || _model.MapField((int)x, (int)y) == FieldTypes.Free;
        }
    }
}
