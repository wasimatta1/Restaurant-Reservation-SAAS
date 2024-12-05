namespace RestaurantReservation.API.DTO_s.MenuItenDto
{
    public class BaseMenuItemDto
    {
        /// <summary>
        /// the name of the menu item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// the description of the menu item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// the price of the menu item
        /// </summary>
        public decimal Price { get; set; }
    }
}
