using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf2048.Logic.Bot;

namespace Wpf2048.Services
{
    public interface IBotsManager
    {
        IEnumerable<IBot> Bots { get; }
    }
}
