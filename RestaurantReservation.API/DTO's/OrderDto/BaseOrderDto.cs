namespace RestaurantReservation.API.DTO_s.OrderDto
{
    public class BaseOrderDto
    {
        /// <summary>
        /// the id of the reservation of the order
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// the id of the employee who took the order
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// the date of the order
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// the total amount of the order
        /// </summary>
        public decimal TotalAmount { get; set; }


    }
}
