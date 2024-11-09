using AutoMapper;
using RestaurantReservation.API.DTO_s.ReservationDto;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationInfoDto>();
            CreateMap<ReservationCreateDto, Reservation>();
            CreateMap<ReservationUpdateDto, Reservation>();
        }
    }
}
