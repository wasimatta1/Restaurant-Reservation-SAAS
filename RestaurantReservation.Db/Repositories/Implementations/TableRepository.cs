using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Implementations.BaseImplementation;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{
    public class TableRepository : Repository<Table>, ITableRepository
    {
        public TableRepository(RestaurantReservationDbContext context) : base(context)
        {
        }
    }
}
