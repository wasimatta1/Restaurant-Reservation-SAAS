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

            builder.Property(c => c.First_Name).IsRequired(false).HasColumnType("VARCHAR").HasMaxLength(50);

            builder.Property(c => c.Last_Name).IsRequired(false).HasColumnType("VARCHAR").HasMaxLength(50);

            builder.Property(c => c.Email).IsRequired(false).HasColumnType("VARCHAR").HasMaxLength(255);

            builder.Property(c => c.Phone_Number).IsRequired(false).HasColumnType("VARCHAR").HasMaxLength(15);

            builder.ToTable("Customers");

            builder.HasData(LoadCustomers());

        }
        public List<Customer> LoadCustomers()
        {
            return new List<Customer>
            {
                new Customer { Id = 1, First_Name = "Alice", Last_Name = "Johnson", Email = "alice.j@example.com", Phone_Number = "5551234567" },
                new Customer { Id = 2, First_Name = "Bob", Last_Name = "Smith", Email = "bob.smith@example.com", Phone_Number = "5557654321" },
                new Customer { Id = 3, First_Name = "Carlos", Last_Name = "Martinez", Email = "carlos.m@example.com", Phone_Number = "5552345678" },
                new Customer { Id = 4, First_Name = "Dana", Last_Name = "Lee", Email = "dana.lee@example.com", Phone_Number = "5558765432" },
                new Customer { Id = 5, First_Name = "Eva", Last_Name = "Brown", Email = "eva.b@example.com", Phone_Number = "5553456789" }
            };
        }

    }
}
