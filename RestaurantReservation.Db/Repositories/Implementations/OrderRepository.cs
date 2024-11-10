using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantReservationDbContext _context;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderRepository(RestaurantReservationDbContext context, IOrderItemRepository orderItemRepository)
        {
            _context = context;
            _orderItemRepository = orderItemRepository;
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

        public async Task<(IEnumerable<Order>, PaginationMetadata)> GetOrdersAsync(int restaurantId,
            string? searchQuery, int pagNumber, int pageSize, int? empoyeeId, int? reservationId)
        {
            var collection = _context.Orders.AsQueryable();

            collection = collection.Include(x => x.Employee)
                .Include(x => x.MenuItems).Where(x => x.Employee.RestaurantId == restaurantId);



            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(x => x.OrderDate.ToString().Contains(searchQuery)
                || x.TotalAmount.ToString().Contains(searchQuery));
            }
            if (empoyeeId.HasValue)
            {
                collection = collection.Where(x => x.EmployeeId == empoyeeId);
            }
            if (reservationId.HasValue)
            {
                collection = collection.Where(x => x.ReservationId == reservationId);
            }


            var totalOrders = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalOrders, pageSize, pagNumber);

            var orders = await collection
                .Skip(pageSize * (pagNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (orders, paginationMetadata);
        }

        public async Task<Order?> GetOrderAsync(int restaurantId, int id)
        {
            return await _context.Orders.Include(x => x.Employee).Include(x => x.MenuItems)
                .Where(x => x.Employee.RestaurantId == restaurantId && x.OrderId == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            var lastOrderId = _context.Orders.Max(x => x.OrderId);

            order.OrderId = lastOrderId + 1;
            foreach (var item in order.OrderItems)
            {
                item.OrderId = lastOrderId + 1;
                _orderItemRepository.AddOrderItem(item);
            }

            await _context.Orders.AddAsync(order);
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
