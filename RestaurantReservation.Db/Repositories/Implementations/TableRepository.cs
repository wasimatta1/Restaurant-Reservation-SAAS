using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{
    public class TableRepository : ITableRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public TableRepository(RestaurantReservationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddTableAsync(Table table)
        {
            var lastTableId = _context.Tables.Max(x => x.TableId);

            table.TableId = lastTableId + 1;

            await _context.Tables.AddAsync(table);
        }

        public async Task DeleteTableAsync(Table table)
        {

            _context.Tables.Remove(table);
        }

        public async Task<Table?> GetTableByIdAsync(int restaurantId, int id)
        {
            return await _context.Tables
                .Where(c => c.RestaurantId == restaurantId && c.TableId == id).FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<Table>, PaginationMetadata)> GetTablesAsync(int restaurantId, string? capcity, int pageNumber, int pageSize)
        {
            var collection = _context.Tables.AsQueryable();

            collection = collection.Where(x => x.RestaurantId == restaurantId);

            if (!string.IsNullOrEmpty(capcity))
            {
                collection = collection.Where(x => x.Capacity == int.Parse(capcity));
            }

            var totalTables = await collection.CountAsync();


            var paginationMetadata = new PaginationMetadata(totalTables, pageSize, pageNumber);

            var tables = await collection
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (tables, paginationMetadata);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
        public async Task<bool> TableExistsAsync(int id)
        {
            return await _context.Tables.AnyAsync(x => x.TableId == id);
        }
    }
}
