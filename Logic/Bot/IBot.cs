using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf2048.Models;

namespace Wpf2048.Logic.Bot
{
    public interface IBot
    {
        String Name { get; }
        String Description { get; }
        Direction ComputeNextMove(GridModel grid);

        bool CanLearn { get; }
        void Save();
        void Load();
        void TrainNextMove(GridModel grid, Direction direction);
    }
}
