using Catel.Fody;
using Catel.MVVM;
using Wpf2048.Models;

namespace Wpf2048.ViewModels
{
    public class GridViewModel : ViewModelBase
    {
        public GridViewModel(GridModel model)
        {
            Model = model;
        }

        [Model]
        [Expose("Cells")]
        public GridModel Model { get; private set; }
    }
}
