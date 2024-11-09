using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantReservationDbContext _context;
        public RestaurantRepository(RestaurantReservationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<(IEnumerable<Restaurant>, PaginationMetadata)> GetRestaurantAsync(string? name, string? searchQuery,
            int pagNumber, int pageSize)
        {
            var collection = _context.Restaurants.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(x => x.Name == name);
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(x => x.Name.Contains(searchQuery)
                || x.Address.Contains(searchQuery)
                || x.OpeningHours.Contains(searchQuery));
            }

            var totalRestaurant = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalRestaurant, pageSize, pagNumber);

            var restaurants = await collection
                .Skip(pageSize * (pagNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (restaurants, paginationMetadata);
        }

        public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
        {
            return await _context.Restaurants.FindAsync(id);
        }

        public async Task AddRestaurantAsync(Restaurant restaurant)
        {
            var lastRestaurantId = await _context.Restaurants.CountAsync();

            restaurant.RestaurantId = lastRestaurantId + 1;

            await _context.Restaurants.AddAsync(restaurant);
        }

        public async Task DeleteRestaurantAsync(Restaurant restaurant)
        {
            _context.Restaurants.Remove(restaurant);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }



        public async Task<IEnumerable<Reservation>> GetReservationsByCustomer(int CustomerId)
        {
            return await _context.Customers.Where(x => x.Id == CustomerId)
                .Include(c => c.Reservations)
                .SelectMany(c => c.Reservations)
                .ToListAsync();
        }

        public async Task<bool> RestaurantExistsAsync(int id)
        {
            return await _context.Restaurants.AnyAsync(x => x.RestaurantId == id);
        }
    }

}
