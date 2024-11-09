using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Implementations;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<IEnumerable<Employee>> ListManagers();
        public Task<(IEnumerable<Employee>, PaginationMetadata)> GetEmployeesAsync(int restaurantId, string? name, string? searchQuery,
            int pagNumber, int pageSize);
        public Task<Employee?> GetEmployeeByIdAsync(int restaurantId, int id);
        public Task AddEmployeeAsync(Employee employee);
        public Task DeleteEmployeeAsync(Employee employee);
        public Task<bool> SaveChangesAsync();

    }

}
