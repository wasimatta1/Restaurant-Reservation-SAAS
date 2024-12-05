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

            builder.HasOne(c => c.Restaurant)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(c => c.RestaurantId)
                .IsRequired();

            builder.ToTable("MenuItems");

            builder.HasData(LoadMenuItems());
        }
        public List<MenuItem> LoadMenuItems()
        {
            return new List<MenuItem>
            {
                new MenuItem { ItemId = 1, RestaurantId = 1, Name = "Tacos", Description = "Spicy beef tacos with fresh salsa", Price = 9.99M },
                new MenuItem { ItemId = 2, RestaurantId = 2, Name = "Classic Cheeseburger", Description = "Juicy burger with cheddar cheese", Price = 12.99M },
                new MenuItem { ItemId = 3, RestaurantId = 3, Name = "California Roll", Description = "Crab, avocado, and cucumber roll", Price = 8.49M },
                new MenuItem { ItemId = 4, RestaurantId = 4, Name = "Vegan Burrito", Description = "Black beans, rice, and veggies", Price = 7.99M },
                new MenuItem { ItemId = 5, RestaurantId = 5, Name = "Spaghetti Bolognese", Description = "Classic pasta with rich meat sauce", Price = 13.99M }
            };
        }

    }
}
