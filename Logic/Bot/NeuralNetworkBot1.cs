using AForge.Neuro;
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
    public class NeuralNetworkBot1 : BaseBot
    {
        public NeuralNetworkBot1()
            : base(
                "Neural Network Bot I",
                "Runs a neural network neural network to compute a score out of different simulated moves",
                true)
        {
            _network = new ActivationNetwork(new SigmoidFunction(), 4*4, 4*4, 1);
            //_network.Randomize();
            //ActivationNetwork.Load();
            //_network.Save();
        }

        public override Direction ComputeNextMove(GridModel grid)
        {
            Direction bestMove = Direction.None;
            double bestScore = double.NegativeInfinity;
            foreach (var direction in Vector.Directions)
            {
                var score = StaticEvaluation(grid, direction);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = direction;
                }
            }
            return bestMove;
        }

        // use a neural network to compute a score out of different simulated moves
        private double StaticEvaluation(GridModel grid, Direction direction)
        {
            var newGrid = new GridModel(grid);
            int score;
            if (PlayerTurn.Move(newGrid, direction, out score))
            {
                var input = new double[16];
                for (int i = 0; i < 16; i++)
                    input[i] = newGrid[i].Value;
                var output = _network.Compute(input);
                return output[0];
            }
            return double.NegativeInfinity;
        }

        private ActivationNetwork _network;
    }
}
