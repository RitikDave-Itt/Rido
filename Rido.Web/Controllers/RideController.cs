using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rido.Common.Exceptions;
using Rido.Common.Models.Requests;
using Rido.Common.Utils;
using Rido.Data.DataTypes;
using Rido.Data.DTOs;
using Rido.Data.Entities;
using Rido.Data.Enums;
using Rido.Services.Interfaces;

namespace Rido.Web.Controllers
{
    [ApiController]
    [Route("api/ride")]
    [Authorize]

    public class RideController : BaseController<RideController>
    {
        private readonly IRideService _rideService;
        private readonly IBaseService<RideRequest> _rideRequestService;


        public RideController(IRideService rideService, IServiceProvider serviceProvider, IBaseService<RideRequest> rideRequestService) : base(serviceProvider)
        {
            _rideService = rideService;
            _rideRequestService = rideRequestService;
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
                    return BadRequest("Invalid pickup or destination coordinates format.");
                }

                LocationType pickupLocation = new LocationType { Latitude = pickupCoordinates[0], Longitude = pickupCoordinates[1] };
                LocationType destinationLocation = new LocationType { Latitude = destinationCoordinates[0], Longitude = destinationCoordinates[1] };

                RideCalculations rideCalculations = new RideCalculations();

                var fareList = rideCalculations.FareList(pickupLocation, destinationLocation);
                return Ok(fareList);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = "Failed to retrieve the fare list.", Error = ex.Message });
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

                return Ok(new { Message = "Ride Requested Successfully", Data = result.Id });
            }
            catch (ALreadyRideExistsException ex)
            {
                return StatusCode(409, new { Message = "Cannot Create Multiple Ride Request" });


            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while  your request." });
            }
        }
        [Authorize(Roles = "Driver")]
        [HttpGet("get-ride-list")]
        public async Task<IActionResult> GetRideRequestByLocation([FromQuery] string latitude, string longitude)
        {
            LocationType location = new LocationType(latitude, longitude);
            try
            {
                var rideRequests = await _rideService.GetRideRequestList(location);
                return Ok(rideRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " error occurred while  ride requests by location.");
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpDelete("cancel-by-user/{rideRequestId}")]
        public async Task<IActionResult> CancelRideByUser(string rideRequestId)
        {
            try
            {
                var result = await _rideService.CancelRideByUser(rideRequestId);
                if (result)
                {
                    return Ok(new { Success = true });
                }
                return NotFound("Ride request not found .");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while canceling the Ride by user.");
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpPut("driver-cancel-ride/{rideRequestId}")]
        public async Task<IActionResult> CancelRideByDriver(string rideRequestId)
        {
            try
            {
                var result = await _rideService.CancelRideByDriver(rideRequestId);
                if (result)
                {
                    return Ok(new { success = true, Message = "Ride Cancelled Successfully" });
                }
                return NotFound("Ride request Not found or could not be canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error  while canceling the ride by driver.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("accept-ride/{rideRequestId}")]
        public async Task<IActionResult> AssignRideDriver(string rideRequestId)
        {
            try
            {
                var rideRequest = await _rideService.AssignRideDriver(rideRequestId);
                if (rideRequest != null)
                {
                    return Ok(new { success = true, Message = "Ride Accepted Successfully" });
                }
                return NotFound("No available driver or ride request not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while assigning a driver to the ride request.");
                return StatusCode(500, "Internal server error.");
            }
        }

        //[HttpGet("ride-confirm-detail/{rideRequestId}")]
        //public async Task<IActionResult> GetRideAndDriverDetail(string rideRequestId)
        //{
        //    try
        //    {
        //        var rideAndDriverDetail = await _rideService.GetRideAndDriverDetail(rideRequestId);
        //        if (rideAndDriverDetail != null)
        //        {
        //            return Ok(rideAndDriverDetail);
        //        }
        //        else
        //        {
        //            return NotFound(new { Message = "Ride Not Found Invalid Id" });
        //        }

        //    }
        //    catch (DriverNotAssignedException ex)
        //    {
        //        return StatusCode(460, new { Message = "Driver not Assigned" });


        //    }

        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, " error  while retrieving ride and driver details.");
        //        return StatusCode(500, "Internal server error.");
        //    }
        //}

        [HttpPost("verify-otp")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto)
        {
            try
            {
                var verify = await _rideService.VerifyOTP(verifyOtpDto.Otp, verifyOtpDto.RideRequestId);

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
            catch (Exception ex)
            {

                _logger.LogError(ex, "Something Went Wrong in Otp Verification");
                return BadRequest();
            }
        }

        [HttpGet("driver")]

        public async Task<IActionResult> GetRideByDriverId()
        {
            var userId = GetCurrentUserId();


            var rideRequest = await _rideRequestService.FindAsync(ride => ride.DriverId == userId, ride => ride.Rider);

            if (rideRequest == null)
            {
                return NotFound(new { success = false });

            }

            else
            {
                return Ok(
                    new
                    {
                        id = rideRequest.Id,
                        pickupLatitude = rideRequest.PickupLatitude,
                        pickupLongitude = rideRequest.PickupLongitude,

                        pickupAddress = rideRequest.PickupAddress,
                        pickupTime = rideRequest.PickupTime,
                        destinationLatitude = rideRequest.DestinationLatitude,
                        destinationLongitude = rideRequest.DestinationLongitude,
                        destinationAddress = rideRequest.DestinationAddress,
                        price = rideRequest.MaxPrice,
                        vehicleType = rideRequest.VehicleType.ToString(),

                        geohashCode = rideRequest.GeohashCode,
                        distanceInKm = rideRequest.DistanceInKm,
                        userName = $"{rideRequest.Rider.FirstName} {rideRequest.Rider.LastName}",

                        status = rideRequest.Status,
                        gender = rideRequest.Rider.Gender.ToString(),
                        phoneNumber = rideRequest.Rider.PhoneNumber
                    }
                 );
            }


        }

        [HttpGet("ride-confirm-details")]

        public async Task<IActionResult> GetRideByRiderId()
        {
            var userId = GetCurrentUserId();


            var rideRequest = await _rideRequestService.FindAsync(ride => ride.UserId == userId, ride => ride.Driver, ride => ride.Driver.location);

            if (rideRequest == null)
            {
                return NotFound(new { success = false });

            }
           

            else
            {

                return Ok(
                    new
                    {
                        rideRequest = new
                        {
                            id = rideRequest.Id,
                            pickupLatitude = rideRequest.PickupLatitude,
                            pickupLongitude = rideRequest.PickupLongitude,

                            pickupAddress = rideRequest.PickupAddress,
                            pickupTime = rideRequest.PickupTime,
                            destinationLatitude = rideRequest.DestinationLatitude,
                            destinationLongitude = rideRequest.DestinationLongitude,
                            destinationAddress = rideRequest.DestinationAddress,
                            price = rideRequest.MaxPrice,
                            vehicleType = rideRequest.VehicleType.ToString(),

                            geohashCode = rideRequest.GeohashCode,
                            distanceInKm = rideRequest.DistanceInKm,

                            status = rideRequest.Status.ToString(),

                        }
                    ,
                        driver = rideRequest?.Status==RideRequestStatus.Accepted ? new
                        {
                            id = rideRequest?.Driver?.Id,
                            driverName = $"{rideRequest?.Driver.FirstName} {rideRequest?.Driver.LastName}",

                            mobileNo = rideRequest.Driver.PhoneNumber,
                            vehicleType = rideRequest.VehicleType,
                            latitude = rideRequest.Driver?.location?.Latitude,
                            longitude = rideRequest.Driver?.location?.Longitude,
                            otp = OtpUtils.GenerateOtp(rideRequest.Id)

                        } : null
                    }
                 );
            }


        }

        [HttpGet("check-status")]
        public async Task<IActionResult> CheckStatus()
        {
            try
            {
                var userID = GetCurrentUserId();
                var rideRequest = await _rideRequestService.FindAsync(ride=>ride.UserId == userID);
                if (rideRequest != null)
                {
                    return Ok(rideRequest.Status.ToString());
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while assigning a driver to the ride request.");
                return StatusCode(500, "Internal server error.");
            }
        }



    }
}
