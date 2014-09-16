using Catel.Data;
using System;
using System.Diagnostics;

namespace Wpf2048.Models
{
    [DebuggerDisplay("({Value}")]
    public class TileModel : ModelBase
    {
        public TileModel(TileModel tile1, TileModel tile2)
        {
            if (tile1.Value != tile2.Value)
                throw new Exception("Only tiles with same value can be merged");
            Value = tile1.Value + 1;
            MergedFrom1 = tile1;
            MergedFrom2 = tile2;
        }

        public TileModel(TileModel source)
        {
            Value = source.Value;
        }

        public TileModel(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public TileModel MergedFrom1 { get; private set; }
        public TileModel MergedFrom2 { get; private set; }

        public int Value { get; private set; }

        private CellModel _position;
        
        public CellModel Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (_position == null || _position != value)
                {
                    if (value != null && value.IsOccupied)
                        throw new Exception("Cannot move a tile to an occupied cell");

                    if (_position != null)
                        _position.Tile = null;
                    _position = value;
                    if (_position != null)
                        _position.Tile = this;
                }
            }
        }
    }
}
