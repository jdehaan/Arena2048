using Catel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf2048.Logic.Bot;

namespace Wpf2048.Services
{
    public class BotsManager : IBotsManager, IService, IDisposable
    {
        public string Name
        {
            get { return "Bot Manager"; }
        }

        public BotsManager()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(BotsManager).Assembly));
            _container = new CompositionContainer(catalog);
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }

        [ImportMany(typeof(IBot))]
        public IEnumerable<IBot> Bots { get; set; }

        private CompositionContainer _container;

        public void Dispose()
        {
            _container.Dispose();
        }

    }
}
