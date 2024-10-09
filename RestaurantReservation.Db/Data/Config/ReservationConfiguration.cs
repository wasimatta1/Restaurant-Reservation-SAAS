using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Data.Config
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(c => c.ReservationId);
            builder.Property(c => c.ReservationId).ValueGeneratedNever();

            builder.Property(c => c.ReservationDate);

            builder.Property(c => c.PartySize);

            builder.HasOne(c => c.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(c => c.CustomerId)
                .IsRequired();

            builder.HasOne(c => c.Table)
                .WithMany(c => c.Reservations)
                .HasForeignKey(c => c.TableId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(c => c.Restaurant)
                .WithMany(c => c.Reservations)
                .HasForeignKey(c => c.RestaurantId)
                .IsRequired();


            builder.ToTable("Reservations");

            builder.HasData(LoadReservations());
        }
        public List<Reservation> LoadReservations()
        {
            return new List<Reservation>
            {
                new Reservation { ReservationId = 1, CustomerId = 1, RestaurantId = 1, TableId = 1, ReservationDate = DateTime.Now.AddDays(1).AddHours(7), PartySize = 4 },
                new Reservation { ReservationId = 2, CustomerId = 2, RestaurantId = 2, TableId = 2, ReservationDate = DateTime.Now.AddDays(2).AddHours(5), PartySize = 2 },
                new Reservation { ReservationId = 3, CustomerId = 3, RestaurantId = 3, TableId = 3, ReservationDate = DateTime.Now.AddDays(3).AddHours(6), PartySize = 5 },
                new Reservation { ReservationId = 4, CustomerId = 4, RestaurantId = 4, TableId = 4, ReservationDate = DateTime.Now.AddDays(4).AddHours(8), PartySize = 3 },
                new Reservation { ReservationId = 5, CustomerId = 5, RestaurantId = 5, TableId = 5, ReservationDate = DateTime.Now.AddDays(5).AddHours(7), PartySize = 6 }
            };
        }

    }
}
