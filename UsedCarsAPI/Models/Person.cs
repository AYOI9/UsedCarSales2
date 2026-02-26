namespace UsedCarsAPI.Models
{
    // Пользователь для авторизации
    public class Person
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
