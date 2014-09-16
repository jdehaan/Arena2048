using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf2048.Models
{
    public struct Vector
    {
        public int x;
        public int y;


        // Get the vector representing the chosen direction
        public static Vector FromDirection(Direction direction)
        {
            // Vectors representing tile movement
            switch (direction)
            {
                case Direction.Up:
                    return _vectorUp;
                case Direction.Right:
                    return _vectorRight;
                case Direction.Down:
                    return _vectorDown;
                case Direction.Left:
                    return _vectorLeft;
            }
            throw new ArgumentOutOfRangeException("direction");
        }

        public static IEnumerable<Direction> Directions
        {
            get
            {
                yield return Direction.Up;
                yield return Direction.Right;
                yield return Direction.Down;
                yield return Direction.Left;
            }
        }

        private static Vector _vectorUp = new Vector { x = 0, y = -1 }; // Up
        private static Vector _vectorRight = new Vector { x = 1, y = 0 }; // Right
        private static Vector _vectorDown = new Vector { x = 0, y = 1 }; // Down
        private static Vector _vectorLeft = new Vector { x = -1, y = 0 }; // Left
    }
}
