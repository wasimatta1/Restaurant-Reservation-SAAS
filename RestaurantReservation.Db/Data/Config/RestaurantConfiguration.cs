using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Data.Config
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasKey(c => c.RestaurantId);
            builder.Property(c => c.RestaurantId).ValueGeneratedNever();

            builder.Property(c => c.Name).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50);

            builder.Property(c => c.Address).IsRequired().HasColumnType("VARCHAR").HasMaxLength(255);

            builder.Property(c => c.PhoneNumber).IsRequired().HasColumnType("VARCHAR").HasMaxLength(15);

            builder.Property(c => c.OpeningHours).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50);


            builder.ToTable("Restaurant");
        }
    }
}
