namespace RestaurantReservation.API.DTO_s.OrderDto
{
    public class OrderUpdateDto : BaseOrderDto
    {
        public int OrderId { get; set; }
        public ICollection<OrderItemUpdateDto> OrderItemUpdateDto { get; set; } = new List<OrderItemUpdateDto>();
    }
}
