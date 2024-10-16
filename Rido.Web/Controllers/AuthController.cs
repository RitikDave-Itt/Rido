using Microsoft.AspNetCore.Mvc;
using Rido.Common.Models.Requests;
using Rido.Services.Interfaces;
using Rido.Common.Attributes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Rido.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;

        public AuthController(IAuthServices authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RegisterUserAsync(request);
                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(500, "An error occurred while creating the user.");
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = "Failed to register the user. Please try again later.", Error = ex.Message });
            }
        }


        [HttpPost("login")]
        [PrintHello]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto request)
        {
            try
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
                    Expires = DateTimeOffset.Now.AddMinutes(60),
                    Path = "/"
                };

                Response.Cookies.Append("access_token", result.Token, cookieOptions);

                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = "Failed to log in. Please try again later.", Error = ex.Message });
            }
        }

    }
}
