using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Repositories.Interfaces.BaseInterface;


namespace RestaurantReservation.Db.Repositories.Implementations.BaseImplementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly RestaurantReservationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(RestaurantReservationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity is null) return;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

        }
    }

}
