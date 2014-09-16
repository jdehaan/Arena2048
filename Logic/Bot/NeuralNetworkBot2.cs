using AForge.Neuro;
using AForge.Neuro.Learning;
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
    /// <summary>
    /// https://www.youtube.com/watch?v=jsVnuw5Bv0s
    /// </summary>
    [Export(typeof(IBot))]
    public class NeuralNetworkBot2 : BaseBot
    {
        public NeuralNetworkBot2()
            : base(
                "Neural Network Bot II",
                "Runs a neural network neural network to directly calculate outputs for each move",
                true)
        {
            _network = new ActivationNetwork(new SigmoidFunction(), 4*4, 4*4, 2);
            //_network.Randomize();
            Load();
            
            _teacher = new BackPropagationLearning(_network);
            _teacher.LearningRate = 0.05;
            _teacher.Momentum = 0.05;
        }

        private BackPropagationLearning _teacher;

        public override Direction ComputeNextMove(GridModel grid)
        {
            var input = GridToInput(grid);
            var output = _network.Compute(input);

            // if output is impossible teach->0
            // if output has many merges teach good
            // learning mode from human

            Direction result = OutputToDirection(output);
            var validMoves = ValidDirections(grid);
            if (validMoves.Contains(result))
                return result;

            foreach (Direction direction in validMoves)
            {
                TrainNextMove(grid, direction);
            }
            /*
            int validMoveCount = validMoves.Count();
            if (validMoveCount > 0)
                TrainNextMove(grid, validMoves.Last());
             */
            return Direction.None;
        }

        private static double[] GridToInput(GridModel grid)
        {
            var input = new double[16];
            for (int i = 0; i < 16; i++)
                input[i] = grid[i].Value;
            double max = input.Max();
            if (max > 0)
            {
                // scale all inputs
                for (int i = 0; i < 16; i++)
                    input[i] = input[i] / max;
            }
            return input;
        }

        private static Direction OutputToDirection(double[] output)
        {
            //up    0 1
            //down  0 0
            //left  1 0
            //right 1 1
            double threshold = 0.5;
            int a = (output[0] > threshold) ? 1 : 0;
            int b = (output[1] > threshold) ? 2 : 0;
            int val = a + b;
            switch (val)
            {
                case 0: return Direction.Down;
                case 1: return Direction.Up;
                case 2: return Direction.Left;
                case 3: return Direction.Right;
            }
            return Direction.None;
        }

        private static double[] DirectionToOutput(Direction direction)
        {
            const double high = 0.9;
            const double low = 0.1;
            switch (direction)
            {
                case Direction.Right:
                    return new double[] { high, high };
                case Direction.Left:
                    return new double[] { low, high };
                case Direction.Up:
                    return new double[] { high, low };
                case Direction.Down:
                    return new double[] { low, low };
                default:
                    return new double[] { 0.5, 0.5 };
            }
        }

        public override void Load()
        {
            _network = ActivationNetwork.Load("NeuralNetworkBot2.dat") as ActivationNetwork;
        }

        public override void Save()
        {
            _network.Save("NeuralNetworkBot2.dat");
        }

        public override void TrainNextMove(GridModel grid, Direction direction)
        {
            var input = GridToInput(grid);
            var output = DirectionToOutput(direction);
            double error = _teacher.Run(input, output);

            // also train rotated boards at the same time!
            input = RotateLeft(input);
            output = DirectionToOutput(RotateLeft(direction));
            error = Math.Max(_teacher.Run(input, output), error);

            input = RotateLeft(input);
            output = DirectionToOutput(RotateLeft(direction));
            error = Math.Max(_teacher.Run(input, output), error);

            input = RotateLeft(input);
            output = DirectionToOutput(RotateLeft(direction));
            error = Math.Max(_teacher.Run(input, output), error);
            Console.WriteLine("Error: {0}", error);
        }

        private double[] RotateLeft(double[] input)
        {
            double[] result = new double[input.Length];
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    result[(3-x)*4 + y] = input[y*4 + x];
                }
            }
            return result;
        }

        private Direction RotateLeft(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Right;
            }
            return Direction.None;
        }

        private Direction RotateRight(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.Right;
            }
            return Direction.None;
        }

        private ActivationNetwork _network;
    }
}
