using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UsedCarsClient.Models;
using UsedCarsClient.Services;
using UsedCarsClient.Utils;
using UsedCarsClient.Views;

namespace UsedCarsClient.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthService _authService = new AuthService();

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(async o => await Login());
            RegisterCommand = new RelayCommand(async o => await Register());
        }

        private async Task Login()
        {
            try
            {
                var person = new Person { Email = Email, Password = Password };
                var resp = await _authService.SignIn(person);
                if (resp == null)
                {
                    MessageBox.Show("Неверный логин или пароль");
                    return;
                }

                var main = new MainWindow(resp.access_token);
                Application.Current.MainWindow?.Close();
                Application.Current.MainWindow = main;
                main.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task Register()
        {
            try
            {
                var person = new Person { Email = Email, Password = Password };
                var res = await _authService.Register(person);
                if (res != null)
                {
                    MessageBox.Show("Пользователь зарегистрирован, теперь войдите");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
