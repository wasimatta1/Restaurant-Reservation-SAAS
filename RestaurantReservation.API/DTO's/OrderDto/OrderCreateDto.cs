namespace RestaurantReservation.API.DTO_s.OrderDto
{
    public class OrderCreateDto : BaseOrderDto
    {
        public ICollection<OrderItemCreateDto> OrderItemCreateDto { get; set; } = new List<OrderItemCreateDto>();
    }
}
