namespace RestaurantReservation.API.DTO_s.OrderDto
{
    public class OrderItemCreateDto
    {
        /// <summary>
        /// the id of the MenuItem
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// the quantity of the MenuItem
        /// </summary>
        public int Quantity { get; set; }
    }
}
