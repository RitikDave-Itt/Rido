using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rido.Common.Models.Types;
using Rido.Common.Utils;

namespace Rido.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class RideController : ControllerBase
    {
        private ILogger _logger;
        public RideController(ILogger logger)
        {

            _logger = logger;
        }


        [HttpGet]
        [Route("fare-list")]

        public async Task<ActionResult<List<FareType>>> GetFareList([FromQuery] string pickup, [FromQuery] string destination)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pickup) || string.IsNullOrWhiteSpace(destination))
                {
                    return BadRequest("Pickup and destination cannot be null or empty.");
                }

                var pickupCoordinates = pickup.Split(',');
                var destinationCoordinates = destination.Split(',');

                if (pickupCoordinates.Length != 2 || destinationCoordinates.Length != 2)
                {
                    return BadRequest("Invalid pickup or destination coordinates format. Please provide coordinates in 'latitude,longitude' format.");
                }

                LocationType pickupLocation = new LocationType { Latitude = pickupCoordinates[0], Longitude = pickupCoordinates[1] };
                LocationType destinationLocation = new LocationType { Latitude = destinationCoordinates[0], Longitude = destinationCoordinates[1] };

                RideCalculations rideCalculations = new RideCalculations();

                var fareList = rideCalculations.FareList(pickupLocation, destinationLocation);
                return Ok(fareList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the fare list.");

                return StatusCode(500, new { Message = "Failed to retrieve the fare list. Please try again later.", Error = ex.Message });
            }
        }





    }
}
