using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf2048.Models;

namespace Wpf2048.Logic.Bot
{
    public abstract class BaseBot : IBot
    {
        /// <summary>
        /// http://stackoverflow.com/questions/22342854/what-is-the-optimal-algorithm-for-the-game-2048
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        protected BaseBot(String name, String description, bool canLearn = false)
        {
            Name = name;
            Description = description;
            CanLearn = canLearn;
            PlayerTurn = new PlayerTurn();
        }

        public override string ToString()
        {
            return Name;
        }

        public String Name { get; private set; }

        public String Description { get; private set; }

        protected PlayerTurn PlayerTurn { get; private set; }

        protected Direction[] ValidDirections(GridModel grid)
        {
            Dictionary<Direction, bool> d = new Dictionary<Direction, bool>();
            foreach (Direction direction in Vector.Directions)
            {
                d[direction] = CheckDirection(grid, direction);
            }
            return d.Where(x => x.Value).Select(x => x.Key).ToArray();
        }

        private static bool CheckDirection(GridModel grid, Direction direction)
        {
            var vector = Vector.FromDirection(direction);
            foreach (int x in grid.TraversalsX(direction))
            {
                foreach (int y in grid.TraversalsY(direction))
                {
                    var cell1 = grid[x, y];
                    var cell2 = grid[x + vector.x, y + vector.y];
                    if (cell1 != null && cell2 != null)
                    {
                        if (cell1.IsOccupied && cell2.IsAvailable)
                        {
                            return true;
                        }
                        if (cell1.IsOccupied && cell2.IsOccupied)
                        {
                            if (cell1.Tile.Value == cell2.Tile.Value)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public abstract Direction ComputeNextMove(GridModel grid);

        public virtual void Save()
        {
        }

        public virtual void Load()
        {
        }

        public virtual void TrainNextMove(GridModel grid, Direction direction)
        {
        }

        public bool CanLearn
        {
            get;
            private set;
        }
    }
}
