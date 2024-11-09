using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{
    public partial class CustomerRepository : ICustomerRepository
    {
        private readonly RestaurantReservationDbContext _context;
        public CustomerRepository(RestaurantReservationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<(IEnumerable<Customer>, PaginationMetadata)> GetCustomersAsync(string? name, string? searchQuery,
            int pagNumber, int pageSize)
        {
            var collection = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(x => x.FirstName != null && x.FirstName == name);
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(x => x.FirstName != null && x.FirstName.Contains(searchQuery)
                || x.LastName != null && x.LastName.Contains(searchQuery)
                || x.Email != null && x.Email.Contains(searchQuery));
            }

            var totalCustomers = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalCustomers, pageSize, pagNumber);

            var customers = await collection
                .Skip(pageSize * (pagNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (customers, paginationMetadata);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            var lastCustomerId = await _context.Customers.CountAsync();

            customer.Id = lastCustomerId + 1;

            await _context.Customers.AddAsync(customer);
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
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
