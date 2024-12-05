namespace RestaurantReservation.Db.Entities.Procedures_Models
{
    public class CustomerReservation
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int PartySize { get; set; }

        public override string ToString()
        {
            return $"Customer: {CustomerName}, Party Size: {PartySize}\n";
        }
    }
}
