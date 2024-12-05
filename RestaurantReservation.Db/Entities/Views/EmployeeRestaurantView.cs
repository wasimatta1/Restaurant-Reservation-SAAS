

namespace RestaurantReservation.Db.Entities.Views
{
    public class EmployeeRestaurantView
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public string RestaurantPhoneNumber { get; set; }
        public string RestaurantOpeningHours { get; set; }

        public override string ToString()
        {
            return $"Employee: {FirstName} {LastName}, Position: {Position}, " +
                   $"Restaurant: {RestaurantName}, Address: {RestaurantAddress}, " +
                   $"Phone: {RestaurantPhoneNumber}, Opening Hours: {RestaurantOpeningHours}";
        }
    }
}
