using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Wpf2048.Logic.Bot;
using Wpf2048.Models;
using Wpf2048.Services;

namespace Wpf2048.ViewModels
{
    class MainViewModel : ViewModelBase, IDisposable
    {
        public MainViewModel(/*IBotsManager botsManager*/)
        {
            /*
            _botsManager = botsManager;
            _ai = new BackgroundWorker();
            _ai.WorkerSupportsCancellation = true;
            _ai.DoWork += AiDoWork;
            */
            NewGame = new Command(NewGameExecute, NewGameCanExecute);
            MoveLeft = new Command(MoveLeftExecute);
            MoveRight = new Command(MoveRightExecute);
            MoveUp = new Command(MoveUpExecute);
            MoveDown = new Command(MoveDownExecute);

            Game = new GameModel();
        }

        [Model]
        [Catel.Fody.Expose("Grid")]
        [Catel.Fody.Expose("Score")]
        [Catel.Fody.Expose("HighScore")]
        [Catel.Fody.Expose("IsGameOver")]
        [Catel.Fody.Expose("IsGameWon")]
        [Catel.Fody.Expose("KeepPlaying")]
        [Catel.Fody.Expose("StartTiles")]
        public GameModel Game { get; private set; }

        public bool AutoPlay { get; set; }

        private void OnAutoPlayChanged()
        {
            if (AutoPlay)
            {
                _ai.RunWorkerAsync();
            }
            else
            {
                _ai.CancelAsync();
            }
        }

        public bool TrainingMode { get; set; }

        private BackgroundWorker _ai;
        private IBotsManager _botsManager;

        public IEnumerable<IBot> AvailableBots
        {
            get
            {
                if (_botsManager == null)
                    return Enumerable.Empty<IBot>();
                return _botsManager.Bots;
            }
        }

        public IBot SelectedBot { get; set; }

        private void AiDoWork(object Sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (!Game.IsGameOver && !Game.IsGameWon)
            {
                if (_ai.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                var bot = SelectedBot;
                if (bot != null)
                {
                    var move = bot.ComputeNextMove(Game.Grid);
                    if (move != Direction.None)
                    {
                        Console.WriteLine("AI plays: {0}", move);
                        Game.Move(move);
                        Thread.Sleep(100);
                    }
                }
                else break;
            }
            AutoPlay = false;
            RefreshMetrics();
        }

        public int GridSize
        {
            get
            {
                return Math.Max(Game.Grid.SizeX, Game.Grid.SizeY);
            }
            set
            {
                Game.SetGridSize(value);
                RaisePropertyChanged(String.Empty);
            }
        }

        public Command NewGame { get; private set; }

        private void NewGameExecute()
        {
            Game.Restart();
        }

        private bool NewGameCanExecute()
        {
            return true;
        }

        public Command MoveLeft { get; private set; }
        public Command MoveRight { get; private set; }
        public Command MoveUp { get; private set; }
        public Command MoveDown { get; private set; }

        private void MoveLeftExecute()
        {
            if (!AutoPlay)
                ManualMove(Direction.Left);
        }

        private void MoveRightExecute()
        {
            if (!AutoPlay)
                ManualMove(Direction.Right);
        }

        private void MoveUpExecute()
        {
            if (!AutoPlay)
                ManualMove(Direction.Up);
        }

        private void MoveDownExecute()
        {
            if (!AutoPlay)
                ManualMove(Direction.Down);
        }

        private void ManualMove(Direction direction)
        {
            if (TrainingMode)
            {
                var bot = SelectedBot;
                if (bot != null && bot.CanLearn)
                {
                    bot.TrainNextMove(Game.Grid, direction);
                }
            }
            Game.Move(direction);
            RefreshMetrics();
        }

        private void RefreshMetrics()
        {
            RaisePropertyChanged("Monotonicity");
            RaisePropertyChanged("Monotonicity2");
            RaisePropertyChanged("Smoothness");
            RaisePropertyChanged("MaxValue");
            RaisePropertyChanged("AverageValue");
        }

        public double Monotonicity
        {
            get { return Game.Grid.Monotonicity(); }
        }

        public double Monotonicity2
        {
            get { return Game.Grid.Monotonicity2(); }
        }

        public double Smoothness
        {
            get { return Game.Grid.Smoothness(); }
        }

        public double MaxValue
        {
            get { return Game.Grid.MaxValue(); }
        }

        public double AverageValue
        {
            get { return Game.Grid.AverageValue(); }
        }

        public void Dispose()
        {
            _ai.Dispose();
        }
    }
}
