using AutoMapper;
using RestaurantReservation.API.DTO_s.RestaurantDto;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<Restaurant, RestaurantInfoDto>();
            CreateMap<RestaurantInfoDto, Restaurant>();
            CreateMap<RestaurantUpdateDto, Restaurant>();
        }
    }
}
