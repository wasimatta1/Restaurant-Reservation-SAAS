using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Entities.Views;

namespace RestaurantReservation.Db.Data.Config
{
    public class EmployeeRestaurantViewConfiguration : IEntityTypeConfiguration<EmployeeRestaurantView>
    {

        public void Configure(EntityTypeBuilder<EmployeeRestaurantView> builder)
        {
            builder.HasNoKey().ToView("EmployeeRestaurantView");
        }
    }
}
