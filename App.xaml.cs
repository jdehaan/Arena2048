using Catel.ApiCop;
using Catel.ApiCop.Listeners;
using Catel.IoC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

            //ServiceLocator.Default.RegisterTypesUsingDefaultNamingConvention();
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
