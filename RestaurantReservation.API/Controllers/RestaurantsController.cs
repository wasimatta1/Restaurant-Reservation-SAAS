using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.RestaurantDto;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;
        public RestaurantsController(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantInfoDto>>> GetAllAsync(
            string? name, string? searchQuery,
            int pagNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = 10;
            }

            var (restaurantEntities, paginationMetadata) = await _restaurantRepository
                .GetRestaurantAsync(name, searchQuery, pagNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<RestaurantInfoDto>>(restaurantEntities));
        }
        [HttpGet("{id}", Name = "GetRestaurant")]
        public async Task<ActionResult<RestaurantInfoDto>> GetRestaurantByIdAsync(int id)
        {
            var restaurantEntity = await _restaurantRepository.GetRestaurantByIdAsync(id);

            if (restaurantEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RestaurantInfoDto>(restaurantEntity));
        }
        [HttpPost]
        public async Task<ActionResult<RestaurantInfoDto>> CreateRestaurant(RestaurantInfoDto restaurant)
        {
            var restaurantEntity = _mapper.Map<Restaurant>(restaurant);

            await _restaurantRepository.AddRestaurantAsync(restaurantEntity);

            await _restaurantRepository.SaveChangesAsync();

            return CreatedAtRoute("GetRestaurant",
                 new
                 {
                     id = restaurantEntity.RestaurantId
                 },
                 restaurant);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateRestaurant(RestaurantUpdateDto restaurant)
        {
            var restaurantEntity = await _restaurantRepository.GetRestaurantByIdAsync(restaurant.RestaurantId);

            if (restaurantEntity is null)
            {
                return NotFound();
            }

            _mapper.Map(restaurant, restaurantEntity);

            await _restaurantRepository.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRestaurant(int id)
        {
            var restaurantEntity = await _restaurantRepository.GetRestaurantByIdAsync(id);

            if (restaurantEntity is null)
            {
                return NotFound();
            }

            await _restaurantRepository.DeleteRestaurantAsync(restaurantEntity);

            await _restaurantRepository.SaveChangesAsync();

            return NoContent();

        }
    }
}
