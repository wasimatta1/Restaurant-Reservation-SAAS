using AutoMapper;
using RestaurantReservation.API.DTO_s.OrderDto;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class OrderProfie : Profile
    {
        public OrderProfie()
        {
            CreateMap<Order, OrderInfoDto>()
                .ForMember(dest => dest.MenuItems, opt => opt.MapFrom(src => src.MenuItems));
            CreateMap<OrderUpdateDto, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItemUpdateDto));
            CreateMap<OrderCreateDto, Order>()
                 .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItemCreateDto));
        }
    }
}
