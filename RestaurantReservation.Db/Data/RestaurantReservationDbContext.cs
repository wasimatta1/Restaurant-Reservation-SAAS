using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Entities.Procedures_Models;
using RestaurantReservation.Db.Entities.Views;

namespace RestaurantReservation.Db.Data
{
    public class RestaurantReservationDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<MenuItem> MenuItemss { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<ReservationView> ReservationsView { get; set; }
        public DbSet<EmployeeRestaurantView> EmployeeRestaurantView { get; set; }
        public DbSet<CustomerReservation> CustomerReservations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetSection("constr").Value;

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RestaurantReservationDbContext).Assembly);
        }

        [DbFunction("fn_CalculateTheTotalRevenueGeneratedByASpecificRestaurant", Schema = "dbo")]
        public decimal CalculateTheTotalRevenueGeneratedByASpecificRestaurant(int RestaurantID)
        {
            throw new NotImplementedException();
        }


    }
}
