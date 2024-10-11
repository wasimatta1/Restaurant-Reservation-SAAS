namespace RestaurantReservation.Db.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ReservationId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public Employee Employee { get; set; }
        public Reservation Reservation { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        override public string ToString()
        {
            var result = $"Order: {OrderId}, Date: {OrderDate}, Total: {TotalAmount}\n";
            if (MenuItems != null)
            {
                foreach (var item in MenuItems)
                {
                    var quantity = OrderItems.FirstOrDefault(oi => oi.ItemId == item.ItemId)?.Quantity;
                    result += $"\t --Quantity: {quantity}, {item.ToString()}";

                }
            }
            return result;
        }
    }


}
