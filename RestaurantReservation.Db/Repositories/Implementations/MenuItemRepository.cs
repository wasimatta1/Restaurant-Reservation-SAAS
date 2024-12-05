using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{

    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public MenuItemRepository(RestaurantReservationDbContext context)
        {
            _context = context;
        }
        //now all function same above function 
        public async Task AddMenuItemAsync(MenuItem menuItem)
        {

            var lastMenuItemId = _context.MenuItemss.Max(x => x.ItemId);

            menuItem.ItemId = lastMenuItemId + 1;

            await _context.MenuItemss.AddAsync(menuItem);

        }
        public async Task DeleteMenuItemAsync(MenuItem menuItem)
        {
            _context.MenuItemss.Remove(menuItem);
        }

        public async Task<MenuItem?> GetMenuItemAsync(int restaurantId, int id)
        {
            return await _context.MenuItemss
                .Where(c => c.RestaurantId == restaurantId && c.ItemId == id).FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<MenuItem>, PaginationMetadata)> GetMenuItemsAsync(
           int restaurantId, string? name, string? searchQuery, int pagNumber, int pageSize)
        {

            var collection = _context.MenuItemss.AsQueryable();

            collection = collection.Where(x => x.RestaurantId == restaurantId);

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(x => x.Name == name);
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(x => x.Name.Contains(searchQuery)
                || x.Price.ToString().Contains(searchQuery)
                || x.Description.Contains(searchQuery));
            }
            var totalMenuItems = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalMenuItems, pageSize, pagNumber);

            var menuItems = await collection
                .Skip(pageSize * (pagNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (menuItems, paginationMetadata);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

    }

}
