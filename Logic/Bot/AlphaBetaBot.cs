using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf2048.Models;

namespace Wpf2048.Logic.Bot
{
    //[Export(typeof(IBot))]
    public class AlphaBetaBot : BaseBot
    {
        public AlphaBetaBot()
            : base("Alpha Beta Bot", "Runs an alpha beta analysis with a mixed weighted metrics evaluation")
        {
            _minSearchTime = 1000;
        }

        public override Direction ComputeNextMove(GridModel grid)
        {
            Direction bestMove = Direction.None;
            double bestScore = double.NegativeInfinity;
            var timer = new Stopwatch();
            timer.Start();
            int depth = 2;
            do
            {
                foreach (var direction in Vector.Directions)
                {
                    var score = AlphaBetaSearch(grid, depth, true, Double.NegativeInfinity, Double.PositiveInfinity);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = direction;
                    }
                }
                depth++;
            } while (depth < 3);//timer.ElapsedMilliseconds < _minSearchTime);
            return bestMove;
        }

        private long _minSearchTime;

        private static double StaticEvaluation(GridModel grid)
        {
            double
                smoothWeight = 0.1,
                monoWeight   = 0.1,
                //islandWeight = 0.0,
                mono2Weight = 1.0,
                emptyWeight = 2.7,
                maxWeight = 1.0;

            return
                //  grid.Smoothness() * smoothWeight
                //grid.Monotonicity() * monoWeight
                //- grid.islands() * islandWeight
                + grid.Monotonicity2() * mono2Weight
                //+ Math.Log(grid.EmptyCells()) * emptyWeight
                //+ grid.MaxValue() * maxWeight
                ;
        }

        private double Maximizer(GridModel grid, int depth, double alpha, double beta)
        {
            double bestScore = alpha;
            foreach (var direction in Vector.Directions) 
            {
                var newGrid = new GridModel(grid);
                int deltaScore;
                if (PlayerTurn.Move(newGrid, direction, out deltaScore)) 
                {
                    if (depth == 0) 
                    {
                        // leaf node, static evaluation
                        return StaticEvaluation(newGrid);
                    }
                    else
                    {
                        alpha = Math.Max(alpha, AlphaBetaSearch(newGrid,  depth-1, false, alpha, beta));
                    }

                    if (alpha > beta) // pruning
                    {
                        return beta;
                    }
                }
            }
            return alpha;
        }

        private double Minimizer(GridModel grid, int depth, double alpha, double beta)
        {
            // computer's turn, we'll do heavy pruning to keep the branching factor low
            var availableCells = grid.AvailableCells().ToList();

            // try out all combinations possible
            foreach (var value in new int[] { 1, 2 })
            {
                foreach (CellModel cell in availableCells)
                {
                    var tile = new TileModel(value);
                    tile.Position = cell;
                    beta = Math.Min(beta, AlphaBetaSearch(grid, depth, true, alpha, beta));
                    tile.Position = null;

                    if (beta < alpha) // pruning
                    {
                        return alpha;
                    }
                }
            }

            /*
            // try a 2 and 4 in each cell and measure how annoying it is
            // with metrics from eval
            //var scores = { 2: [], 4: [] };
            foreach (var value in new int[] { 1, 2 })
            {
                foreach (CellModel cell in availableCells)
                {
                    scores[value].push(null);

                    var tile = new TileModel(value);
                    tile.Position = cell;

                    scores[value][i] = -grid.Smoothness() + grid.Islands();

                    tile.Position = null;
                }
            }

            // now just pick out the most annoying moves
            var candidates = new List<CellModel>();
            var maxScore = Math.Max(Math.Max(null, scores[2]), Math.Max(null, scores[4]));
            foreach (var value in new int[] { 1, 2 })
            {
                for (var i = 0; i < scores[value].length; i++)
                {
                    if (scores[value][i] >= maxScore)
                    {
                        candidates.Add(cells[i]);
                    }
                }
            }

            // search on each candidate
            foreach (CellModel position in candidates)
            {
      
            }
             */
            return beta;
        }

        private double AlphaBetaSearch(GridModel grid, int depth, bool playerTurn, double alpha, double beta) 
        {
            if (playerTurn)
            {
                return Maximizer(grid, depth, alpha, beta);
            }
            else
            {
                return Minimizer(grid, depth, alpha, beta);
            }
        }
    }
}
