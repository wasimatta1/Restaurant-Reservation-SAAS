using AutoMapper;
using RestaurantReservation.API.DTO_s.CustomerDto;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerInfoDto>();
            CreateMap<CustomerInfoDto, Customer>();
            CreateMap<CustomerUpdaetDto, Customer>();
        }
    }
}
