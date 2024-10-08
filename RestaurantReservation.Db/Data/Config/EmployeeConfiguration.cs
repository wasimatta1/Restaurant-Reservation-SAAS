using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Data.Config
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(c => c.EmployeeId);
            builder.Property(c => c.EmployeeId).ValueGeneratedNever();

            builder.Property(c => c.FirstName).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50);

            builder.Property(c => c.LastName).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50);

            builder.Property(c => c.Position).IsRequired().HasColumnType("VARCHAR").HasMaxLength(255);

            builder.HasOne(c => c.Restaurant)
                .WithMany(c => c.Employees)
                .HasForeignKey(c => c.RestaurantId)
                .IsRequired();

            builder.ToTable("Employee");
        }

    }
}
