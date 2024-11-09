using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly RestaurantReservationDbContext _context;
        public EmployeeRepository(RestaurantReservationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddEmployeeAsync(Employee employee)
        {

            var lastEmployeeId = _context.Employees.Count();

            employee.EmployeeId = lastEmployeeId + 1;

            await _context.Employees.AddAsync(employee);

        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public async Task<Employee?> GetEmployeeAsync(int restaurantId, int id)
        {
            return await _context.Employees
                .Where(c => c.RestaurantId == restaurantId && c.EmployeeId == id).FirstOrDefaultAsync();
        }
        public async Task<Employee?> GetEmployeeAsync(int id)
        {
            return await _context.Employees.Where(c => c.EmployeeId == id).FirstOrDefaultAsync();
        }
        public async Task<(IEnumerable<Employee>, PaginationMetadata)> GetEmployeesAsync(
           int restaurantId, string? name, string? searchQuery, int pagNumber, int pageSize)
        {


            var collection = _context.Employees.AsQueryable();

            collection = collection.Where(x => x.RestaurantId == restaurantId);

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(x => x.FirstName == name);
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(x => x.FirstName.Contains(searchQuery)
                || x.LastName.Contains(searchQuery)
                || x.Position.Contains(searchQuery));
            }
            var totalEmployees = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalEmployees, pageSize, pagNumber);

            var employees = await collection
                .Skip(pageSize * (pagNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (employees, paginationMetadata);
        }

        public async Task<IEnumerable<Employee>> ListManagers()
        {
            return await _context.Employees.Where(e => e.Position.Equals("Manager")).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }


    }
}



