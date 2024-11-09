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
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<TableInfoDto>>> GetAllAsync(
            string? capacity, int pagNumber = 1, int pageSize = 10)
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

            var (tableEntities, paginationMetadata) = await _tableRepository
                .GetTablesAsync(restaurantId, capacity, pagNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<TableInfoDto>>(tableEntities));
        }

        [HttpGet("{id}", Name = "GetTable")]
        public async Task<ActionResult<TableInfoDto>> GetTableByIdAsync(int id)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }

            var tableEntity = await _tableRepository.GetTableByIdAsync(restaurantId, id);

            if (tableEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TableInfoDto>(tableEntity));
        }

        [HttpPost]
        public async Task<ActionResult<TableInfoDto>> CreateTable(TableCreateDto table)
        {

            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
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
        public async Task<ActionResult> UpdateTable(TableUpdateDto table)
        {
            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
            }
            var tablEntity = await _tableRepository.GetTableByIdAsync(
                restaurantId, table.TableId);

            if (tablEntity is null)
            {
                return NotFound();
            }

            _mapper.Map(table, tablEntity);

            await _tableRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTable(int id)
        {

            var restaurantIdClaim = User.FindFirst("RestaurantId");

            if (restaurantIdClaim == null || !int.TryParse(restaurantIdClaim.Value, out int restaurantId))
            {
                return Unauthorized("Restaurant ID not found in token.");
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
