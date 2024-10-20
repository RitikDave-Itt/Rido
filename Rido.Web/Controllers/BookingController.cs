using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rido.Services.Interfaces;

namespace Rido.Web.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/bookings")]
    public class BookingController:BaseController<BookingController>
    {
        private readonly  IBookingService _bookingService;
        
        public BookingController(IServiceProvider serviceProvider ,IBookingService bookingService) : base(serviceProvider) 
        {
            _bookingService = bookingService;
        }

        [HttpGet("get-bookings")]
        public async Task<ActionResult> GetBookings([FromQuery] int pageNo , int pageSize)
        {
            try
            {
                var userId = GetCurrentUserId();
                var (items, totalCount) = await _bookingService.GetUserBookings(userId, pageNo, pageSize);
                if (items == null || items.Count == 0)
                {
                    return NotFound(new { TotalItems = 0 });

                }
                return Ok(new { Data = items, TotalItems = totalCount });
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error While  Fetching Bookings");
                return BadRequest(ex.Message);
            }

        }
    }
}
