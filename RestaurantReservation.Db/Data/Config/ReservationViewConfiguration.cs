using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Entities.Procedures_Models;
using RestaurantReservation.Db.Entities.Views;

namespace RestaurantReservation.Db.Data.Config
{
    public class ReservationViewConfiguration : IEntityTypeConfiguration<ReservationView>
    {

        public void Configure(EntityTypeBuilder<ReservationView> builder)
        {
            builder.HasNoKey().ToView("ReservationsView");

            builder.Property(x => x.CustomerId).HasColumnName("Id");
            builder.Property(x => x.CustomerFirstName).HasColumnName("First_Name");
            builder.Property(x => x.CustomerPhoneNumber).HasColumnName("Phone_Number");
            builder.Property(x => x.RestaurantName).HasColumnName("Name");
            builder.Property(x => x.RestaurantAddress).HasColumnName("Address");
            builder.Property(x => x.RestaurantId).HasColumnName("RestaurantId");
            builder.Property(x => x.ReservationId).HasColumnName("ReservationId");
            builder.Property(x => x.ReservationDate).HasColumnName("ReservationDate");
            builder.Property(x => x.PartySize).HasColumnName("PartySize");
        }
    }
}
public class CustomerReservationProcedureConfiguration : IEntityTypeConfiguration<CustomerReservation>
{

    public void Configure(EntityTypeBuilder<CustomerReservation> builder)
    {
        builder.HasNoKey();

    }
}
