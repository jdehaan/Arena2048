using Catel.Windows;
using System.Windows.Input;
using Wpf2048.ViewModels;

namespace Wpf2048.Views
{
    public partial class MainWindow : DataWindow
    {
        public MainWindow()
            : base(DataWindowMode.Custom)
        {
            InitializeComponent();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                case Key.Left:
                    (ViewModel as MainViewModel).MoveLeft.Execute();
                    e.Handled = true;
                    break;
                case Key.D:
                case Key.Right:
                    (ViewModel as MainViewModel).MoveRight.Execute();
                    e.Handled = true;
                    break;
                case Key.W:
                case Key.Up:
                    (ViewModel as MainViewModel).MoveUp.Execute();
                    e.Handled = true;
                    break;
                case Key.S:
                case Key.Down:
                    (ViewModel as MainViewModel).MoveDown.Execute();
                    e.Handled = true;
                    break;
                default:
                    base.OnKeyDown(e);
                    break;
            }
        }

    }
}
