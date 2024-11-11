using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.CustomerDto;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;
        public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Retrieves all customers with support for pagination and search query.
        /// </summary>
        /// <param name="name">An optional filter to search for customers by name.</param>
        /// <param name="searchQuery">An optional search query to filter customers based on other fields (e.g., email, address).</param>
        /// <param name="pagNumber">The page number for pagination. Defaults to 1.</param>
        /// <param name="pageSize">The number of customers to retrieve per page. Defaults to 10. Maximum allowed value is 10.</param>
        /// <response code="200">Returns a list of customers with pagination information.</response>
        /// <returns>A list of customer information along with pagination metadata.</returns>

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<CustomerInfoDto>>> GetAllAsync(string? name, string? searchQuery,
            int pagNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = 10;
            }

            var (customerEntities, paginationMetadata) = await _customerRepository
                .GetCustomersAsync(name, searchQuery, pagNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<CustomerInfoDto>>(customerEntities));
        }

        /// <summary>
        /// Retrieves a customer by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>A <see cref="CustomerInfoDto"/> representing the customer, or a 404 Not Found response if the customer doesn't exist.</returns>
        /// <response code="200">Returns the customer information.</response>
        /// <response code="404">If the customer with the specified ID is not found.</response>
        [HttpGet("{id}", Name = "GetCustomer")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CustomerInfoDto>> GetCustomerByIdAsync(int id)
        {
            var customerEntity = await _customerRepository.GetCustomerByIdAsync(id);

            if (customerEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CustomerInfoDto>(customerEntity));
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customer">The customer information, provided as a <see cref="CustomerInfoDto"/>.</param>
        /// <returns>A <see cref="CustomerInfoDto"/> representing the created customer, with a location header pointing to the newly created customer.</returns>
        /// <response code="201">Returns the created customer information.</response>
        /// <response code="400">If the provided customer data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CustomerInfoDto>> CreateCustomter(CustomerInfoDto customer)
        {

            var customerEntity = _mapper.Map<Customer>(customer);

            await _customerRepository.AddCustomerAsync(customerEntity);

            await _customerRepository.SaveChangesAsync();



            return CreatedAtRoute("GetCustomer",
                 new
                 {
                     id = customerEntity.Id
                 },
                 customer);
        }

        /// <summary>
        /// Updates an existing customer's information.
        /// </summary>
        /// <param name="customer">The customer information to update, provided as a <see cref="CustomerUpdateDto"/>.</param>
        /// <returns>A <see cref="NoContentResult"/> if the update is successful.</returns>
        /// <response code="204">If the customer is successfully updated.</response>
        /// <response code="404">If the customer with the specified ID is not found.</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateCustomer(CustomerUpdaetDto customer)
        {
            var customerEntity = await _customerRepository.GetCustomerByIdAsync(customer.Id);

            if (customerEntity is null)
            {
                return NotFound();
            }

            _mapper.Map(customer, customerEntity);

            await _customerRepository.SaveChangesAsync();

            return NoContent();
        }

        //[HttpPatch]
        //public async Task<ActionResult> PartiallyUpdateCustomer(int id,
        //    JsonPatchDocument<CustomerUpdaetDto> patchDocument)
        //{
        //    var customerEntity = await _customerRepository.GetCustomerByIdAsync(id);

        //    if (customerEntity is null)
        //    {
        //        return NotFound();
        //    }

        //    var customerToPatch = _mapper.Map<CustomerUpdaetDto>(customerEntity);

        //    patchDocument.ApplyTo(customerToPatch);

        //    if (!TryValidateModel(customerToPatch))
        //    {
        //        return ValidationProblem(ModelState);
        //    }

        //    _mapper.Map(customerToPatch, customerEntity);

        //    await _customerRepository.SaveChangesAsync();

        //    return NoContent();
        //}

        /// <summary>
        /// Deletes a customer by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete.</param>
        /// <returns>A <see cref="NoContentResult"/> if the deletion is successful.</returns>
        /// <response code="204">If the customer is successfully deleted.</response>
        /// <response code="404">If the customer with the specified ID is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customerEntity = await _customerRepository.GetCustomerByIdAsync(id);

            if (customerEntity is null)
            {
                return NotFound();
            }

            await _customerRepository.DeleteCustomerAsync(customerEntity);

            await _customerRepository.SaveChangesAsync();

            return NoContent();

        }
    }
}
