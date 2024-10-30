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
        }
    }


