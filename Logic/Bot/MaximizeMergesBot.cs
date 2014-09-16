using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf2048.Models;

namespace Wpf2048.Logic.Bot
{
    //[Export(typeof(IBot))]
    public class MaximizeMergesBot : BaseBot
    {
        public MaximizeMergesBot()
            : base("Maximize merges", "Maximizes the amount of merges for the next move")
        {

        }

        private Random _rand = new Random();

        public override Direction ComputeNextMove(GridModel grid)
        {
            int maxMerges = 0;
            var dirs = ValidDirections(grid);
            Direction move = dirs[_rand.Next(dirs.Length)];
            foreach (Direction direction in Vector.Directions)
            {
                int merges = CountMerges(direction, grid);
                if (merges > maxMerges)
                {
                    maxMerges = merges;
                    move = direction;
                }
            }
            return move;
        }

        private int CountMerges(Direction direction, GridModel grid)
        {
            var vector = Vector.FromDirection(direction);
            int merges = 0;
            foreach (int x in grid.TraversalsX(direction))
            {
                foreach (int y in grid.TraversalsY(direction))
                {
                    var cell1 = grid[x, y];
                    var cell2 = grid[x + vector.x, y + vector.y];
                    if (cell1 != null && cell2 != null)
                    {
                        if (cell1.IsOccupied && cell2.IsOccupied)
                        {
                            if (cell1.Tile.Value == cell2.Tile.Value)
                                merges++;
                        }
                    }
                }
            }
            return merges;
        }
    }
}
