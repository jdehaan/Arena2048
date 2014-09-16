using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf2048.Models;

namespace Wpf2048.Logic
{
    public class ComputerTurn
    {
        public ComputerTurn()
        {
            _percentageOfValue1 = 0.9;
        }

        /// <summary>
        ///     Adds a tile in a random position
        /// </summary>
        public void Move(GridModel grid)
        {
            var cells = grid.AvailableCells();
            if (cells.Any())
            {
                int value = _rand.NextDouble() < _percentageOfValue1 ? 1 : 2;
                var tile = new TileModel(value);
                var position = cells[_rand.Next(cells.Length)];
                tile.Position = position;
            }
        }

        private double _percentageOfValue1;
        private Random _rand = new Random();
    }
}
