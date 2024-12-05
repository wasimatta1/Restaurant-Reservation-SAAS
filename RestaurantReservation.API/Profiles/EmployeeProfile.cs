using AutoMapper;
using RestaurantReservation.API.DTO_s.EmployeeDto;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeInfoDto>();
            CreateMap<EmployeeInfoDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
            CreateMap<EmployeeCreateDto, Employee>();
        }
    }

}
