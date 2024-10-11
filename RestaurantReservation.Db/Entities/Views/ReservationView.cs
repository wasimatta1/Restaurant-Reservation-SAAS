

namespace RestaurantReservation.Db.Entities.Views
{
    public class ReservationView
    {
        public int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }

        public override string ToString()
        {
            return $"Reservation: {ReservationId}, Date: {ReservationDate}, " +
                $"Party Size: {PartySize}, Customer: {CustomerFirstName}, " +
                $"Phone Number: {CustomerPhoneNumber}, Restaurant: {RestaurantName}, " +
                $"Address: {RestaurantAddress}\n";
        }
    }
}
