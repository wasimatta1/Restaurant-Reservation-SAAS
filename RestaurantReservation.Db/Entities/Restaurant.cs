namespace RestaurantReservation.Db.Entities
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string OpeningHours { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public ICollection<Table> Tables { get; set; } = new List<Table>();

        override public string ToString()
        {
            return $"Restaurant: {Name}, Address: {Address}, Phone Number: {PhoneNumber}\n";
        }
    }


}
