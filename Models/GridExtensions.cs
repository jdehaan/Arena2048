using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf2048.Models
{
    public static class GridExtensions
    {
        public static CellModel FindFarthestPosition(this GridModel grid, CellModel cell, Direction direction, out CellModel next)
        {
            return FindFarthestPosition(grid, cell, Vector.FromDirection(direction), out next);
        }

        public static CellModel FindFarthestPosition(this GridModel grid, CellModel cell, Vector vector, out CellModel next)
        {
            CellModel previous;

            // Progress towards the vector direction until an obstacle is found
            do
            {
                previous = cell;
                cell = grid[previous.PosX + vector.x, previous.PosY + vector.y];
            }
            while (cell != null && cell.IsAvailable);

            next = cell; // Used to check if a merge is required
            return previous;
        }

        public static CellModel FindNextCellInDirection(this GridModel grid, CellModel cell, Direction direction)
        {
            var vector = Vector.FromDirection(direction);
            CellModel previous;
            do
            {
                // Progress towards the vector direction until an obstacle is found
                previous = cell;
                cell = grid[previous.PosX + vector.x, previous.PosY + vector.y];
            }
            while (cell != null && cell.IsAvailable);

            return cell;
        }

        public static double EmptyCells(this GridModel grid)
        {
            return grid.Cells
                .Count(x => x.IsAvailable);
        }

        public static double MaxValue(this GridModel grid)
        {
            return grid.Cells
                .Where(x => x.IsOccupied)
                .Select(x => x.Value)
                .DefaultIfEmpty()
                .Max();
        }

        public static double AverageValue(this GridModel grid)
        {
            return grid.Cells
                .Where(x => x.IsOccupied)
                .Select(x => x.Value)
                .DefaultIfEmpty()
                .Average();
        }

        // counts the number of isolated groups. 
        public static double Islandsfunction(this GridModel grid)
        {
          //var self = this;
          //var mark = function(x, y, value) {
          //  if (x >= 0 && x <= 3 && y >= 0 && y <= 3 &&
          //      self.cells[x][y] &&
          //      self.cells[x][y].value == value &&
          //      !self.cells[x][y].marked ) {
          //    self.cells[x][y].marked = true;
      
          //    for (direction in [0,1,2,3]) {
          //      var vector = self.getVector(direction);
          //      mark(x + vector.x, y + vector.y, value);
          //    }
          //  }
          //}

          //var islands = 0;

          //for (var x=0; x<4; x++) {
          //  for (var y=0; y<4; y++) {
          //    if (this.cells[x][y]) {
          //      this.cells[x][y].marked = false
          //    }
          //  }
          //}
          //for (var x=0; x<4; x++) {
          //  for (var y=0; y<4; y++) {
          //    if (this.cells[x][y] &&
          //        !this.cells[x][y].marked) {
          //      islands++;
          //      mark({ x:x, y:y }, this.cells[x][y].value);
          //    }
          //  }
          //}
  
          //return islands;
            return 0;
        }

        // measures how smooth the grid is (as if the values of the pieces
        // were interpreted as elevations). Sums of the pairwise difference
        // between neighboring tiles (in log space, so it represents the
        // number of merges that need to happen before they can merge). 
        // Note that the pieces can be distant
        public static double Smoothness(this GridModel grid)
        {
            var directions = new Direction[] { Direction.Right, Direction.Up };
            double smoothness = 0;
            for (var x=0; x<grid.SizeX; x++)
            {
                for (var y=0; y<grid.SizeY; y++) 
                {
                    CellModel cell = grid[x,y];
                    if (cell.IsOccupied) 
                    {
                        var value = cell.Tile.Value;
                        foreach (var direction in directions) 
                        {
                            CellModel targetCell = grid.FindNextCellInDirection(cell, direction);
                            if (targetCell != null)
                            {
                                var targetValue = targetCell.Value;
                                if (targetValue > 0)
                                {
                                    smoothness -= Math.Abs(value - targetValue);
                                }
                            }
                        }
                    }
                }
            }
            return smoothness;
        }
        /*
Grid.prototype.monotonicity = function() {
  var self = this;
  var marked = [];
  var queued = [];
  var highestValue = 0;
  var highestCell = {x:0, y:0};
  for (var x=0; x<4; x++) {
    marked.push([]);
    queued.push([]);
    for (var y=0; y<4; y++) {
      marked[x].push(false);
      queued[x].push(false);
      if (this.cells[x][y] &&
          this.cells[x][y].value > highestValue) {
        highestValue = this.cells[x][y].value;
        highestCell.x = x;
        highestCell.y = y;
      }
    }
  }

  increases = 0;
  cellQueue = [highestCell];
  queued[highestCell.x][highestCell.y] = true;
  markList = [highestCell];
  markAfter = 1; // only mark after all queued moves are done, as if searching in parallel

  var markAndScore = function(cell) {
    markList.push(cell);
    var value;
    if (self.cellOccupied(cell)) {
      value = Math.log(self.cellContent(cell).value) / Math.log(2);
    } else {
      value = 0;
    }
    for (direction in [0,1,2,3]) {
      var vector = self.getVector(direction);
      var target = { x: cell.x + vector.x, y: cell.y+vector.y }
      if (self.withinBounds(target) && !marked[target.x][target.y]) {
        if ( self.cellOccupied(target) ) {
          targetValue = Math.log(self.cellContent(target).value ) / Math.log(2);
          if ( targetValue > value ) {
            //console.log(cell, value, target, targetValue);
            increases += targetValue - value;
          }
        } 
        if (!queued[target.x][target.y]) {
          cellQueue.push(target);
          queued[target.x][target.y] = true;
        }
      }
    }
    if (markAfter == 0) {
      while (markList.length > 0) {
        var cel = markList.pop();
        marked[cel.x][cel.y] = true;
      }
      markAfter = cellQueue.length;
    }
  }

  while (cellQueue.length > 0) {
    markAfter--;
    markAndScore(cellQueue.shift())
  }

  return -increases;
}
        */
        // measures how monotonic the grid is. This means the values of the tiles are strictly increasing
        // or decreasing in both the left/right and up/down directions
        public static double Monotonicity2(this GridModel grid)
        {
            // scores for all four directions
            var totals = new Dictionary<Direction,double>();
            totals[Direction.Up] = 0;
            totals[Direction.Down] = 0;
            totals[Direction.Left] = 0;
            totals[Direction.Right] = 0;

            // up/down direction
            for (var x=0; x<grid.SizeX; x++) 
            {
                var next = 1;
                var currentValue = grid[x, 0].Value;
                while (next < grid.SizeY)
                {
                    var nextValue = grid[x, next].Value;
                    if (nextValue > 0)
                    {
                        if (currentValue > nextValue)
                        {
                            totals[Direction.Up] += nextValue - currentValue;
                        }
                        else if (nextValue > currentValue)
                        {
                            totals[Direction.Down] += currentValue - nextValue;
                        }
                        currentValue = nextValue;
                    }
                    next++;
                }
            }

            // left/right direction
            for (var y=0; y<grid.SizeY; y++)
            {
                var next = 1;
                var currentValue = grid[0, y].Value;
                while (next < grid.SizeX) 
                {
                    var nextValue = grid[next, y].Value;
                    if (nextValue > 0)
                    {
                        if (currentValue > nextValue)
                        {
                            totals[Direction.Left] += nextValue - currentValue;
                        }
                        else if (nextValue > currentValue)
                        {
                            totals[Direction.Right] += currentValue - nextValue;
                        }
                        currentValue = nextValue;
                    }
                    next++;
                }
            }

            return  Math.Max(totals[Direction.Left], totals[Direction.Right]) +
                    Math.Max(totals[Direction.Up], totals[Direction.Down]);
        }

        public static double Monotonicity(this GridModel grid)
        {
            IList<CellModel> cells = new List<CellModel>();
            for (var y = 0; y < grid.SizeY; y++)
            {
                for (var x = 0; x < grid.SizeX; x++)
                {
                    var xi = ((y & 1) == 0) ? x : grid.SizeX - x - 1;
                    cells.Add(grid[xi, y]);
                }
            }

            CellModel previous = null;
            double result = 0;
            var lastNonEmptyCell = cells.Where(x => x.IsOccupied).Reverse().Skip(1).FirstOrDefault();
            bool insideCells = lastNonEmptyCell != null;
            foreach (CellModel cell in cells)
            {
                if (previous != null)
                {
                    if (cell.Value - previous.Value < 0)
                    {
                        var delta = cell.Value - previous.Value;
                        result -= delta*delta;
                    }
                    if (cell.Value == 0 && insideCells)
                        result -= 1000;
                    if (cell == lastNonEmptyCell)
                        insideCells = false;
                }
                previous = cell;
            }

            return result;
        }
        /*
// WIP. trying to favor top-heavy distributions (force consolidation of higher value tiles)
/*
Grid.prototype.valueSum = function() {
  var valueCount = [];
  for (var i=0; i<11; i++) {
    valueCount.push(0);
  }

  for (var x=0; x<4; x++) {
    for (var y=0; y<4; y++) {
      if (this.cellOccupied(this.indexes[x][y])) {
        valueCount[Math.log(this.cellContent(this.indexes[x][y]).value) / Math.log(2)]++;
      }
    }
  }

  var sum = 0;
  for (var i=1; i<11; i++) {
    sum += valueCount[i] * Math.pow(2, i) + i;
  }

  return sum;
}
*/
    }
}
