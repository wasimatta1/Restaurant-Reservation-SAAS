using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Implementations;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IMenuItemRepository
    {
        public Task<(IEnumerable<MenuItem>, PaginationMetadata)> GetMenuItemsAsync(
            int restaurantId, string? name, string? searchQuery, int pageNumber, int pageSize);
        public Task<MenuItem?> GetMenuItemAsync(int restaurantId, int itemId);
        public Task AddMenuItemAsync(MenuItem menuItem);
        public Task DeleteMenuItemAsync(MenuItem menuItem);
        public Task<bool> SaveChangesAsync();
    }
}
