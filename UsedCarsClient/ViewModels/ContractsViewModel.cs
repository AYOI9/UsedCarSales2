using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UsedCarsClient.Models;
using UsedCarsClient.Services;
using UsedCarsClient.Utils;
using UsedCarsClient.Views;

namespace UsedCarsClient.ViewModels
{
    public class ContractsViewModel : ViewModelBase
    {
        private readonly ContractsService _contractsService;
        private readonly ClientsService _clientsService;

        private List<Client> _clientsCache = new();

        public ObservableCollection<Contract> Contracts { get; set; } = new ObservableCollection<Contract>();

        private Contract? _selectedContract;
        public Contract? SelectedContract
        {
            get => _selectedContract;
            set { _selectedContract = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ContractsViewModel(string token)
        {
            _contractsService = new ContractsService(token);
            _clientsService = new ClientsService(token);

            RefreshCommand = new RelayCommand(async o => await Load());
            AddCommand = new RelayCommand(async o => await Add());
            EditCommand = new RelayCommand(async o => await Edit(), o => SelectedContract != null);
            DeleteCommand = new RelayCommand(async o => await Delete(), o => SelectedContract != null);

            Task.Run(Load);
        }

        private async Task Load()
        {
            try
            {
                _clientsCache = await _clientsService.GetAll();
                var contracts = await _contractsService.GetAll();

                var dict = _clientsCache.ToDictionary(c => c.ClientId);
                foreach (var c in contracts)
                {
                    c.Client = dict.TryGetValue(c.ClientId, out var cl) ? cl : null;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Contracts.Clear();
                    foreach (var c in contracts) Contracts.Add(c);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task Add()
        {
            if (_clientsCache.Count == 0)
            {
                MessageBox.Show("Сначала добавьте хотя бы одного клиента.");
                return;
            }

            var contract = new Contract
            {
                ClientId = _clientsCache[0].ClientId,
                ContractDate = DateTime.Today,
                ProductionDate = DateTime.Today,
                CarMake = string.Empty,
                Mileage = 0
            };

            var vm = new AddEditContractViewModel(contract, _clientsCache);
            var win = new AddEditContractView { DataContext = vm };
            if (win.ShowDialog() == true)
            {
                await _contractsService.Add(vm.Contract);
                await Load();
            }
        }

        private async Task Edit()
        {
            if (SelectedContract == null) return;

            var copy = new Contract
            {
                ContractId = SelectedContract.ContractId,
                ClientId = SelectedContract.ClientId,
                ContractDate = SelectedContract.ContractDate,
                CarMake = SelectedContract.CarMake,
                CarModel = SelectedContract.CarModel,
                ProductionDate = SelectedContract.ProductionDate,
                Mileage = SelectedContract.Mileage,
                SaleDate = SelectedContract.SaleDate,
                SalePrice = SelectedContract.SalePrice,
                Commission = SelectedContract.Commission
            };

            var vm = new AddEditContractViewModel(copy, _clientsCache);
            var win = new AddEditContractView { DataContext = vm };
            if (win.ShowDialog() == true)
            {
                await _contractsService.Update(vm.Contract);
                await Load();
            }
        }

        private async Task Delete()
        {
            if (SelectedContract == null) return;
            if (MessageBox.Show("Удалить договор?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _contractsService.Delete(SelectedContract);
                await Load();
            }
        }
    }
}
