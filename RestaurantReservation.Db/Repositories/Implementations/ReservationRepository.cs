using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Data;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;

namespace RestaurantReservation.Db.Repositories.Implementations
{

    public class ReservationRepository : IReservationRepository
    {
        private readonly RestaurantReservationDbContext _context;
        public ReservationRepository(RestaurantReservationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            var lastReservationId = _context.Reservations.Max(x => x.ReservationId);

            reservation.ReservationId = lastReservationId + 1;

            await _context.Reservations.AddAsync(reservation);
        }

        public async Task DeleteReservationAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
        }

        public async Task<(IEnumerable<Reservation>, PaginationMetadata)> GetReservationsAsync(
            string? searchQuery, int pagNumber, int pageSize,
            int? customerId, int? restauranId, int? tableId)
        {

            var collection = _context.Reservations.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(x => x.ReservationDate.ToString().Contains(searchQuery)
                || x.PartySize.ToString().Contains(searchQuery));
            }
            if (customerId.HasValue)
            {
                collection = collection.Where(x => x.CustomerId == customerId);
            }
            if (restauranId.HasValue)
            {
                collection = collection.Where(x => x.RestaurantId == restauranId);
            }
            if (tableId.HasValue)
            {
                collection = collection.Where(x => x.TableId == tableId);
            }
            var totalReservations = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalReservations, pageSize, pagNumber);

            var reservations = await collection
                .Skip(pageSize * (pagNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (reservations, paginationMetadata);
        }

        public async Task<Reservation?> GetReservationAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
