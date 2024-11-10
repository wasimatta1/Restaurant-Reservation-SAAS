using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.EmployeeDto;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;
        public EmployeesController(IEmployeeRepository employeeRepository, IRestaurantRepository restaurantRepository,
            IOrderRepository orderRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeInfoDto>>> GetAllAsync(string? name, string? searchQuery,
            int pagNumber = 1, int pageSize = 10)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }

            if (pageSize > maxCitiesPageSize)
            {
                pageSize = 10;
            }

            var (employeeEntities, paginationMetadata) = await _employeeRepository
                .GetEmployeesAsync(restaurantId, name, searchQuery, pagNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<EmployeeInfoDto>>(employeeEntities));
        }
        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<ActionResult<EmployeeInfoDto>> GetEmployeeByIdAsync(int id)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }

            var employeeEntity = await _employeeRepository.GetEmployeeAsync(restaurantId, id);

            if (employeeEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EmployeeInfoDto>(employeeEntity));
        }
        [HttpPost]
        public async Task<ActionResult<EmployeeInfoDto>> CreateEmployee(EmployeeCreateDto employee)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }


            var employeeEntity = _mapper.Map<Employee>(employee);

            employeeEntity.RestaurantId = restaurantId;

            await _employeeRepository.AddEmployeeAsync(employeeEntity);

            await _employeeRepository.SaveChangesAsync();

            return CreatedAtRoute("GetEmployee",
                 new
                 {
                     restaurantId = restaurantId,
                     id = employeeEntity.EmployeeId
                 },
                 employee);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateEmployee(EmployeeUpdateDto employee)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }

            var employeeEntity = await _employeeRepository.GetEmployeeAsync(restaurantId, employee.EmployeeId);

            if (employeeEntity is null)
            {
                return NotFound();
            }


            _mapper.Map(employee, employeeEntity);

            await _employeeRepository.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }

            var employeeEntity = await _employeeRepository.GetEmployeeAsync(restaurantId, id);

            if (employeeEntity is null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteEmployeeAsync(employeeEntity);

            await _employeeRepository.SaveChangesAsync();

            return NoContent();

        }
        [HttpGet("managers")]
        public async Task<ActionResult<IEnumerable<EmployeeInfoDto>>> GetManagersAsync()
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeInfoDto>>(await _employeeRepository.ListManagers()));
        }
        [HttpGet("{employeeId}/average-order-amount")]
        public async Task<ActionResult<double>> GetAverageOrderAmount(int employeeId)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }
            var employeeEntity = await _employeeRepository.GetEmployeeAsync(restaurantId, employeeId);

            if (employeeEntity is null)
            {
                return NotFound();
            }

            var result = await _orderRepository.CalculateAverageOrderAmount(employeeId);

            return Ok(result);
        }
    }


}
