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
        private IBaseService<RideTransaction> _transactionBaseService;
        
        public RideTransactionController(IServiceProvider serviceProvider ,IRideTransactionService rideTransactionService ,IBaseService<RideTransaction> transactionBaseService) :base(serviceProvider) 
        {
            _rideTransactionService = rideTransactionService;
            _transactionBaseService = transactionBaseService;
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


                var transaction = await _transactionBaseService.FindAsync(rt => rt.RideRequestId == rideRequestId);

                if (transaction == null)
                {
                    return Ok(RideTransactionStatus.Pending.ToString());
                }
                else
                {
                    return Ok(RideTransactionStatus.Completed.ToString());
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
                var transaction = await _transactionBaseService.FindAsync(rt => rt.RideRequestId == rideRequestId, rt => rt.Rider);


                if (transaction == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new
                    {
                        amount = transaction.Amount,
                        riderName = $"{transaction.Rider.FirstName} {transaction.Rider.LastName}",
                        transactionId = transaction.Id,
                        transactionStatus = transaction.Status.ToString(),
                        createdAt = transaction.Date
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
