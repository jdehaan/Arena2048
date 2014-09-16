using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf2048.Models;

namespace Wpf2048.Logic
{
    /// <summary>
    ///     Execution of player moves
    /// </summary>
    public class PlayerTurn
    {
        public PlayerTurn()
        {
        }

        public bool Move(GridModel grid, Direction direction, out int score)
        {
            score = 0;
            if (direction == Direction.None)
                return false;

            var vector = Vector.FromDirection(direction);
            var moved = false;
            var alreadyMerged = new List<TileModel>();

            // Traverse the grid in the right direction and move tiles
            foreach (int x in grid.TraversalsX(direction))
            {
                foreach (int y in grid.TraversalsY(direction))
                {
                    CellModel cell = grid[x, y];
                    if (cell.IsOccupied)
                    {
                        TileModel tile = cell.Tile;
                        CellModel nextCell;
                        CellModel farthestCell = grid.FindFarthestPosition(cell, vector, out nextCell);

                        TileModel nextTile = (nextCell == null) ? null : nextCell.Tile;

                        // Only one merger per row traversal?
                        if (nextTile != null && nextTile.Value == tile.Value && !alreadyMerged.Contains(nextTile))
                        {
                            var merged = new TileModel(tile, nextTile);
                            alreadyMerged.Add(merged);
                            tile.Position = null;
                            nextTile.Position = null;
                            merged.Position = nextCell;
                            moved = true;

                            // Update the score
                            score += (int)Math.Pow(2, merged.Value);
                        }
                        else
                        {
                            var oldPosition = tile.Position;
                            tile.Position = farthestCell;
                            if ((oldPosition.PosX != tile.Position.PosX)
                                || (oldPosition.PosY != tile.Position.PosY))
                                moved = true;
                        }
                    }
                }
            }
            return moved;
        }
    }
}
