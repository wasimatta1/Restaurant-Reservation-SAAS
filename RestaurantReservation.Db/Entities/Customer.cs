namespace RestaurantReservation.Db.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? Email { get; set; }
        public string? Phone_Number { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public override string ToString()
        {
            return $"Customer: {First_Name} {Last_Name}, Email: {Email}, Phone Number: {Phone_Number}\n";
        }
    }


}
