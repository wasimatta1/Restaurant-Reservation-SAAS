using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Implementations;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface ITableRepository
    {

        public Task<(IEnumerable<Table>, PaginationMetadata)> GetTablesAsync(int restaurantId, string? capcity, int pageNumber, int pageSize);
        public Task<Table?> GetTableByIdAsync(int restaurantId, int id);

        public Task AddTableAsync(Table table);
        public Task DeleteTableAsync(Table table);
        public Task<bool> SaveChangesAsync();
        public Task<bool> TableExistsAsync(int id);
    }
}
