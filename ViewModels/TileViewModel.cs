using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Wpf2048.Models;

namespace Wpf2048.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        public TileViewModel(TileModel model)
        {
            Model = model;
        }

        public Brush TextColor
        {
            get
            {
                return _appearance[Model.Value].TextColor;
            }
        }

        public Brush BackColor
        {
            get
            {
                return _appearance[Model.Value].BackColor;
            }
        }

        public String Text
        {
            get
            {
                return _appearance[Model.Value].Text;
            }
        }

        [Model]
        public TileModel Model { get; private set; }

        private void OnValueChanged()
        {
            RaisePropertyChanged("TextColor");
            RaisePropertyChanged("BackColor");
            RaisePropertyChanged("Text");
        }

        static TileViewModel()
        {
            String[][] settings = new String[][]{
                new [] { "1",      "#eee4da", "#000000" },
                new [] { "2",      "#eee4da", "#000000" },
                new [] { "4",      "#ede0c8", "#000000" },
                new [] { "8",      "#f2b179", "#f9f6f2" },
                new [] { "16",     "#f59563", "#f9f6f2" },
                new [] { "32",     "#f67c5f", "#f9f6f2" },
                new [] { "64",     "#f65e3b", "#f9f6f2" },
                new [] { "128",    "#edcf72", "#f9f6f2" },
                new [] { "256",    "#edcc61", "#f9f6f2" },
                new [] { "512",    "#edc850", "#f9f6f2" },
                new [] { "1024",   "#edc53f", "#f9f6f2" },
                new [] { "2048",   "#edc22e", "#f9f6f2" },
                new [] { "4096",   "#edc22e", "#f9f6f2" },
                new [] { "8192",   "#edc22e", "#f9f6f2" },
                new [] { "16384",  "#edc22e", "#f9f6f2" },
                new [] { "32768",  "#edc22e", "#f9f6f2" },
            };
            _appearance = new Dictionary<int,TileAppearance>();
            int i = 0;
            foreach (String[] setting in settings)
            {
                _appearance[i] = new TileAppearance(
                        i.ToString(),
                        //setting[0],
                        setting[1],
                        setting[2]);
                i++;
            }
        }

        public static Dictionary<int, TileAppearance> _appearance;
    }
}
