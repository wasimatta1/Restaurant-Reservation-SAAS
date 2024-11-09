using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTO_s.TokenDto;
using RestaurantReservation.API.JWTToken;

namespace RestaurantReservation.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IJwtTokenService _jwtTokenService;

        public AuthenticationController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        public async Task<ActionResult> Authenticate(EmployeeTokenDto employee)
        {
            var tokens = await _jwtTokenService.GenerateToken(employee);
            if (tokens == null)
            {
                return Unauthorized("User Not Sign UP");
            }
            return Ok(tokens);
        }
    }
}
