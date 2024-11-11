namespace RestaurantReservation.API.DTO_s.CustomerDto
{
    public class BaseCustomerDto
    {
        /// <summary>
        /// first name of the customer
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// last name of the customer
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// the email of the customer
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// the phone number of the customer
        /// </summary>
        public string? PhoneNumber { get; set; }
    }

}
