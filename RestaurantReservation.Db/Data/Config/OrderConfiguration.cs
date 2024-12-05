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

            builder.HasOne(c => c.Reservation)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.ReservationId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(c => c.Employee)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.EmployeeId)
                .IsRequired();

            builder.HasMany(c => c.MenuItems)
                .WithMany(c => c.Orders)
                .UsingEntity<OrderItem>();


            builder.ToTable("Orders");

            builder.HasData(LoadOrders());
        }
        public List<Order> LoadOrders()
        {
            return new List<Order>
            {
                new Order { OrderId = 1, ReservationId = 1, EmployeeId = 1, OrderDate = DateTime.Now, TotalAmount = 45.50M },
                new Order { OrderId = 2, ReservationId = 2, EmployeeId = 2, OrderDate = DateTime.Now, TotalAmount = 25.00M },
                new Order { OrderId = 3, ReservationId = 3, EmployeeId = 3, OrderDate = DateTime.Now, TotalAmount = 75.99M },
                new Order { OrderId = 4, ReservationId = 4, EmployeeId = 4, OrderDate = DateTime.Now, TotalAmount = 50.75M },
                new Order { OrderId = 5, ReservationId = 5, EmployeeId = 5, OrderDate = DateTime.Now, TotalAmount = 60.00M }
            };
        }

    }
}
