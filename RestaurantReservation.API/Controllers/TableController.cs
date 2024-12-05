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

        /// <summary>
        /// Retrieves a list of tables with optional filtering by capacity.
        /// </summary>
        /// <param name="capacity">Optional capacity to filter tables by.</param>
        /// <param name="pagNumber">Page number for pagination.</param>
        /// <param name="pageSize">Page size for pagination.</param>
        /// <returns>A list of tables matching the search criteria.</returns>
        /// <response code="200">Returns a paginated list of tables.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
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

        /// <summary>
        /// Retrieves a specific table by its ID.
        /// </summary>
        /// <param name="id">The ID of the table to retrieve.</param>
        /// <returns>A <see cref="TableInfoDto"/> object representing the table.</returns>
        /// <response code="200">Returns the requested table if found.</response>
        /// <response code="404">If the table is not found.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>

        [HttpGet("{id}", Name = "GetTable")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Creates a new table for the restaurant.
        /// </summary>
        /// <param name="table">An object containing the details of the table to create.</param>
        /// <returns>The created <see cref="TableInfoDto"/> object.</returns>
        /// <response code="201">Returns the newly created table.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="400">If the provided table data is invalid.</response>

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
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

        /// <summary>
        /// Updates an existing table by its ID.
        /// </summary>
        /// <param name="table">An object containing the updated details of the table.</param>
        /// <returns>A <see cref="NoContentResult"/> if the update is successful.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the table is not found.</response>

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Deletes a specific table by its ID.
        /// </summary>
        /// <param name="id">The ID of the table to delete.</param>
        /// <returns>A <see cref="NoContentResult"/> if the deletion is successful.</returns>
        /// <response code="204">If the deletion is successful.</response>
        /// <response code="401">If the Restaurant ID is not found in the token.</response>
        /// <response code="404">If the table is not found.</response>

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
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
