using Snake.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Snake.ViewModels
{
    public class SnakeViewModel : ViewModelBase
    {
        public ObservableCollection<RectItem> Fields { get; set; }
        private int _score;
        SnakeGameModel _model;
        public int Score
        {
            get { return _score; }
            set
            {
                if (_score != value)
                {
                    _score = value;
                    OnPropertyChanged();
                }
            }
        }
        public SnakeViewModel(SnakeGameModel model)
        {
            _model = model;
            _model.AiAdvanced += new EventHandler(Model_AiAdvanced);
            _model.AiScored += new EventHandler(Model_AiScored);
            _model.AiDied += new EventHandler(Model_AiDied);

            Fields = new ObservableCollection<RectItem>();
            Double X = 0;
            Double Y = 0;
            for (int i = 0; i < _model.TableHeight; i++)
            {
                for (int j = 0; j < _model.TableWidth; j++)
                {
                    if (_model.MapField(i, j) == FieldTypes.Wall)
                    {
                        RectItem r = new RectItem(X, Y, 10, 10, "Black");
                        Fields.Add(r);
                    }
                    if (_model.MapField(i, j) == FieldTypes.Food)
                    {
                        RectItem r = new RectItem(X, Y, 10, 10, "Red");
                        Fields.Add(r);
                    }
                    if (_model.MapField(i, j) == FieldTypes.Snake)
                    {
                        RectItem r = new RectItem(X, Y, 10, 10, "Green");
                        Fields.Add(r);
                    }
                    if (_model.MapField(i, j) == FieldTypes.Free)
                    {
                        RectItem r = new RectItem(X, Y, 10, 10, "Grey");
                        Fields.Add(r);
                    }
                    X += 10;
                }
                X = 0;
                Y += 10;
            }
        }

        private void Model_AiDied(object sender, EventArgs e)
        {
            MessageBox.Show("Ai scored: " + Score.ToString(), "Game Over", MessageBoxButton.OK);
            _model.NewGame();
            RefreshTable();
            Score = _model.Score;
        }

        private void Model_AiScored(object sender, EventArgs e)
        {
            Score = _model.Score;
        }

        private void Model_AiAdvanced(object sender, EventArgs e)
        {
            RefreshTable();
        }

        public void RefreshTable()
        {
            for (int i = 0; i < _model.TableHeight; i++)
            {
                for (int j = 0; j < _model.TableWidth; j++)
                {
                    if (_model.MapField(i, j) == FieldTypes.Wall)
                    {
                        ColorRectItem(i, j, "Black");
                    }
                    if (_model.MapField(i, j) == FieldTypes.Food)
                    {
                        ColorRectItem(i, j, "Red");
                    }
                    if (_model.MapField(i, j) == FieldTypes.Snake)
                    {
                        ColorRectItem(i, j, "Green");
                    }
                    if (_model.MapField(i, j) == FieldTypes.Free)
                    {
                        ColorRectItem(i, j, "Grey");
                    }
                }
            }
        }

        public void ColorRectItem(int x, int y, string c)
        {
            int index = x * _model.TableWidth + y;
            Fields[index].Color = c;
        }
    }
}
