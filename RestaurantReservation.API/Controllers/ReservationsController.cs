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

        [HttpGet]
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
        [HttpGet("{id}", Name = "GetReservation")]
        public async Task<ActionResult<ReservationInfoDto>> GetReservationAsync(int id)
        {
            var reservationEntity = await _reservationRepository.GetReservationAsync(id);

            if (reservationEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ReservationInfoDto>(reservationEntity));
        }
        [HttpPost]
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
        [HttpDelete("{id}")]
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
        [HttpPut]
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


        [HttpGet("{reservationId}/orders")]
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

        [HttpGet("customer/{customerId}")]
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
        [HttpGet("{reservationId}/menu-items")]
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
