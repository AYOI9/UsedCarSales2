using System;
using System.Text.Json.Serialization;

namespace UsedCarsClient.Models
{
    // Должен совпадать по полям с UsedCarsAPI.Models.Contract
    public class Contract
    {
        public int ContractId { get; set; }

        public int ClientId { get; set; }

        public DateTime ContractDate { get; set; }

        public string CarMake { get; set; } = null!;

        public string? CarModel { get; set; }

        public DateTime ProductionDate { get; set; }

        public int Mileage { get; set; }

        public DateTime? SaleDate { get; set; }

        public decimal? SalePrice { get; set; }

        public decimal? Commission { get; set; }

        // Для отображения (не отправляем в API)
        [JsonIgnore]
        public Client? Client { get; set; }

        [JsonIgnore]
        public string ClientName => Client?.DisplayName ?? string.Empty;
    }
}
