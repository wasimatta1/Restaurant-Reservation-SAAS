using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.OrderDto;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;
        public OrderController(IOrderRepository orderRepository, IEmployeeRepository employeeRepository,
            IReservationRepository reservationRepository, IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
            _orderItemRepository = orderItemRepository ?? throw new ArgumentNullException(nameof(orderItemRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        /// <summary>
        /// Retrieves a list of orders for the authenticated restaurant with optional filtering by search query, employee, and reservation.
        /// </summary>
        /// <param name="searchQuery">Optional search term to filter orders.</param>
        /// <param name="employeeId">Optional ID of the employee to filter orders.</param>
        /// <param name="reservatiooId">Optional reservation ID to filter orders.</param>
        /// <param name="pagNumber">Page number for pagination.</param>
        /// <param name="pageSize">Page size for pagination.</param>
        /// <returns>A list of orders matching the search criteria.</returns>
        /// <response code="200">Returns a paginated list of orders.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<OrderInfoDto>>> GetAllAsync(
                string? searchQuery, int? employeeId, int? reservatiooId,
                 int pagNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = 10;
            }
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }

            var (orderEntities, paginationMetadata) = await _orderRepository
                .GetOrdersAsync(restaurantId, searchQuery, pagNumber, pageSize, employeeId, reservatiooId);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<OrderInfoDto>>(orderEntities));
        }

        /// <summary>
        /// Retrieves a specific order by its ID for the authenticated restaurant.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>An <see cref="OrderInfoDto"/> object representing the order.</returns>
        /// <response code="200">Returns the requested order if found.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the order is not found.</response>
        /// 
        [HttpGet("{id}", Name = "GetOrder")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrderInfoDto>> GetOrderAsync(int id)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }
            var orderEntity = await _orderRepository.GetOrderAsync(restaurantId, id);

            if (orderEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OrderInfoDto>(orderEntity));
        }


        /// <summary>
        /// Creates a new order for the authenticated restaurant.
        /// </summary>
        /// <param name="order">An object containing the details of the order to create.</param>
        /// <returns>The created <see cref="OrderInfoDto"/> object.</returns>
        /// <response code="201">Returns the newly created order.</response>
        /// <response code="400">If the reservation, employee, or menu item does not exist.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<OrderInfoDto>> CreateOrder(OrderCreateDto order)
        {
            var orderEntity = _mapper.Map<Order>(order);

            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }

            if (await _reservationRepository.GetReservationAsync(order.ReservationId) is null)
            {
                return BadRequest("Reservation Not Found");
            }
            if (await _employeeRepository.GetEmployeeAsync(restaurantId, order.EmployeeId) is null)
            {
                return BadRequest("Employee Not Found");
            }
            foreach (var item in order.OrderItemCreateDto)
            {
                if (!await _orderItemRepository.OrderItemExistsAsync(item.ItemId))
                {
                    return BadRequest("Menu Item Not Found");
                }
            }


            await _orderRepository.AddOrderAsync(orderEntity);

            await _orderRepository.SaveChangesAsync();

            return CreatedAtRoute("GetOrder",
                 new
                 {
                     id = orderEntity.OrderId
                 },
                 order);
        }


        /// <summary>
        /// Updates an existing order for the authenticated restaurant.
        /// </summary>
        /// <param name="order">An object containing the updated details of the order.</param>
        /// <returns>A <see cref="NoContentResult"/> if the update is successful.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the reservation, employee, or menu item does not exist.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the order is not found.</response>

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateOrder(OrderUpdateDto order)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }
            var orderEntity = await _orderRepository.GetOrderAsync(restaurantId, order.OrderId);

            if (orderEntity is null)
            {
                return NotFound();
            }

            if (await _reservationRepository.GetReservationAsync(order.ReservationId) is null)
            {
                return BadRequest("Reservation Not Found");
            }
            if (await _employeeRepository.GetEmployeeAsync(restaurantId, order.EmployeeId) is null)
            {
                return BadRequest("Employee Not Found");
            }
            foreach (var item in order.OrderItemUpdateDto)
            {
                if (!await _orderItemRepository.OrderItemExistsAsync(item.ItemId))
                {
                    return BadRequest("Menu Item Not Found");
                }
            }

            _mapper.Map(order, orderEntity);

            await _orderRepository.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Deletes a specific order by its ID for the authenticated restaurant.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A <see cref="NoContentResult"/> if the deletion is successful.</returns>
        /// <response code="204">If the deletion is successful.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the order is not found.</response>
        /// 
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }
            var orderEntity = await _orderRepository.GetOrderAsync(restaurantId, id);

            if (orderEntity is null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteOrderAsync(orderEntity);

            await _orderRepository.SaveChangesAsync();

            return NoContent();

        }

    }
}
