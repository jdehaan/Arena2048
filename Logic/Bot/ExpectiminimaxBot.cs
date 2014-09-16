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
    public class ExpectiminimaxBot : BaseBot
    {
        public ExpectiminimaxBot()
            : base("Expectiminimax Bot", "Runs an probabilistic minimax search")
        {
        }

        public override Direction ComputeNextMove(GridModel grid)
        {
            return Direction.Up;
        }

    }
}
