using Catel.Data;
using System;
using System.Collections.Generic;
using Wpf2048.Logic;


namespace Wpf2048.Models
{
    public class GameModel : ModelBase
    {
        public GameModel()
        {
            _playerTurn = new PlayerTurn();
            _computerTurn = new ComputerTurn();

            Score = 0;
            HighScore = 0;

            StartTiles = 2;
            KeepPlaying = true;

            Grid = new GridModel(4);
        }

        public IEnumerable<CellModel> Cells { get { return Grid.Cells; } }

        public GridModel Grid { get; private set; }

        public int Score { get; private set; }

        public int HighScore { get; private set; }

        public bool IsGameOver { get; private set; }

        public bool IsGameWon { get; private set; }

        public int StartTiles { get; set; }

        /// <summary>
        ///     Keep playing after winning (allows going over 2048)
        /// </summary>
        public bool KeepPlaying { get; set; }

        public void Restart()
        {
            Grid.Reset();
            Score = 0;

            IsGameOver = false;
            IsGameWon = false;
            
            // Set up the initial tiles to start the game with
            for (var i = 0; i < StartTiles; i++)
            {
                _computerTurn.Move(Grid);
            }
        }

        private void EvaluateScore()
        {
            Score++;
            if (Score > HighScore)
                HighScore = Score;
        }

        // Return true if the game is lost, or has won and the user hasn't kept playing
        public bool IsGameTerminated
        {
            get
            {
                return IsGameOver || (IsGameWon && !KeepPlaying);
            }
        }

        private bool MovesAvailable 
        {
            get
            {
                return Grid.AreCellsAvailable() || AreTileMatchesAvailable();
            }
        }

        // Check for available matches between tiles (more expensive check)
        public bool AreTileMatchesAvailable()
        {
            for (var x = 0; x < Grid.SizeX; x++)
            {
                for (var y = 0; y < Grid.SizeY; y++) 
                {
                    var tile = Grid[x,y].Tile;
                    if (tile != null)
                    {
                        foreach (Direction direction in Vector.Directions) 
                        {
                            var vector = Vector.FromDirection(direction);
                            var other = Grid[x + vector.x, y + vector.y];
                            if (other != null && other.Tile != null && other.Tile.Value == tile.Value) 
                            {
                                return true; // These two tiles can be merged
                            }
                        }
                    }
                }
            }
            return false;
        }

        // Move tiles on the grid in the specified direction
        public bool Move(Direction direction)
        {
            if (IsGameTerminated) return false; // Don't do anything if the game's over

            int deltaScore;
            var moved = _playerTurn.Move(Grid, direction, out deltaScore);
            Score += deltaScore;
            if (Score > HighScore)
                HighScore = Score;
            // The mighty 2048 tile
            if (Grid.MaxValue() == 11 && !KeepPlaying)
            {
                IsGameWon = true;
            }
            else if (moved) 
            {
                _computerTurn.Move(Grid);
                if (!MovesAvailable) 
                {
                    IsGameOver = true; // Game over!
                }
            }

            return moved;
        }

        public void SetGridSize(int value)
        {
            Grid = new GridModel(value);
            Restart();
        }

        private PlayerTurn _playerTurn;
        private ComputerTurn _computerTurn;
    }
}
