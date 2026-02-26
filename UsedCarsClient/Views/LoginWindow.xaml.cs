using System.Windows;
using UsedCarsClient.ViewModels;

namespace UsedCarsClient.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = pwdBox.Password;
                vm.LoginCommand.Execute(null);
            }
        }
    }
}
