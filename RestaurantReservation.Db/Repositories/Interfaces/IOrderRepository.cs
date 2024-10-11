using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces.BaseInterface;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task<IEnumerable<Order>> ListOrdersAndMenuItems(int ReservationId);
        public Task<IEnumerable<MenuItem>> ListOrderedMenuItems(int ReservationId);
        public Task<decimal> CalculateAverageOrderAmount(int EmployeeId);
    }
}
