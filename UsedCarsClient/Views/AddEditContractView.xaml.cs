using System.Windows;

namespace UsedCarsClient.Views
{
    public partial class AddEditContractView : Window
    {
        public AddEditContractView()
        {
            InitializeComponent();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
