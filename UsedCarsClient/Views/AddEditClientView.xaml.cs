using System.Windows;

namespace UsedCarsClient.Views
{
    public partial class AddEditClientView : Window
    {
        public AddEditClientView()
        {
            InitializeComponent();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
