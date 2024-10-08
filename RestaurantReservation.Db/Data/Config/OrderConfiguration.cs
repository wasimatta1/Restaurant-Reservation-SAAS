using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(c => c.OrderId);
            builder.Property(c => c.OrderId).ValueGeneratedNever();

            builder.Property(c => c.OrderDate);

            builder.Property(c => c.TotalAmount).HasPrecision(15, 2);

            builder.ToTable("Order");
        }
    }
}
