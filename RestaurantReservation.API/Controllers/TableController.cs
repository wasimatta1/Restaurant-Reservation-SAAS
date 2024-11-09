using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.TableDto;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.Text.Json;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/restaurants/{restaurantId}/[controller]")]
    public class TableController : Controller
    {

        private readonly ITableRepository _tableRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public TableController(ITableRepository tableRepository, IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            _tableRepository = tableRepository ?? throw new ArgumentNullException(nameof(tableRepository));
            _restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableInfoDto>>> GetAllAsync(int restaurantId,
            string? capacity, int pagNumber = 1, int pageSize = 10)
        {
            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }

            if (pageSize > maxCitiesPageSize)
            {
                pageSize = 10;
            }

            var (tableEntities, paginationMetadata) = await _tableRepository
                .GetTablesAsync(restaurantId, capacity, pagNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<TableInfoDto>>(tableEntities));
        }

        [HttpGet("{id}", Name = "GetTable")]
        public async Task<ActionResult<TableInfoDto>> GetTableByIdAsync(int restaurantId, int id)
        {
            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }

            var tableEntity = await _tableRepository.GetTableByIdAsync(restaurantId, id);

            if (tableEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TableInfoDto>(tableEntity));
        }

        [HttpPost]
        public async Task<ActionResult<TableInfoDto>> CreateTable(
            int restaurantId, TableCreateDto table)
        {

            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }


            var tableEntity = _mapper.Map<Table>(table);

            tableEntity.RestaurantId = restaurantId;

            await _tableRepository.AddTableAsync(tableEntity);

            await _tableRepository.SaveChangesAsync();

            return CreatedAtRoute("GetTable",
                 new
                 {
                     restaurantId = restaurantId,
                     id = tableEntity.TableId
                 },
                 table);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTable(int restaurantId, TableUpdateDto table)
        {
            var tablEntity = await _tableRepository.GetTableByIdAsync(
                restaurantId, table.TableId);

            if (tablEntity is null)
            {
                return NotFound();
            }

            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }
            if (!await _restaurantRepository.RestaurantExistsAsync(tablEntity.RestaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }

            _mapper.Map(table, tablEntity);

            await _tableRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTable(int restaurantId, int id)
        {

            if (!await _restaurantRepository.RestaurantExistsAsync(restaurantId))
            {
                return BadRequest("Restaurant Not Found");
            }
            var tableEntity = await _tableRepository.GetTableByIdAsync(restaurantId, id);

            if (tableEntity is null)
            {
                return NotFound();
            }

            await _tableRepository.DeleteTableAsync(tableEntity);

            await _tableRepository.SaveChangesAsync();

            return NoContent();

        }

    }

}
