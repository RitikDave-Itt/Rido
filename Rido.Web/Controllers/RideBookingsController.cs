using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rido.Model.Enums;
using Rido.Model.Responses;
using Rido.Services.Interfaces;

namespace Rido.Web.Controllers
{
    
        [ApiController]
        [Route("api/bookings")]
        [Authorize]    
        public class RideBookingsController : BaseController<RideBookingsController>
        {
            private readonly IRideService _rideBookingService;

            public RideBookingsController(IServiceProvider serviceProvider, IRideService rideBookingService) : base(serviceProvider)
            {
                _rideBookingService = rideBookingService;
            }

            [HttpGet("get-bookings")]
            public async Task<ActionResult> GetBookings([FromQuery] int pageNo, int pageSize)
            {
            try
            {
                var userId = GetCurrentUserId();
                var role = GetCurrentUserRole();

                if (role == UserRole.User.ToString())
                {

                    var (items, totalCount) = await _rideBookingService.FindPageAsync(b =>
                    b.RiderId == userId &&b.IsActive==false,
                    pageSize,
                    pageNo,
                    [query => query.OrderByDescending(item => item.CreatedAt)]);
                    var bookings = _mapper.Map<List<BookingsResponseDto>>(items);

                    if (bookings == null || bookings.Count == 0)
                    {
                        return NotFound(new { TotalItems = 0 });

                    }
                    return Ok(new { Data = bookings, TotalItems = totalCount });
                }
                else if (role == UserRole.Driver.ToString()) {
                    var (items, totalCount) = await _rideBookingService.FindPageAsync(b =>
                    b.DriverId == userId && b.IsActive == false,
                    pageSize,
                    pageNo,
                    [query => query.OrderByDescending(item => item.CreatedAt)]);


                    var bookings = _mapper.Map<List<BookingsResponseDto>>(items);

                    if (bookings == null || bookings.Count == 0)
                    {
                        return NotFound(new { TotalItems = 0 });

                    }
                    return Ok(new { Data = bookings, TotalItems = totalCount });

                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error While  Fetching Bookings");
                return BadRequest(ex.Message);
            }

            }

        [HttpGet("get-by-id/{rideRequestId}")]

        public async Task<IActionResult> GetByIdAsync(string rideRequestId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var userRole = GetCurrentUserRole();

                var rideRequest = await _rideBookingService.FindAsync(rr => rr.Id == rideRequestId, rr => rr.Driver, rr => rr.Rider);

                if(rideRequest == null)
                {
                    return NotFound();
                }

                var rideRequestData = new
                {
                    id = rideRequest.Id,
                    riderId = rideRequest.RiderId,
                    driverId = rideRequest.DriverId,
                    pickupAddress = rideRequest.PickupAddress,
                    pickupLatitude = rideRequest.PickupLatitude,
                    pickupLongitude = rideRequest.PickupLongitude,
                    pickupTime = rideRequest.PickupTime,
                    dropoffAddress = rideRequest.DestinationAddress,
                    destinationLatitude = rideRequest.DestinationLatitude,
                    destinationLongitude = rideRequest.DestinationLongitude,
                    dropoffTime = rideRequest.DropoffTime,
                    status = rideRequest.Status.ToString(),
                    vehicleType = rideRequest.VehicleType.ToString(),
                    distanceInKm = rideRequest.DistanceInKm,
                    amount = rideRequest.Amount,
                    transactionId = rideRequest.TransactionId,
                    createdAt = rideRequest.CreatedAt,
                    updatedAt = rideRequest.UpdatedAt,
                    cancelBy = rideRequest.CancelBy.ToString(),
                    cancelReason = rideRequest.CancelReason,

                };


                if (userRole == UserRole.User.ToString())
                {
                    return Ok(new
                    {
                        bookingData = rideRequestData,
                        driver = rideRequest.Driver != null ? new
                        {
                            name = $"{rideRequest.Driver.FirstName} {rideRequest.Driver.LastName}"
                        } : null,

                        
                    });

                }
                else if(userRole == UserRole.Driver.ToString())
                {
                    return Ok(new
                    {
                        bookingData = rideRequestData,
                        rider = rideRequest.Rider != null ? new
                        {
                            name = $"{rideRequest.Rider.FirstName} {rideRequest.Rider.LastName}"
                        } : null,


                    });

                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Exception In Fetching Data of Ride Request");
                return BadRequest();
            }
        }


        }

    }


