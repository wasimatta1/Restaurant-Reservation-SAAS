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


            builder.ToTable("Restaurants");

            builder.HasData(LoadRestaurants());
        }
        public List<Restaurant> LoadRestaurants()
        {
            return new List<Restaurant>
            {
                new Restaurant { RestaurantId = 1, Name = "La Fiesta", Address = "123 Fiesta Lane", PhoneNumber = "5557894561", OpeningHours = "10:00 AM - 10:00 PM" },
                new Restaurant { RestaurantId = 2, Name = "Burger Haven", Address = "456 Burger Blvd", PhoneNumber = "5557894562", OpeningHours = "11:00 AM - 11:00 PM" },
                new Restaurant { RestaurantId = 3, Name = "Sushi World", Address = "789 Sushi St", PhoneNumber = "5557894563", OpeningHours = "12:00 PM - 10:00 PM" },
                new Restaurant { RestaurantId = 4, Name = "The Veggie Spot", Address = "321 Veggie Way", PhoneNumber = "5557894564", OpeningHours = "9:00 AM - 9:00 PM" },
                new Restaurant { RestaurantId = 5, Name = "Pasta Palace", Address = "654 Pasta Ave", PhoneNumber = "5557894565", OpeningHours = "10:00 AM - 11:00 PM" }
            };
        }

    }
}
