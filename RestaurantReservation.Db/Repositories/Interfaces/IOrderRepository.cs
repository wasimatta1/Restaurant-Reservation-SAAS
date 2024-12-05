using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Implementations;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<(IEnumerable<Order>, PaginationMetadata)> GetOrdersAsync(int restaurantId,
            string? searchQuery, int pagNumber, int pageSize,
            int? empoyeeId, int? reservationId);
        public Task<Order?> GetOrderAsync(int restaurantId, int id);
        public Task AddOrderAsync(Order order);
        public Task DeleteOrderAsync(Order order);
        public Task<bool> SaveChangesAsync();
        public Task<IEnumerable<Order>> ListOrdersAndMenuItems(int ReservationId);
        public Task<IEnumerable<MenuItem>> ListOrderedMenuItems(int ReservationId);
        public Task<decimal> CalculateAverageOrderAmount(int EmployeeId);
    }
}
