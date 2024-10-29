using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rido.Data.Entities;
using Rido.Data.Repositories.Interfaces;
using Rido.Services.Interfaces;

namespace Rido.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/wallet")]
    public class WalletController :BaseController<Wallet>
    {
        private IBaseService<Wallet> _walletService;

        public WalletController(IBaseService<Wallet> walletService,IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _walletService = walletService;
        }


        [HttpGet]

        public async Task<IActionResult> GetWalletByUserId()
        {
            try
            {
                var userId = GetCurrentUserId();
                var wallet = await _walletService.FindAsync(w => w.UserId == userId);
                if (wallet == null)
                {
                    return NotFound("Wallet not Found");
                }
                return Ok(new
                {
                    balance = wallet.Balance,
                    status = wallet.WalletStatus.ToString(),
                });



            }
            catch (Exception ex) {
                _logger.LogError(ex, "Wallet Fetch Error");
                return BadRequest();
            
            }
        }




        

    }
}
