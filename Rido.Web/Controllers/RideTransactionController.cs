using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rido.Data.Entities;
using Rido.Model.Enums;
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
        
        public RideTransactionController(IServiceProvider serviceProvider ,IRideTransactionService rideTransactionService) :base(serviceProvider) 
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


        [HttpGet("status/{rideRequestId}")]
        public async Task<IActionResult> CheckTransactionStatus(string rideRequestId)
        {
            try
            {
                var userId = GetCurrentUserId();


                var rideRequest = await _rideTransactionService.FindAsync(rt => rt.Status == RideRequestStatus.Completed);

                if (rideRequest.Status == RideRequestStatus.Unpaid)
                {
                    return Ok(RideTransactionStatus.Pending.ToString());
                }
                else if(rideRequest.Status==RideRequestStatus.Completed) 
                {
                    return Ok(RideTransactionStatus.Completed.ToString());
                }
                else
                {
                    return BadRequest();
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking the transaction status for RideRequestId: {RideRequestId}", rideRequestId);

                return StatusCode(500, new { Message = "An error occurred while checking Ride Transaction Status" });
            }
        }

        [HttpGet("by-ride-request-id/{rideRequestId}")]

        public async Task<IActionResult> GetRideTransactionByRideRequestId(string rideRequestId)
        {
            try
            {
                var rideRequest = await _rideTransactionService.FindAsync(rr=>rr.Id == rideRequestId, rr => rr.Rider,RedirectResult=>RedirectResult.RideTransaction);


                if (rideRequest == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new
                    {
                        amount = rideRequest.Amount,
                        riderName = $"{rideRequest.Rider.FirstName} {rideRequest.Rider.LastName}",
                        transactionId = rideRequest.Id,
                        transactionStatus = rideRequest.RideTransaction.Status.ToString(),
                        createdAt = rideRequest?.RideTransaction.Date
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something Went Wrong in Transaction");
                return StatusCode(500,"Server Error");

            }
            }
        }





    
}
