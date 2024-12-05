using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Implementations;

namespace RestaurantReservation.Db.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        public Task<(IEnumerable<Reservation>, PaginationMetadata)> GetReservationsAsync(
            string? searchQuery, int pagNumber, int pageSize,
            int? customerId, int? restauranId, int? tableId);
        public Task<Reservation?> GetReservationAsync(int id, bool includeOrder = false);
        public Task AddReservationAsync(Reservation reservation);
        public Task DeleteReservationAsync(Reservation reservation);
        public Task<bool> SaveChangesAsync();

    }
}
