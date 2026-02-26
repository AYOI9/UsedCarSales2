using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UsedCarsClient.Models;
using UsedCarsClient.Services;
using UsedCarsClient.Utils;
using UsedCarsClient.Views;

namespace UsedCarsClient.ViewModels
{
    public class ClientsViewModel : ViewModelBase
    {
        private readonly ClientsService _service;

        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();

        private Client? _selectedClient;
        public Client? SelectedClient
        {
            get => _selectedClient;
            set { _selectedClient = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ClientsViewModel(string token)
        {
            _service = new ClientsService(token);
            RefreshCommand = new RelayCommand(async o => await Load());
            AddCommand = new RelayCommand(async o => await Add());
            EditCommand = new RelayCommand(async o => await Edit(), o => SelectedClient != null);
            DeleteCommand = new RelayCommand(async o => await Delete(), o => SelectedClient != null);

            Task.Run(Load);
        }

        private async Task Load()
        {
            try
            {
                var list = await _service.GetAll();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Clients.Clear();
                    foreach (var c in list) Clients.Add(c);
                });
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task Add()
        {
            var vm = new AddEditClientViewModel(new Client());
            var win = new AddEditClientView { DataContext = vm };
            if (win.ShowDialog() == true)
            {
                await _service.Add(vm.Client);
                await Load();
            }
        }

        private async Task Edit()
        {
            if (SelectedClient == null) return;
            var copy = new Client
            {
                ClientId = SelectedClient.ClientId,
                LastName = SelectedClient.LastName,
                FirstName = SelectedClient.FirstName,
                MiddleName = SelectedClient.MiddleName,
                City = SelectedClient.City,
                Address = SelectedClient.Address,
                Phone = SelectedClient.Phone
            };
            var vm = new AddEditClientViewModel(copy);
            var win = new AddEditClientView { DataContext = vm };
            if (win.ShowDialog() == true)
            {
                await _service.Update(vm.Client);
                await Load();
            }
        }

        private async Task Delete()
        {
            if (SelectedClient == null) return;
            if (MessageBox.Show("Удалить клиента?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _service.Delete(SelectedClient);
                await Load();
            }
        }
    }
}
