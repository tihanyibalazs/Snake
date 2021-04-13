using Snake.AIs;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Snake.Models
{
    public enum FieldTypes
    {
        Snake, Free, Wall, Food
    };

    public enum Direction
    {
        Up, Right, Down, Left
    };

    public class SnakeGameModel
    {
        #region Fields

        private FieldTypes[][] _gameMap;
        private int _score;
        private List<Point> _snakeBody;
        private Point _snakeHead;
        private int _tableHeight;
        private int _tableWidth;
        private Point _food;
        private Random _rnd = new Random();
        private ISnakeAI _ai;
        private int _extraBody;

        #endregion

        #region Events

        public event EventHandler AiAdvanced;
        public event EventHandler AiDied;
        public event EventHandler AiScored;

        #endregion

        #region Properties

        public int Score { get { return _score; } }
        public int TableHeight { get { return _tableHeight; } }
        public int TableWidth { get { return _tableWidth; } }
        public List<Point> SnakeBody { get { return _snakeBody; } }
        public double FoodX { get { return _food.X; } }
        public double FoodY { get { return _food.Y; } }
        public Point Head { get { return _snakeHead; } }
        public FieldTypes MapField(int x, int y)
        {
            return _gameMap[x][y];
        }

        #endregion

        #region Constructor

        public SnakeGameModel(int Width, int Height)
        {
            _tableWidth = Width;
            _tableHeight = Height;
            _gameMap = new FieldTypes[_tableWidth][];

            for (int i = 0; i < _tableWidth; i++)
            {
                _gameMap[i] = new FieldTypes[_tableHeight];
            }

            _snakeBody = new List<Point>(_tableHeight * _tableWidth);

            NewGame();
        }

        #endregion

        #region NewGame

        public void NewGame()
        {
            for (int i = 0; i < _tableWidth; i++)
            {
                for (int j = 0; j < _tableHeight; j++)
                {
                    _gameMap[i][j] = FieldTypes.Free;
                }
            }

            _gameMap[9][9] = FieldTypes.Snake;
            _gameMap[8][9] = FieldTypes.Snake;
            _gameMap[7][9] = FieldTypes.Snake;

            _snakeBody.Clear();
            _snakeBody.Add(new Point(7, 9));
            _snakeBody.Add(new Point(8, 9));
            _snakeBody.Add(new Point(9, 9));
            _snakeHead = new Point(9, 9);
            _score = 0;
            NextFood();

            for (int i = 0; i < _tableWidth; i++)
            {
                _gameMap[0][i] = FieldTypes.Wall;
                _gameMap[i][0] = FieldTypes.Wall;
                _gameMap[_tableHeight - 1][i] = FieldTypes.Wall;
                _gameMap[i][_tableWidth - 1] = FieldTypes.Wall;
            }
        }

        #endregion

        #region Move

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    Specific_Move(_snakeHead.X, _snakeHead.Y - 1);
                    break;
                case Direction.Right:
                    Specific_Move(_snakeHead.X + 1, _snakeHead.Y);
                    break;
                case Direction.Left:
                    Specific_Move(_snakeHead.X - 1, _snakeHead.Y);
                    break;
                case Direction.Down:
                    Specific_Move(_snakeHead.X, _snakeHead.Y + 1);
                    break;
            }
        }

        #endregion

        #region SpecificMove

        public void Specific_Move(double x, double y)
        {
            if (Collision(x, y))
            {
                OnAiDeath();
                return;
            }

            if (IsFood(x, y))
            {
                _score += 1;
                _gameMap[(int)x][(int)y] = FieldTypes.Snake;
                _snakeBody.Add(new Point(x, y));
                _snakeHead.X = x;
                _snakeHead.Y = y;
                NextFood();
                OnAiScore();
                _extraBody++;
                return;
            }

            _gameMap[(int)x][(int)y] = FieldTypes.Snake;
            _snakeBody.Add(new Point(x, y));
            _snakeHead.X = x;
            _snakeHead.Y = y;
            if (_extraBody == 0)
            {
                _gameMap[(int)_snakeBody[0].X][(int)_snakeBody[0].Y] = FieldTypes.Free;
                _snakeBody.RemoveAt(0);
            }
            if (_extraBody > 0)
            {
                _extraBody--;
            }
        }

        #endregion

        #region Collision

        public bool Collision(double x, double y)
        {
            if (_gameMap[(int)x][(int)y] == FieldTypes.Wall || _gameMap[(int)x][(int)y] == FieldTypes.Snake)
                return true;

            return false;
        }

        #endregion

        #region IsFood

        public bool IsFood(double x, double y)
        {
            if (_gameMap[(int)x][(int)y] == FieldTypes.Food)
                return true;

            return false;
        }

        #endregion

        #region NextFood

        public void NextFood()
        {
            int x = _rnd.Next(1, _tableWidth - 1);
            int y = _rnd.Next(1, _tableHeight - 1);

            while (y < _tableHeight - 1 && _gameMap[x][y] != FieldTypes.Free)
            {
                y++;
            }

            if (_gameMap[x][y] != FieldTypes.Free)
                NextFood();

            _gameMap[x][y] = FieldTypes.Food;

            _food = new Point(x, y);
        }

        #endregion

        #region SetAi

        public void SetAI(ISnakeAI ai)
        {
            _ai = ai;
        }

        #endregion

        #region AiMove

        public void NextMove()
        {
            Move(_ai.NextMove());
            OnAiAdvanced();
        }

        #endregion

        #region EventsInvoke

        private void OnAiAdvanced()
        {
            AiAdvanced?.Invoke(this, EventArgs.Empty);
        }

        private void OnAiDeath()
        {
            AiDied?.Invoke(this, EventArgs.Empty);
        }

        private void OnAiScore()
        {
            AiScored?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
