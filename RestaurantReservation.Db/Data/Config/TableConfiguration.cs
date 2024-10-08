using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Data.Config
{
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.HasKey(c => c.TableId);
            builder.Property(c => c.TableId).ValueGeneratedNever();

            builder.Property(c => c.Capacity);

            builder.ToTable("Table");
        }
    }
}
