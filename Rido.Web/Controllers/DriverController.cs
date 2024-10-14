using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rido.Common.Models.Requests;
using Rido.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rido.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly ILogger<DriverController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DriverController(IDriverService driverService 
            ,ILogger<DriverController> logger
            ,IHttpContextAccessor httpContextAccessor
            )
        {
            _driverService = driverService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterDriver([FromBody] RegisterDriverDto registerDriverDto)
        {
            try
            {
                if (registerDriverDto == null)
                {
                    return BadRequest("Driver information is required.");
                }

                var driverId = await _driverService.RegisterDriverAsync(registerDriverDto);

                if (string.IsNullOrEmpty(driverId))
                {
                    return StatusCode(500, "An error occurred while registering the driver.");
                }

                return CreatedAtAction(nameof(RegisterDriver), new { id = driverId }, registerDriverDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the driver.");

                return StatusCode(500, new { Message = "Failed to register the driver. Please try again later.", Error = ex.Message });
            }
        }


    }
}
