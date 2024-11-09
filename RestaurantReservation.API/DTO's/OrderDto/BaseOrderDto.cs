namespace RestaurantReservation.API.DTO_s.OrderDto
{
    public class BaseOrderDto
    {
        public int ReservationId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }


    }
}
