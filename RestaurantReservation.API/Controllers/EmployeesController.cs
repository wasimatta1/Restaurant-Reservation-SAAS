using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.EmployeeDto;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("api/restaurants/{restaurantId}/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;
        public EmployeesController(IEmployeeRepository employeeRepository, IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeInfoDto>>> GetAllAsync(int restaurantId, string? name, string? searchQuery,
            int pagNumber = 1, int pageSize = 10)
        {
            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
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
        public async Task<ActionResult<EmployeeInfoDto>> GetEmployeeByIdAsync(int restaurantId, int id)
        {
            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }

            var employeeEntity = await _employeeRepository.GetEmployeeByIdAsync(restaurantId, id);

            if (employeeEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EmployeeInfoDto>(employeeEntity));
        }
        [HttpPost]
        public async Task<ActionResult<EmployeeInfoDto>> CreateEmployee(int restaurantId, EmployeeCreateDto employee)
        {

            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
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
        public async Task<ActionResult> UpdateEmployee(int restaurantId, EmployeeUpdateDto employee)
        {
            var employeeEntity = await _employeeRepository.GetEmployeeByIdAsync(restaurantId, employee.EmployeeId);

            if (employeeEntity is null)
            {
                return NotFound();
            }

            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }
            if (!await _restaurantRepository.RestaurantExistsAsync(employeeEntity.RestaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }

            _mapper.Map(employee, employeeEntity);

            await _employeeRepository.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int restaurantId, int id)
        {

            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }
            var employeeEntity = await _employeeRepository.GetEmployeeByIdAsync(restaurantId, id);

            if (employeeEntity is null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteEmployeeAsync(employeeEntity);

            await _employeeRepository.SaveChangesAsync();

            return NoContent();

        }
    }


}
