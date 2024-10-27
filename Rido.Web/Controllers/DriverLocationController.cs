using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rido.Services.Interfaces;
using Rido.Data.Enums;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Rido.Data.DataTypes;

namespace Rido.Web.Controllers
{
    [Authorize(Roles = "Driver")]
    [ApiController]
    [Route("/api/driver-location")]
    public class DriverLocationController : ControllerBase
    {
        private readonly IDriverLocationService _driverLocationService;
        private readonly ILogger<DriverLocationController> _logger;


        public DriverLocationController(IDriverLocationService driverLocationService, ILogger<DriverLocationController> logger)
        {
            _driverLocationService = driverLocationService;
            _logger = logger;
        }

        [HttpPost]
        [Route("update-location")]
        public async Task<IActionResult> UpdateLocation( [FromQuery] string lat ,string lon ,VehicleType vehicleType)
        {
            try
            {
              
                if (lat == null || lon==null)
                {
                    _logger.LogWarning("Invalid input data for updating location.");
                    return BadRequest("Invalid input data.");
                }

                string updateResult = await _driverLocationService.UpdateLocation(new LocationType() { Latitude=lat,Longitude=lon}, vehicleType);

                if (!updateResult.IsNullOrEmpty())
                {
                    return Ok(new { Message = "Driver location updated successfully.", Geohash = updateResult });
                }
                else
                {
                    _logger.LogWarning("Failed to update driver location.");
                    return StatusCode(500, new { Message = "Failed to update driver location." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating driver location.");
                return StatusCode(500, new { Message = "Failed to update driver location." ,error = ex.Message });
            }
        }
    }
}
