using Catel.Fody;
using Catel.MVVM;
using System;
using System.Windows;
using Wpf2048.Models;

namespace Wpf2048.ViewModels
{
    public class CellViewModel : ViewModelBase
    {
        public CellViewModel(CellModel model)
        {
            Model = model;
        }

        [Model]
        [Expose("Tile")]
        public CellModel Model { get; private set; }

        private void OnTileChanged()
        {
            RaisePropertyChanged("Visibility");
        }

        public Visibility Visibility
        {
            get
            {
                if (Model.Tile == null)
                    return Visibility.Hidden;
                return Visibility.Visible;
            }
        }
    }
}
