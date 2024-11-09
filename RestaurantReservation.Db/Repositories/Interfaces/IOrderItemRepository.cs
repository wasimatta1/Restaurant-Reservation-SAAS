using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        public Task AddOrderItem(OrderItem orderItem);

        public Task<bool> OrderItemExistsAsync(int id);

    }
}
