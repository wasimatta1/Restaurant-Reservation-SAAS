using RestaurantReservation.API.DTO_s.TokenDto;

namespace RestaurantReservation.API.JWTToken
{
    public interface IJwtTokenService
    {
        public Task<JWTToken> GenerateToken(EmployeeTokenDto employee);

    }
}
