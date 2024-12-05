namespace RestaurantReservation.API.DTO_s.ReservationDto
{
    public class BaseReservationDto
    {
        /// <summary>
        /// the id of the customer who made the reservation
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// the id of the table that the reservation is for
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// the date and time of the reservation
        /// </summary>
        public DateTime ReservationDate { get; set; }

        /// <summary>
        /// the size of the party for the reservation
        /// </summary>
        public int PartySize { get; set; }
    }
}
