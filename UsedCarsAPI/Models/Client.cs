using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UsedCarsAPI.Models
{
    // Клиент фирмы
    public partial class Client
    {
        public int ClientId { get; set; }

        // Фамилия
        public string LastName { get; set; } = null!;
        // Имя
        public string FirstName { get; set; } = null!;
        // Отчество
        public string? MiddleName { get; set; }

        // Город
        public string City { get; set; } = null!;
        // Адрес
        public string Address { get; set; } = null!;
        // Контактный телефон
        public string Phone { get; set; } = null!;

        // Навигационное свойство (для EF). Для JSON не нужно, поэтому скрываем.
        [JsonIgnore]
        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}
