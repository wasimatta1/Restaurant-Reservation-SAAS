using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.MenuItenDto;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public MenuItemController(IMenuItemRepository menuItemRepository, IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _menuItemRepository = menuItemRepository ?? throw new ArgumentNullException(nameof(menuItemRepository));
            _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Retrieves a paginated list of menu items for the authenticated restaurant, with optional search and filtering.
        /// </summary>
        /// <param name="name">An optional filter for the name of menu items.</param>
        /// <param name="searchQuery">An optional search query to further filter the menu items.</param>
        /// <param name="pagNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page for pagination. The maximum allowed value is limited by <c>maxCitiesPageSize</c>.</param>
        /// <returns>A paginated list of <see cref="MenuItemInfoDto"/> objects representing the menu items.</returns>
        /// <response code="200">Returns the paginated list of menu items.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<MenuItemInfoDto>>> GetAllAsync(
        string? name, string? searchQuery, int pagNumber = 1, int pageSize = 10)
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

            var (MenuItemEntities, paginationMetadata) = await _menuItemRepository
                .GetMenuItemsAsync(restaurantId, name, searchQuery, pagNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<MenuItemInfoDto>>(MenuItemEntities));
        }


        /// <summary>
        /// Retrieves a specific menu item by its ID for the authenticated restaurant.
        /// </summary>
        /// <param name="id">The ID of the menu item to retrieve.</param>
        /// <returns>A <see cref="MenuItemInfoDto"/> object representing the menu item.</returns>
        /// <response code="200">Returns the requested menu item if found.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the menu item is not found.</response>
        [HttpGet("{id}", Name = "GetMenuItem")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MenuItemInfoDto>> GetMenuItemAsync(int id)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }

            var menuItemEntity = await _menuItemRepository.GetMenuItemAsync(restaurantId, id);

            if (menuItemEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MenuItemInfoDto>(menuItemEntity));
        }

        /// <summary>
        /// Creates a new menu item for the authenticated restaurant.
        /// </summary>
        /// <param name="menuItem">An object containing the details of the menu item to create.</param>
        /// <returns>The created <see cref="MenuItemInfoDto"/> object.</returns>
        /// <response code="201">Returns the newly created menu item.</response>
        /// <response code="400">If the provided menu item data is invalid.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<MenuItemInfoDto>> CreateMenuItem(
        MenuItemCreateDto menuItem)
        {

            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }


            var menuItemEntity = _mapper.Map<MenuItem>(menuItem);

            menuItemEntity.RestaurantId = restaurantId;

            await _menuItemRepository.AddMenuItemAsync(menuItemEntity);

            await _menuItemRepository.SaveChangesAsync();

            return CreatedAtRoute("GetMenuItem",
                 new
                 {
                     restaurantId = restaurantId,
                     id = menuItemEntity.ItemId
                 },
                 menuItem);
        }

        /// <summary>
        /// Updates an existing menu item for the authenticated restaurant.
        /// </summary>
        /// <param name="menuItem">An object containing the updated details of the menu item.</param>
        /// <returns>A <see cref="NoContentResult"/> if the update is successful.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the provided menu item data is invalid.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the menu item is not found.</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateMenuItem(MenuItemUpdateDto menuItem)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }
            var menuItemEntity = await _menuItemRepository.GetMenuItemAsync(restaurantId, menuItem.ItemId);

            if (menuItemEntity is null)
            {
                return NotFound();
            }

            _mapper.Map(menuItem, menuItemEntity);

            await _menuItemRepository.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Deletes a specific menu item by its ID for the authenticated restaurant.
        /// </summary>
        /// <param name="id">The ID of the menu item to delete.</param>
        /// <returns>A <see cref="NoContentResult"/> if the deletion is successful.</returns>
        /// <response code="204">If the deletion is successful.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the menu item is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteMenuItem(int id)
        {

            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }
            var menuItemEntity = await _menuItemRepository.GetMenuItemAsync(restaurantId, id);

            if (menuItemEntity is null)
            {
                return NotFound();
            }

            await _menuItemRepository.DeleteMenuItemAsync(menuItemEntity);

            await _menuItemRepository.SaveChangesAsync();

            return NoContent();

        }
    }
}

