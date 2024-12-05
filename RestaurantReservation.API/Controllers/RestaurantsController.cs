using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.RestaurantDto;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Authorize]
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

        /// <summary>
        /// Retrieves a list of restaurants with optional filtering by name and search query.
        /// </summary>
        /// <param name="name">Optional name to filter restaurants by.</param>
        /// <param name="searchQuery">Optional search term to filter restaurants.</param>
        /// <param name="pagNumber">Page number for pagination.</param>
        /// <param name="pageSize">Page size for pagination.</param>
        /// <returns>A list of restaurants matching the search criteria.</returns>
        /// <response code="200">Returns a paginated list of restaurants.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
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

        /// <summary>
        /// Retrieves a specific restaurant by its ID.
        /// </summary>
        /// <param name="id">The ID of the restaurant to retrieve.</param>
        /// <returns>An <see cref="RestaurantInfoDto"/> object representing the restaurant.</returns>
        /// <response code="200">Returns the requested restaurant if found.</response>
        /// <response code="404">If the restaurant is not found.</response>
        [HttpGet("{id}", Name = "GetRestaurant")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RestaurantInfoDto>> GetRestaurantByIdAsync(int id)
        {
            var restaurantEntity = await _restaurantRepository.GetRestaurantByIdAsync(id);

            if (restaurantEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RestaurantInfoDto>(restaurantEntity));
        }

        /// <summary>
        /// Creates a new restaurant.
        /// </summary>
        /// <param name="restaurant">An object containing the details of the restaurant to create.</param>
        /// <returns>The created <see cref="RestaurantInfoDto"/> object.</returns>
        /// <response code="201">Returns the newly created restaurant.</response>
        /// <response code="400">If the provided restaurant data is invalid.</response>

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
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

        /// <summary>
        /// Updates an existing restaurant by its ID.
        /// </summary>
        /// <param name="restaurant">An object containing the updated details of the restaurant.</param>
        /// <returns>A <see cref="NoContentResult"/> if the update is successful.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the provided restaurant data is invalid.</response>
        /// <response code="404">If the restaurant is not found.</response>

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Deletes a specific restaurant by its ID.
        /// </summary>
        /// <param name="id">The ID of the restaurant to delete.</param>
        /// <returns>A <see cref="NoContentResult"/> if the deletion is successful.</returns>
        /// <response code="204">If the deletion is successful.</response>
        /// <response code="404">If the restaurant is not found.</response>

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
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
