using Snake.AIs;
using Snake.Models;
using Snake.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Snake
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow _view;
        private SnakeViewModel _viewModel;
        private SnakeGameModel _model;
        private DispatcherTimer _timer;
        private SnakeAI _ai;
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        public void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new SnakeGameModel(50, 50);
            _viewModel = new SnakeViewModel(_model);
            _ai = new SnakeAI(_model);
            _model.SetAI(_ai);

            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing);
            _view.Show();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(30);
            _timer.Tick += new EventHandler(Tick);
            _timer.Start();
        }

        private void View_Closing(object sender, CancelEventArgs e)
        {
            _timer.Stop();
        }

        private void Tick(object sender, EventArgs e)
        {
            _model.NextMove();
        }

    }
}
