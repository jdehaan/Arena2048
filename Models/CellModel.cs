using Catel.Data;
using System;
using System.Diagnostics;

namespace Wpf2048.Models
{
    [DebuggerDisplay("({PosX},{PosY}) {Tile}")]
    public class CellModel : ModelBase
    {
        public CellModel(int x, int y)
        {
            PosX = x;
            PosY = y;
        }

        public int PosX { get; private set; }
        public int PosY { get; private set; }

        public TileModel Tile { get; set; }

        internal void Reset()
        {
            Tile = null;
        }

        // Check if the specified cell is taken
        public bool IsAvailable
        {
            get
            {
                return Tile == null;
            }
        }

        public bool IsOccupied
        {
            get
            {
                return Tile != null;
            }
        }

        /// <summary>
        /// Returns the value of the tile if this cell is occupied, 0 otherwise
        /// </summary>
        public int Value
        {
            get
            {
                return Tile != null ? Tile.Value : 0;
            }
        }

    }
}
