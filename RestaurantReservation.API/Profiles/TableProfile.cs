using AutoMapper;
using RestaurantReservation.API.DTO_s.TableDto;
using RestaurantReservation.Db.Entities;

namespace RestaurantReservation.API.Profiles
{
    public class TableProfile : Profile
    {
        public TableProfile()
        {
            CreateMap<Table, TableInfoDto>();
            CreateMap<TableInfoDto, Table>();
            CreateMap<TableUpdateDto, Table>();
            CreateMap<TableCreateDto, Table>();
        }
    }
}
