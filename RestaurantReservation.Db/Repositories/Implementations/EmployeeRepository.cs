using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Implementations.BaseImplementation;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly RestaurantReservationDbContext _context;
        public EmployeeRepository(RestaurantReservationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Employee>> ListManagers()
        {
            return await _context.Employees.Where(e => e.Position.Equals("Manager")).ToListAsync();
        }
    }
}
