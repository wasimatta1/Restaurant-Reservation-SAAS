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

            builder.HasOne(c => c.Order)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(c => c.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(c => c.MenuItem)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(c => c.ItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.ToTable("OrderItems");

            builder.HasData(LoadOrderItems());
        }
        public List<OrderItem> LoadOrderItems()
        {
            return new List<OrderItem>
            {
                new OrderItem { OrderItemId = 1, OrderId = 1, ItemId = 1, Quantity = 2 },
                new OrderItem { OrderItemId = 2, OrderId = 2, ItemId = 2, Quantity = 1 },
                new OrderItem { OrderItemId = 3, OrderId = 3, ItemId = 3, Quantity = 3 },
                new OrderItem { OrderItemId = 4, OrderId = 4, ItemId = 4, Quantity = 2 },
                new OrderItem { OrderItemId = 5, OrderId = 5, ItemId = 5, Quantity = 1 }
            };
        }

    }
}
