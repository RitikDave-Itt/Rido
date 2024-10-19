using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Rido.Services;
using Rido.Services.Interfaces;

namespace Rido.Web.Controllers
{
    [ApiController]
    [Route("api/ride-transaction")]
    [Authorize]
    public class RideTransactionController:BaseController<RideTransactionController>
    {
        private IRideTransactionService _rideTransactionService;
        public RideTransactionController(IServiceProvider serviceProvider ,IRideTransactionService rideTransactionService):base(serviceProvider) 
        {
            _rideTransactionService = rideTransactionService;
        }


        [HttpPost("transfer/{rideRequestId}")]
        public async Task<ActionResult> spendTransaction(string rideRequestId)
        {
            try
            {

                var result = await _rideTransactionService.SpendTransaction(rideRequestId);
                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(500, new { Message = "Something Went Wrong" });
                }

                return Ok(new { Success = true, Message = "Transaction Successsfully Done" ,BookingId = result});



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in Spend Transaction");
                return NotFound(new {Message = "Ride Not Found"});
            }
        }



    }
}
