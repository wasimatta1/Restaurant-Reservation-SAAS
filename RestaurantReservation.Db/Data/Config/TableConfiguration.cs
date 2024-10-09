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

            builder.HasOne(c => c.Restaurant)
                .WithMany(c => c.Tables)
                .HasForeignKey(c => c.RestaurantId)
                .IsRequired();

            builder.ToTable("Tables");

            builder.HasData(LoadTables());
        }
        public List<Table> LoadTables()
        {
            return new List<Table>
            {
                new Table { TableId = 1, RestaurantId = 1, Capacity = 4 },
                new Table { TableId = 2, RestaurantId = 2, Capacity = 2 },
                new Table { TableId = 3, RestaurantId = 3, Capacity = 6 },
                new Table { TableId = 4, RestaurantId = 4, Capacity = 8 },
                new Table { TableId = 5, RestaurantId = 5, Capacity = 4 }
            };
        }

    }
}
