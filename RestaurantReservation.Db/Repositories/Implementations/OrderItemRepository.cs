using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public OrderItemRepository(RestaurantReservationDbContext context)
        {

            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddOrderItem(OrderItem orderItem)
        {
            var lastOrderItemId = _context.OrderItems.Max(x => x.OrderItemId);
            orderItem.OrderItemId = lastOrderItemId + 1;
            await _context.OrderItems.AddAsync(orderItem);
        }

        public async Task<bool> OrderItemExistsAsync(int id)
        {
            return await _context.OrderItems.AnyAsync(x => x.OrderItemId == id);
        }

        public async Task RemoveOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
        }
    }
}
