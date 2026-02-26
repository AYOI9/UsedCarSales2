using UsedCarsClient.Models;

namespace UsedCarsClient.ViewModels
{
    public class AddEditClientViewModel : ViewModelBase
    {
        public Client Client { get; }

        public AddEditClientViewModel(Client client)
        {
            Client = client;
        }
    }
}
