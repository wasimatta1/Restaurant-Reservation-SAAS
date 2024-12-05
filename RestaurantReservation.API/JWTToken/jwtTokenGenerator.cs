using Microsoft.IdentityModel.Tokens;
using RestaurantReservation.API.DTO_s.TokenDto;
using RestaurantReservation.Db.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantReservation.API.JWTToken
{
    public class jwtTokenGenerator : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;


        public jwtTokenGenerator(IConfiguration configuration, IEmployeeRepository employeeRepository)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public async Task<JWTToken> GenerateToken(EmployeeTokenDto employee)
        {

            var employeeValue = await _employeeRepository.GetEmployeeAsync(employee.EmployeeId);
            if (employeeValue is null || employee.FirstName != employeeValue.FirstName)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTToken:Key"]));
            var ToeknDescp = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Name", employeeValue.FirstName),
                    new Claim("Id",employeeValue.EmployeeId.ToString()),
                    new Claim("RestaurantId",employeeValue.RestaurantId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(ToeknDescp);

            return new JWTToken
            {
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
