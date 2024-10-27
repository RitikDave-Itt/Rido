using Microsoft.AspNetCore.Mvc;
using Rido.Common.Models.Requests;
using Rido.Services.Interfaces;
using Rido.Common.Attributes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Rido.Data.Entities;
using Rido.Data.Enums;
using AutoMapper;
using Rido.Services;

namespace Rido.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController<AuthController>
    {
        private readonly IAuthServices _authService;
        private readonly IBaseService<DriverData> _driverService;
        private readonly IBaseService<User> _userService;


        public AuthController(IAuthServices authService,IBaseService<RideRequest> rideRequestService,IBaseService<DriverData> driverService , IServiceProvider serviceProvider,IBaseService<User> userService):base(serviceProvider)
        {
            _authService = authService;
            _driverService = driverService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] dynamic request)
        {

            try
            {


                var userDto = _mapper.Map<RegisterUserDto>(request);
                RegisterDriverDto driverDto = new RegisterDriverDto();
                
                if (userDto.Role == UserRole.Driver)
                {
                     driverDto = _mapper.Map<RegisterDriverDto>(request);

                }
                else
                {
                    driverDto = null;
                }
                var resultUserSave = await _authService.RegisterUserAsync(userDto, driverDto);




                if (string.IsNullOrEmpty(resultUserSave))
                {
                    return StatusCode(500, "An error occurred while creating the user.");
                }

               

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"\n\n\n Exception in User Registration \n\n\n{request}");

                

                return StatusCode(500, new { Message = "Failed to register the user.", Error = ex.Message });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto request)
        {
            try
            {
                var result = await _authService.LoginUserAsync(request);

                if (!result.Success)
                {
                    return StatusCode(401, "Email or Password Invalid!");
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,  
                    Secure = true,
                    Expires = DateTimeOffset.Now.AddMinutes(60),
                    Path = "/"
                };

                Response.Cookies.Append("accessToken", result?.Token, cookieOptions);
                Response.Cookies.Append("refreshToken", result?.RefreshToken, cookieOptions);


                return Ok(new
                {
                    AccessToken = result.Token,
                    RefreshToken = result?.RefreshToken,
                    User = result?.User,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "eXCEPTION IN LOGIN");

                return StatusCode(500, new { Message = "Failed to log in. Please try again later.", Error = ex.Message });
            }
        }

      


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshAndVerifyToken()
        {
            try {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

                if (authHeader == null)
                {
                    return Unauthorized("Refresh Token Not Found");
                }
                var refreshTokenString = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c=>c.Type=="refreshToken");
                
                if (refreshTokenString == null)
                {
                    return Unauthorized("Refresh Token Not Found");
                }

                var (IsValid,RefreshToken,AccessToken) = await _authService.VerifyAndGenerateRefreshToken(refreshTokenString.Value);

                if (!IsValid) {
                    return Unauthorized("Refresh Token Expired Or Not Found");

                }

                return Ok(new {IsValid,RefreshToken,AccessToken});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EXCEPTION IN REFRESH TOKEN");
                return BadRequest(ex);

            }







        }
        



    }
}
