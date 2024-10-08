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

            builder.ToTable("Reservation");
        }
    }
}
