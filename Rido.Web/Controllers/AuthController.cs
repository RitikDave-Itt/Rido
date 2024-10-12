using Microsoft.AspNetCore.Mvc;
using Rido.Common.Models.Requests;
using Rido.Services.Interfaces;
using Rido.Common.Attributes;
using System.Threading.Tasks;

namespace Rido.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;

        public AuthController(IAuthServices authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user with the provided details.
        /// </summary>
        /// <param name="request">User registration details including email, phone number, password, and full name.</param>
        /// <returns>Returns a success message if the user is registered successfully, otherwise returns an error.</returns>
        /// <response code="200">Returns success status if registration is successful.</response>
        /// <response code="400">Returns if the request data is invalid.</response>
        /// <response code="500">Returns if there is an error during user registration.</response>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterUserAsync(request);
            if (!result)
            {
                return StatusCode(500, "An error occurred while creating the user.");
            }

            return Ok(new { success = true });
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token in a secure cookie.
        /// </summary>
        /// <param name="request">User login credentials, including email and password.</param>
        /// <returns>Returns the JWT token upon successful authentication.</returns>
        /// <response code="200">Returns the token if login is successful.</response>
        /// <response code="400">Returns if the login request data is invalid.</response>
        /// <response code="500">Returns if the login fails due to incorrect credentials.</response>
        [HttpPost("login")]
        [PrintHello]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto request)
        {
            var result = await _authService.LoginUserAsync(request);

            if (!result.Success)
            {
                return StatusCode(500, "Email or Password Invalid!");
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                Path = "/"
            };

            Response.Cookies.Append("access_token", result.Token, cookieOptions);

            return Ok(result);
        }
    }
}
