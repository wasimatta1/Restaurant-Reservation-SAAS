using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Implementations.BaseImplementation;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly RestaurantReservationDbContext _context;
        public CustomerRepository(RestaurantReservationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Reservation>> GetReservationsByCustomer(int CustomerId)
        {
            return await _context.Customers.Where(x => x.Id == CustomerId)
                .Include(c => c.Reservations)
                .SelectMany(c => c.Reservations)
                .ToListAsync();
        }
    }
}
