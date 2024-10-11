namespace RestaurantReservation.Db.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }

        override public string ToString()
        {
            return $"OrderItem: {OrderItemId}, Quantity: {Quantity}\n";
        }

    }


}
