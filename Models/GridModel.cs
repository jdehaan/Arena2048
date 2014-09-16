using Catel.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wpf2048.Models
{
    public class GridModel : ModelBase
    {
        public GridModel(int size)
        {
            LeanAndMeanModel = true;
            SizeX = size;
            SizeY = size;
            _cells = new List<CellModel>(SizeX * SizeY);

            for (int i = 0; i < SizeX * SizeY; i++)
            {
                var cell = new CellModel(i % SizeY, i / SizeX);
                var tile = new TileModel(1);
                tile.Position = cell;
                _cells.Add(cell);
            }
            //Reset();
        }

        /// <summary>
        ///     Create a deep copy of a grid, history of moves and merges removed.
        /// </summary>
        /// <param name="grid"></param>
        public GridModel(GridModel grid)
        {
            SizeX = grid.SizeX;
            SizeY = grid.SizeY;
            _cells = new List<CellModel>(SizeX * SizeY);
            for (int i = 0; i < SizeX * SizeY; i++)
            {
                int x = i % SizeY;
                int y = i / SizeX;
                var cell = new CellModel(x, y);
                _cells.Add(cell);
                var sourceCell = grid[x,y];
                if (sourceCell.IsOccupied)
                {
                    var tileCopy = new TileModel(sourceCell.Tile);
                    tileCopy.Position = cell;
                }
            }
        }

        public int SizeX { get; private set; }
        public int SizeY { get; private set; }

        public CellModel this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= SizeX) return null;
                if (y < 0 || y >= SizeY) return null;
                return _cells[x + y * SizeX];
            }
        }

        public CellModel this[int i]
        {
            get
            {
                return this[i % SizeY, i / SizeX];
            }
        }

        private IList<CellModel> _cells;

        public IEnumerable<CellModel> Cells { get { return _cells; } }

        internal void Reset()
        {
            foreach (CellModel cell in Cells)
            {
                cell.Reset();
            }
            RaisePropertyChanged(string.Empty);
        }

        public CellModel[] AvailableCells()
        {
            return _cells
                .Where(x => x.Tile == null)
                .ToArray();
        }


        // Check if there are any cells available
        public bool AreCellsAvailable()
        {
            return _cells.Any(x => x.Tile == null);
        }

        public IEnumerable<int> TraversalsX(Direction direction)
        {
            if (direction == Direction.Right)
            {
                for (int pos = SizeX - 1; pos >= 0; pos--)
                    yield return pos;
            }
            else
            {
                for (int pos = 0; pos < SizeX; pos++)
                    yield return pos;
            }
        }

        public IEnumerable<int> TraversalsY(Direction direction)
        {
            if (direction == Direction.Down)
            {
                for (int pos = SizeY - 1; pos >= 0; pos--)
                    yield return pos;
            }
            else
            {
                for (int pos = 0; pos < SizeY; pos++)
                    yield return pos;
            }
        }
    }
}
