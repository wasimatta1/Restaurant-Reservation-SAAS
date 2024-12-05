using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Implementations;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IRestaurantRepository
    {
        public Task<(IEnumerable<Restaurant>, PaginationMetadata)> GetRestaurantAsync(
            string? name, string? searchQuery, int pagNumber, int pageSize);
        public Task<Restaurant?> GetRestaurantByIdAsync(int id);
        public Task AddRestaurantAsync(Restaurant restaurant);

        public Task DeleteRestaurantAsync(Restaurant restaurant);
        public Task<bool> RestaurantExistsAsync(int id);

        public Task<bool> SaveChangesAsync();

    }
}
