using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Implementations;


namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<(IEnumerable<Customer>, PaginationMetadata)> GetCustomersAsync(string? name, string? searchQuery,
           int pagNumber, int pageSize);
        public Task<Customer?> GetCustomerByIdAsync(int id);
        public Task AddCustomerAsync(Customer customer);

        public Task DeleteCustomerAsync(Customer customer);

        public Task<bool> SaveChangesAsync();
        public Task<IEnumerable<Reservation>> GetReservationsByCustomer(int CustomerId);
    }
}
