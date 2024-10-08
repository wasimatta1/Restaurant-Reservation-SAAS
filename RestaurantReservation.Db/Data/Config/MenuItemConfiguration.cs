using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Data.Config
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.HasKey(c => c.ItemId);
            builder.Property(c => c.ItemId).ValueGeneratedNever();

            builder.Property(c => c.Name).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50);

            builder.Property(c => c.Description).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50);

            builder.Property(c => c.Price).HasPrecision(15, 2);

            builder.ToTable("MenuItem");
        }

    }
}
