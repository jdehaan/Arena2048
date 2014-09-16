using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf2048.Logic.Bot;

namespace Wpf2048.ViewModels
{
    public class BotViewModel : ViewModelBase
    {
        public BotViewModel(IBot bot)
        {
            Bot = bot;
            SaveState = new Command(SaveExecute, SaveCanExecute);
            LoadState = new Command(LoadExecute, LoadCanExecute);
        }

        public Command LoadState { get; private set; }

        private bool LoadCanExecute()
        {
            return Bot.CanLearn;
        }

        private void LoadExecute()
        {
            Bot.Load();
        }

        public Command SaveState { get; private set; }

        private bool SaveCanExecute()
        {
            return Bot.CanLearn;
        }

        private void SaveExecute()
        {
            Bot.Save();
        }

        [Model]
        public IBot Bot
        { get; private set; }


    }
}
