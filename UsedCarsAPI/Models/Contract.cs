using System;
using System.Text.Json.Serialization;

namespace UsedCarsAPI.Models
{
    // Договор на продажу подержанного автомобиля
    public partial class Contract
    {
        public int ContractId { get; set; }

        public int ClientId { get; set; }

        // Дата заключения договора
        public DateTime ContractDate { get; set; }

        // Марка автомобиля
        public string CarMake { get; set; } = null!;

        // Модель автомобиля (необязательно)
        public string? CarModel { get; set; }

        // Дата выпуска автомобиля
        public DateTime ProductionDate { get; set; }

        // Пробег, км
        public int Mileage { get; set; }

        // Дата продажи (может быть null, если ещё не продан)
        public DateTime? SaleDate { get; set; }

        // Цена продажи (может быть null)
        public decimal? SalePrice { get; set; }

        // Размер комиссионных (может быть null)
        public decimal? Commission { get; set; }

        // Навигационное свойство (для EF). Для JSON не нужно, поэтому скрываем.
        [JsonIgnore]
        public virtual Client? Client { get; set; }
    }
}
