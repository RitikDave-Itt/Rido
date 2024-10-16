using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Rido.Common.Models.Types;
using Rido.Common.Utils;
using Rido.Data.DTOs;
using Rido.Services;
using Rido.Services.Interfaces;

namespace Rido.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class RideController : BaseController<RideController> 
    {
        private readonly IRideService _rideService;

        public RideController(IRideService rideService, ILogger<RideController> logger ,IHttpContextAccessor httpContextAccessor ): base(logger, httpContextAccessor)
        {
            _rideService = rideService;
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

                return StatusCode(500, new { Message = "Failed to retrieve the fare list. Please try again later.", Error = ex.Message });
            }
        }


        [HttpPost]
        [Route("ride-request")]

        public async Task<IActionResult> CreateRideRequest([FromBody] RideRequestDto rideRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);      
                }


                var result = await _rideService.CreateRideRequest(rideRequest);

                return Ok(new { Message = "Ride Requested Successfully", Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Error = ex.Message });
            }
        }
        [Authorize(Roles ="Driver")]
        [HttpGet("Location")]
        public async Task<IActionResult> GetRideRequestByLocation([FromQuery] LocationType location)
        {
            try
            {
                var rideRequests = await _rideService.GetRideRequestByLocation(location);
                return Ok(rideRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving ride requests by location.");
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpDelete("CancelByUser/{rideRequestId}")]
        public async Task<IActionResult> CancelRideByUser(string rideRequestId)
        {
            try
            {
                var result = await _rideService.CancelRideByUser(rideRequestId);
                if (result)
                {
                    return NoContent();
                }
                return NotFound("Ride request not found or could not be canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while canceling the ride by user.");
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpPut("CancelByDriver/{rideRequestId}")]
        public async Task<IActionResult> CancelRideByDriver(string rideRequestId)
        {
            try
            {
                var result = await _rideService.CancelRideByDriver(rideRequestId);
                if (result)
                {
                    return NoContent();
                }
                return NotFound("Ride request not found or could not be canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while canceling the ride by driver.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("AssignDriver/{rideRequestId}")]
        public async Task<IActionResult> AssignRideDriver(string rideRequestId)
        {
            try
            {
                var rideRequest = await _rideService.AssignRideDriver(rideRequestId);
                if (rideRequest != null)
                {
                    return Ok(rideRequest);
                }
                return NotFound("No available driver or ride request not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning a driver to the ride request.");
                return StatusCode(500, "Internal server error.");
            }
        }


    }
}
