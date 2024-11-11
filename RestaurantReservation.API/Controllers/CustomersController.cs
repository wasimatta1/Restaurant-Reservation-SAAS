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

        [HttpGet]
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


        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<ActionResult<CustomerInfoDto>> GetCustomerByIdAsync(int id)
        {
            var customerEntity = await _customerRepository.GetCustomerByIdAsync(id);

            if (customerEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CustomerInfoDto>(customerEntity));
        }

        [HttpPost]
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

        [HttpPut]
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

        [HttpDelete("{id}")]
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
