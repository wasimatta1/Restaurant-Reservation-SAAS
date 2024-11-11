using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.MenuItenDto;
using RestaurantReservation.API.DTO_s.OrderDto;
using RestaurantReservation.API.DTO_s.ReservationDto;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ReservationsController : Controller
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;
        public ReservationsController(IReservationRepository reservationRepository, IRestaurantRepository restaurantRepository, ICustomerRepository customerRepository, ITableRepository tableRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
            _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _tableRepository = tableRepository ?? throw new ArgumentNullException(nameof(tableRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Retrieves a list of reservations with optional filtering by search query, customer, restaurant, and table.
        /// </summary>
        /// <param name="searchQuery">Optional search term to filter reservations.</param>
        /// <param name="customerId">Optional customer ID to filter reservations.</param>
        /// <param name="restauranId">Optional restaurant ID to filter reservations.</param>
        /// <param name="tableId">Optional table ID to filter reservations.</param>
        /// <param name="pagNumber">Page number for pagination.</param>
        /// <param name="pageSize">Page size for pagination.</param>
        /// <returns>A list of reservations matching the search criteria.</returns>
        /// <response code="200">Returns a paginated list of reservations.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<ReservationInfoDto>>> GetAllAsync(
            string? searchQuery, int? customerId, int? restauranId,
            int? tableId, int pagNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = 10;
            }

            var (reservationEntities, paginationMetadata) = await _reservationRepository
                .GetReservationsAsync(searchQuery, pagNumber, pageSize, customerId, restauranId, tableId);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<ReservationInfoDto>>(reservationEntities));
        }


        /// <summary>
        /// Retrieves a specific reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to retrieve.</param>
        /// <returns>An <see cref="ReservationInfoDto"/> object representing the reservation.</returns>
        /// <response code="200">Returns the requested reservation if found.</response>
        /// <response code="404">If the reservation is not found.</response>

        [HttpGet("{id}", Name = "GetReservation")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ReservationInfoDto>> GetReservationAsync(int id)
        {
            var reservationEntity = await _reservationRepository.GetReservationAsync(id);

            if (reservationEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ReservationInfoDto>(reservationEntity));
        }


        /// <summary>
        /// Creates a new reservation for a customer at a specified restaurant and table.
        /// </summary>
        /// <param name="reservation">An object containing the details of the reservation to create.</param>
        /// <returns>The created <see cref="ReservationInfoDto"/> object.</returns>
        /// <response code="201">Returns the newly created reservation.</response>
        /// <response code="400">If the restaurant, customer, or table does not exist.</response>
        /// 
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ReservationInfoDto>> CreateReservation(ReservationCreateDto reservation)
        {
            var reservationEntity = _mapper.Map<Reservation>(reservation);


            if (!await _restaurantRepository.RestaurantExistsAsync(reservation.RestaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }
            if (await _customerRepository.GetCustomerByIdAsync(reservation.CustomerId) is null)
            {
                return BadRequest("Customer Not Found");
            }
            if (!await _tableRepository.TableExistsAsync(reservation.TableId))
            {
                return BadRequest("Table Not Found");
            }

            await _reservationRepository.AddReservationAsync(reservationEntity);

            await _reservationRepository.SaveChangesAsync();

            return CreatedAtRoute("GetReservation",
                 new
                 {
                     id = reservationEntity.ReservationId
                 },
                 reservation);
        }


        /// <summary>
        /// Deletes a specific reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to delete.</param>
        /// <returns>A <see cref="NoContentResult"/> if the deletion is successful.</returns>
        /// <response code="204">If the deletion is successful.</response>
        /// <response code="404">If the reservation is not found.</response>
        /// 
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteReservation(int id)
        {
            var reservationEntity = await _reservationRepository.GetReservationAsync(id);

            if (reservationEntity is null)
            {
                return NotFound();
            }

            await _reservationRepository.DeleteReservationAsync(reservationEntity);

            await _reservationRepository.SaveChangesAsync();

            return NoContent();

        }

        /// <summary>
        /// Updates an existing reservation by its ID.
        /// </summary>
        /// <param name="reservation">An object containing the updated details of the reservation.</param>
        /// <returns>A <see cref="NoContentResult"/> if the update is successful.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the customer or table does not exist.</response>
        /// <response code="404">If the reservation is not found.</response>

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateReservation(ReservationUpdateDto reservation)
        {
            var reservationEntity = await _reservationRepository.GetReservationAsync(reservation.ReservationId);

            if (reservationEntity is null)
            {
                return NotFound();
            }

            if (await _customerRepository.GetCustomerByIdAsync(reservation.CustomerId) is null)
            {
                return BadRequest("Customer Not Found");
            }
            if (await _tableRepository.TableExistsAsync(reservation.TableId))
            {
                return BadRequest("Table Not Found");
            }

            _mapper.Map(reservation, reservationEntity);

            await _reservationRepository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retrieves all orders associated with a specific reservation.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation.</param>
        /// <returns>A list of orders related to the reservation.</returns>
        /// <response code="200">Returns a list of orders for the reservation.</response>
        /// <response code="404">If the reservation is not found.</response>

        [HttpGet("{reservationId}/orders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<OrderInfoDto>>> GetOrdersForReservation(int reservationId)
        {
            var reservationEntity = await _reservationRepository.GetReservationAsync(reservationId, true);

            if (reservationEntity is null)
            {
                return NotFound();
            }

            var orders = reservationEntity.Orders;

            return Ok(_mapper.Map<IEnumerable<OrderInfoDto>>(orders));

        }

        /// <summary>
        /// Retrieves all reservations for a specific customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>A list of reservations for the customer.</returns>
        /// <response code="200">Returns a list of reservations for the customer.</response>
        /// <response code="404">If the customer is not found.</response>

        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ReservationInfoDto>>> GetReservationsByCustomer(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

            if (customer is null)
            {
                return NotFound();
            }

            var reservations = await _customerRepository.GetReservationsByCustomer(customerId);

            return Ok(_mapper.Map<IEnumerable<ReservationInfoDto>>(reservations));
        }

        /// <summary>
        /// Retrieves all menu items associated with a specific reservation by examining its orders.
        /// </summary>
        /// <param name="reservationId">The ID of the reservation.</param>
        /// <returns>A list of distinct menu items ordered for the reservation.</returns>
        /// <response code="200">Returns a list of menu items ordered for the reservation.</response>
        /// <response code="404">If the reservation is not found.</response>

        [HttpGet("{reservationId}/menu-items")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<MenuItemInfoDto>>> GetMenuItemsForReservation(int reservationId)
        {
            var reservationEntity = await _reservationRepository.GetReservationAsync(reservationId, true);

            if (reservationEntity is null)
            {
                return NotFound();
            }

            var orders = reservationEntity.Orders;

            var menuItems = orders.SelectMany(x => x.OrderItems).Select(x => x.MenuItem).Distinct();

            return Ok(_mapper.Map<IEnumerable<MenuItemInfoDto>>(menuItems));
        }
    }
}
