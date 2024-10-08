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

            builder.ToTable("Customer");
        }
    }
}
