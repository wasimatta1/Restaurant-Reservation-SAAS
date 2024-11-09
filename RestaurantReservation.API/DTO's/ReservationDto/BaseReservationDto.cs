namespace RestaurantReservation.API.DTO_s.ReservationDto
{
    public class BaseReservationDto
    {
        public int CustomerId { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }
    }
}
