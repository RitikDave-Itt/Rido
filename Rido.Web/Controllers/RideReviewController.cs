using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Operation.Valid;
using Rido.Common.Exceptions;
using Rido.Model.Requests;
using Rido.Services.Interfaces;

namespace Rido.Web.Controllers
{
    [ApiController]
    [Route("api/ride-review")]
    [Authorize]
    public class RideReviewController : BaseController<RideReviewController>
    {
        private readonly IRideReviewService _rideReviewService;

        public RideReviewController(IRideReviewService rideReviewService,  IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _rideReviewService = rideReviewService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview([FromBody] RideReviewRequestDto request)
        {

            try
            {
                var result  = await _rideReviewService.CreateReview(request);

                if (result != null) { 
                 return Ok(new {Success = true , Message = "Review successful", ReviewId=result});
                }

                return StatusCode(500, new { Message = "Review not saved" });

            }
            catch(NotValidUserException ex)
            {
                return Unauthorized("Not A valid user to Review");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Saving Review");
                return BadRequest("something Went wrong");
                
            }

        }
    }
}
