namespace RestaurantReservation.API.DTO_s.RestaurantDto
{
    public class BaseRestaurantDto
    {
        /// <summary>
        /// the name of the restaurant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// the address of the restaurant
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// the phone number of the restaurant
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// the opening hours of the restaurant
        /// </summary>
        public string OpeningHours { get; set; }
    }
}
