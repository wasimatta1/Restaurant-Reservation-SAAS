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

            builder.ToTable("Employees");

            builder.HasData(LoadEmployees());
        }
        public List<Employee> LoadEmployees()
        {
            return new List<Employee>
            {
                new Employee { EmployeeId = 1, RestaurantId = 1, FirstName = "Miguel", LastName = "Lopez", Position = "Manager" },
                new Employee { EmployeeId = 2, RestaurantId = 2, FirstName = "Samantha", LastName = "Green", Position = "Chef" },
                new Employee { EmployeeId = 3, RestaurantId = 3, FirstName = "David", LastName = "Wong", Position = "Waiter" },
                new Employee { EmployeeId = 4, RestaurantId = 4, FirstName = "Laura", LastName = "Kim", Position = "Host" },
                new Employee { EmployeeId = 5, RestaurantId = 5, FirstName = "Roberto", LastName = "Santini", Position = "Bartender" }
            };
        }

    }
}
