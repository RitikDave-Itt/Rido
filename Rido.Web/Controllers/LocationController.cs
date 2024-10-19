using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rido.Common.Models.Requests;
using Rido.Common.Models.Responses;
using Rido.Common.Models.Types;
using Rido.Services;
using Rido.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rido.Web.Controllers
{
    [Authorize]

    [ApiController]
    [Route("api/location")]

    public class LocationController : ControllerBase
    {
        private readonly LocationUtils _locationServices;
        private readonly ILogger<DriverLocationController> _logger;


        public LocationController(LocationUtils locationServices , ILogger<DriverLocationController> logger)
        {
            _locationServices = locationServices;
            _logger = logger;
        }

        [HttpGet("nearby-places")]
        public async Task<ActionResult<List<NearbyLocation>>> GetNearbyLocations([FromQuery] GetNearbyLocationRequestDto requestDto)
        {
            if (requestDto == null)
            {
                return BadRequest("request DTO cannot be null.");
            }

            try
            {
                var locations = await _locationServices.GetNearbyLocationsAsync(requestDto);

                if (locations == null || locations.Count == 0)
                {
                    return NotFound("No nearby locations found.");
                }

                return Ok(locations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred retrieving nearby locations.");

                return StatusCode(500, new { Message = "Failed to get nearby locations. .", Error = ex.Message });
            }
        }


        [HttpGet("accurate-address")]
        public async Task<ActionResult<ReverseGeocodeResponseDto>> GetAddress([FromQuery] LocationType requestDto)
        {
            if (requestDto == null)
            {
                return BadRequest("request DTO cannot be null.");
            }

            try
            {
                var result = await _locationServices.GetAddressFromCoordinatesAsync(requestDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred retrieving the address.");

                return StatusCode(500, new { Message = "Failed to retrieve the address. .", Error = ex.Message });
            }
        }

    }
}
