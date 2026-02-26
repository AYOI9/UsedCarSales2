using System.Text.Json.Serialization;

namespace UsedCarsClient.Models
{
    // Должен совпадать по полям с UsedCarsAPI.Models.Client
    public class Client
    {
        public int ClientId { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;

        // Для отображения в ComboBox/таблице (не отправляем в API)
        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                string mid = string.IsNullOrWhiteSpace(MiddleName) ? "" : " " + MiddleName.Trim();
                return $"{LastName} {FirstName}{mid}".Trim();
            }
        }
    }
}
