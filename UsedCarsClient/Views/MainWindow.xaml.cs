using System.Windows;
using UsedCarsClient.ViewModels;

namespace UsedCarsClient.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(string token)
        {
            InitializeComponent();
            DataContext = new NavigationViewModel(token);
        }
    }
}
