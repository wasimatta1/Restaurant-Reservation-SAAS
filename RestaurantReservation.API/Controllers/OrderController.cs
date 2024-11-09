using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.OrderDto;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("api/restaurants/{restaurantId}/[controller]")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderInfoDto>>> GetAllAsync(int restaurantId,
                string? searchQuery, int? employeeId, int? reservatiooId,
                 int pagNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = 10;
            }

            var (orderEntities, paginationMetadata) = await _orderRepository
                .GetOrdersAsync(restaurantId, searchQuery, pagNumber, pageSize, employeeId, reservatiooId);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<OrderInfoDto>>(orderEntities));
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderInfoDto>> GetOrderAsync(int restaurantId, int id)
        {
            var orderEntity = await _orderRepository.GetOrderAsync(restaurantId, id);

            if (orderEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OrderInfoDto>(orderEntity));
        }
        [HttpPost]
        public async Task<ActionResult<OrderInfoDto>> CreateOrder(int restaurantId, OrderCreateDto order)
        {
            var orderEntity = _mapper.Map<Order>(order);


            if (await _reservationRepository.GetReservationAsync(order.ReservationId) is null)
            {
                return BadRequest("Reservation Not Found");
            }
            if (await _employeeRepository.GetEmployeeByIdAsync(restaurantId, order.EmployeeId) is null)
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int restaurantid, int id)
        {
            var orderEntity = await _orderRepository.GetOrderAsync(restaurantid, id);

            if (orderEntity is null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteOrderAsync(orderEntity);

            await _orderRepository.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut]
        public async Task<ActionResult> UpdateOrder(int restaurantId, OrderUpdateDto order)
        {
            var orderEntity = await _orderRepository.GetOrderAsync(restaurantId, order.OrderId);

            if (orderEntity is null)
            {
                return NotFound();
            }

            if (await _reservationRepository.GetReservationAsync(order.ReservationId) is null)
            {
                return BadRequest("Reservation Not Found");
            }
            if (await _employeeRepository.GetEmployeeByIdAsync(restaurantId, order.EmployeeId) is null)
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
    }
}
