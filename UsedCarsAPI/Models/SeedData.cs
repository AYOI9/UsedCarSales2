using Microsoft.EntityFrameworkCore;

namespace UsedCarsAPI.Models
{
    public static class SeedData
    {
        public static void SeedDatabase(UsedCarsDb16Context context)
        {
            context.Database.EnsureCreated();
            if (!context.Persons.Any())
            {
                Person user = new Person { Email = "admin@gmail.com", Password = "1234" };
                user.Password = AuthOptions.GetHash(user.Password);
                context.Persons.Add(user);
                context.SaveChanges();
            }
        }
    }
}
