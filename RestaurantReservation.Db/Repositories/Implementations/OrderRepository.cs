using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Implementations.BaseImplementation;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public OrderRepository(RestaurantReservationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> ListOrdersAndMenuItems(int ReservationId)
        {
            return await _context.Orders
                .Where(x => x.ReservationId == ReservationId)
                .Include(x => x.MenuItems)
                .Include(x => x.OrderItems)
                .ToListAsync();
        }
        public async Task<IEnumerable<MenuItem>> ListOrderedMenuItems(int ReservationId)
        {
            return await _context.Orders
                    .Where(x => x.ReservationId == ReservationId)
                    .Include(x => x.MenuItems)
                    .SelectMany(x => x.MenuItems)
                    .ToListAsync();
        }
        public Task<decimal> CalculateAverageOrderAmount(int EmployeeId)
        {
            return _context.Orders
                .Where(x => x.EmployeeId == EmployeeId)
                .AverageAsync(x => x.TotalAmount);
        }
    }
}
