using System.Collections.Generic;
using System.Collections.ObjectModel;
using UsedCarsClient.Models;

namespace UsedCarsClient.ViewModels
{
    public class AddEditContractViewModel : ViewModelBase
    {
        public Contract Contract { get; }

        public ObservableCollection<Client> Clients { get; }

        public AddEditContractViewModel(Contract contract, IEnumerable<Client> clients)
        {
            Contract = contract;
            Clients = new ObservableCollection<Client>(clients);

            // Чтобы ComboBox не был пустым при добавлении
            if (Contract.ClientId == 0 && Clients.Count > 0)
            {
                Contract.ClientId = Clients[0].ClientId;
            }
        }
    }
}
