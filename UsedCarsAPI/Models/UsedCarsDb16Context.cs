using Microsoft.EntityFrameworkCore;

namespace UsedCarsAPI.Models
{
    public partial class UsedCarsDb16Context : DbContext
    {
        public UsedCarsDb16Context()
        {
            Database.EnsureCreated();
        }

        public UsedCarsDb16Context(DbContextOptions<UsedCarsDb16Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<Person> Persons { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Admin");
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(true);
                entity.Property(e => e.Password)
                    .HasMaxLength(2000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ClientId);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.MiddleName).HasMaxLength(50);
                entity.Property(e => e.City).HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.ContractId);

                entity.Property(e => e.CarMake).HasMaxLength(50);
                entity.Property(e => e.CarModel).HasMaxLength(50);

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Commission).HasColumnType("decimal(18,2)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
