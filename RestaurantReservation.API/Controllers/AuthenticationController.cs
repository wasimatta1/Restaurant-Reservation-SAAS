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

        /// <summary>
        /// Authenticates an employee by generating a JWT token if the employee is valid.
        /// </summary>
        /// <param name="employee">An object containing the employee's credentials for authentication (username(first name), password(Id)).</param>
        /// <response code="200">Returns the JWT tokens.</response>
        /// <response code="401">If the employee is not signed up.</response>
        /// <returns> A JWT tokens. </returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> Authenticate(EmployeeTokenDto employee)
        {
            var tokens = await _jwtTokenService.GenerateToken(employee);
            if (tokens == null)
            {
                return Unauthorized("User Not signed UP");
            }
            return Ok(tokens);
        }
    }
}
