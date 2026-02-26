using System.Windows.Input;
using UsedCarsClient.Utils;

namespace UsedCarsClient.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        private readonly string _token;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; }
        public ICommand ClientsCommand { get; }
        public ICommand ContractsCommand { get; }

        public NavigationViewModel(string token)
        {
            _token = token;

            HomeCommand = new RelayCommand(o => CurrentView = new HomeViewModel());
            ClientsCommand = new RelayCommand(o => CurrentView = new ClientsViewModel(_token));
            ContractsCommand = new RelayCommand(o => CurrentView = new ContractsViewModel(_token));

            CurrentView = new HomeViewModel();
        }
    }
}
