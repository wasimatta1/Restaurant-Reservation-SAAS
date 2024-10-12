namespace RestaurantReservation.Db.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public override string ToString()
        {
            return $"Customer: {FirstName} {LastName}, Email: {Email}, Phone Number: {PhoneNumber}\n";
        }
    }
}
