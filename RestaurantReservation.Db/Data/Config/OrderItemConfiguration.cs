using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Data.Config
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(c => c.OrderItemId);
            builder.Property(c => c.OrderItemId).ValueGeneratedNever();

            builder.Property(c => c.Quantity);

            builder.ToTable("OrderItem");
        }
    }
}
