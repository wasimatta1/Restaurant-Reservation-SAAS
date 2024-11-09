using RestaurantReservation.API.DTO_s.MenuItenDto;

namespace RestaurantReservation.API.DTO_s.OrderDto
{
    public class OrderInfoDto : BaseOrderDto
    {
        public ICollection<MenuItemInfoDto> MenuItems { get; set; } = new List<MenuItemInfoDto>();
    }
}
