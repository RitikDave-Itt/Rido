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
        private readonly IBaseService<RideRequest> _rideRequestService;
        private readonly IBaseService<DriverData> _driverService;
        private readonly IBaseService<User> _userService;


        public AuthController(IAuthServices authService,IBaseService<RideRequest> rideRequestService,IBaseService<DriverData> driverService , IServiceProvider serviceProvider,IBaseService<User> userService):base(serviceProvider)
        {
            _authService = authService;
            _rideRequestService = rideRequestService;
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
                _logger.LogError(ex,"\n\n\n Exception in User Registration \n\n\n");

                

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

        [HttpPost("verify-otp")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto)
        {
            try
            {
                var verify = await _authService.VerifyOTP(verifyOtpDto.Otp, verifyOtpDto.RideRequestId);

                if (verify == OTPVerificationStatus.Success)
                {
                    var rideRequest = await _rideRequestService.GetByIdAsync(verifyOtpDto.RideRequestId);
                    rideRequest.Status = RideRequestStatus.InProgress;
                    var updateRideRequest = await _rideRequestService.UpdateAsync(rideRequest);

                    return Ok(new { Success = true });

                }
                else if (verify == OTPVerificationStatus.InvalidRideRequestStatus)
                {
                    return NotFound(new { success = false, Message = "Invalid ride Request" });
                }
                else if (verify == OTPVerificationStatus.InvalidOTP)
                {
                    return Unauthorized(new { success = false, Message = "Invalid OTP " });

                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex) {

                _logger.LogError(ex, "Something Went Wrong in Otp Verification");
                return BadRequest();
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
                var refreshToken = authHeader.Substring("Bearer ".Length).Trim();

                var (IsValid,RefreshToken,JwtToken) = await _authService.VerifyAndGenerateRefreshToken(refreshToken);

                if (!IsValid) {
                    return Unauthorized("Refresh Token Expired Or Not Found");

                }

                return Ok(new {IsValid,RefreshToken,JwtToken});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EXCEPTION IN REFRESH TOKEN");
                return BadRequest(ex);

            }





        }


    }
}
