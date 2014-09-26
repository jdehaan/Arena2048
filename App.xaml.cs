using Catel.ApiCop;
using Catel.ApiCop.Listeners;
using System;
using System.Linq;
using System.Windows;
using Catel.IoC;
using Catel.Logging;
using Wpf2048.Services;

namespace Wpf2048
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

        	#if DEBUG
        	LogManager.AddDebugListener();
        	#endif
            
        	//TODO: does not work, reason to investigate....
            //ServiceLocator.Default.RegisterTypesUsingDefaultNamingConvention();
            // doing it manually instead like for Catel 3.9
            ServiceLocator.Default.RegisterInstance(typeof(IBotsManager), new BotsManager());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var apiCopListener = new ConsoleApiCopListener();
            ApiCopManager.AddListener(apiCopListener);
            ApiCopManager.WriteResults();

            base.OnExit(e);
        }
    }
}
