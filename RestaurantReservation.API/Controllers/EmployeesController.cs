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


        /// <summary>
        /// Retrieves a paginated list of employees, with optional filters for name and search query.
        /// </summary>
        /// <param name="name">An optional parameter to filter employees by name.</param>
        /// <param name="searchQuery">An optional search query to match employee details.</param>
        /// <param name="pagNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page (max is restricted).</param>
        /// <returns>A paginated list of employees matching the filters.</returns>
        /// <response code="200">Returns a list of employees along with pagination metadata in the headers.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
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


        /// <summary>
        /// Retrieves an employee's information by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to retrieve.</param>
        /// <returns>An <see cref="EmployeeInfoDto"/> with the employee's details.</returns>
        /// <response code="200">Returns the employee's details.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the employee with the specified ID is not found.</response>
        [HttpGet("{id}", Name = "GetEmployee")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
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


        /// <summary>
        /// Creates a new employee for the authenticated restaurant.
        /// </summary>
        /// <param name="employee">An <see cref="EmployeeCreateDto"/> object containing the employee's details.</param>
        /// <returns>The created <see cref="EmployeeInfoDto"/> with the employee's information.</returns>
        /// <response code="201">Returns the newly created employee.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="400">If the provided employee data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
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

        /// <summary>
        /// Updates the details of an existing employee for the authenticated restaurant.
        /// </summary>
        /// <param name="employee">An <see cref="EmployeeUpdateDto"/> object containing the updated employee information.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        /// <response code="400">If the provided employee data is invalid.</response>
        /// <response code="204">Indicates that the employee details were successfully updated.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the employee with the specified ID is not found for the given Restaurant ID.</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
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


        /// <summary>
        /// Deletes an employee from the authenticated restaurant's records.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <response code="204">Indicates that the employee was successfully deleted.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the employee with the specified ID is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Retrieves a list of all managers in the restaurant.
        /// </summary>
        /// <returns>A list of managers as <see cref="EmployeeInfoDto"/> objects.</returns>
        /// <response code="200">Returns the list of managers.</response>
        [HttpGet("managers")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<EmployeeInfoDto>>> GetManagersAsync()
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeInfoDto>>(await _employeeRepository.ListManagers()));
        }


        /// <summary>
        /// Retrieves the average order amount for a specified employee in the authenticated restaurant.
        /// </summary>
        /// <param name="employeeId">The unique identifier of the employee for whom the average order amount is calculated.</param>
        /// <returns>The average order amount for the specified employee.</returns>
        /// <response code="200">Returns the average order amount.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the specified employee is not found in the restaurant.</response>
        [HttpGet("{employeeId}/average-order-amount")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
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
