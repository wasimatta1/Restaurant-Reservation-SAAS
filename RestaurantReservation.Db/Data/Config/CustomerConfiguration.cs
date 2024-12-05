using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Data.Config
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {

        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();

            builder.Property(c => c.FirstName).IsRequired(false).HasColumnType("VARCHAR").HasMaxLength(50);

            builder.Property(c => c.LastName).IsRequired(false).HasColumnType("VARCHAR").HasMaxLength(50);

            builder.Property(c => c.Email).IsRequired(false).HasColumnType("VARCHAR").HasMaxLength(255);

            builder.Property(c => c.PhoneNumber).IsRequired(false).HasColumnType("VARCHAR").HasMaxLength(15);

            builder.ToTable("Customers");

            builder.HasData(LoadCustomers());

        }
        public List<Customer> LoadCustomers()
        {
            return new List<Customer>
            {
                new Customer { Id = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice.j@example.com", PhoneNumber = "5551234567" },
                new Customer { Id = 2, FirstName = "Bob", LastName = "Smith", Email = "bob.smith@example.com", PhoneNumber = "5557654321" },
                new Customer { Id = 3, FirstName = "Carlos", LastName = "Martinez", Email = "carlos.m@example.com", PhoneNumber = "5552345678" },
                new Customer { Id = 4, FirstName = "Dana", LastName = "Lee", Email = "dana.lee@example.com", PhoneNumber = "5558765432" },
                new Customer { Id = 5, FirstName = "Eva", LastName = "Brown", Email = "eva.b@example.com", PhoneNumber = "5553456789" }
            };
        }

    }
}
