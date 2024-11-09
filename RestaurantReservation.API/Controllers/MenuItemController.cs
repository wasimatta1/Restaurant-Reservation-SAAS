using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.MenuItenDto;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("api/restaurants/{restaurantId}/[controller]")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemInfoDto>>> GetAllAsync(
        int restaurantId, string? name, string? searchQuery, int pagNumber = 1, int pageSize = 10)
        {
            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
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
        [HttpGet("{id}", Name = "GetMenuItem")]
        public async Task<ActionResult<MenuItemInfoDto>> GetMenuItemAsync(int restaurantId, int id)
        {
            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }

            var menuItemEntity = await _menuItemRepository.GetMenuItemAsync(restaurantId, id);

            if (menuItemEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MenuItemInfoDto>(menuItemEntity));
        }


        [HttpPost]
        public async Task<ActionResult<MenuItemInfoDto>> CreateMenuItem(
        int restaurantId, MenuItemCreateDto menuItem)
        {

            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
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


        [HttpPut]
        public async Task<ActionResult> UpdateMenuItem(int restaurantId, MenuItemUpdateDto menuItem)
        {
            var menuItemEntity = await _menuItemRepository.GetMenuItemAsync(restaurantId, menuItem.ItemId);

            if (menuItemEntity is null)
            {
                return NotFound();
            }

            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }

            _mapper.Map(menuItem, menuItemEntity);

            await _menuItemRepository.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMenuItem(int restaurantId, int id)
        {

            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
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

