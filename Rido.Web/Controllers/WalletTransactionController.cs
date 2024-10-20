using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Rido.Common.Exceptions;
using Rido.Common.Models.Requests;
using Rido.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Rido.Web.Controllers
{
    [ApiController]
    [Route("api/wallet-transaction")]
    [Authorize]
    public class WalletTransactionController : BaseController<WalletTransactionController>
    {
        private readonly IWalletTransactionService _walletTransactionService;

        public WalletTransactionController(IServiceProvider serviceProvider, IWalletTransactionService walletTransactionService)
            : base(serviceProvider)
        {
            _walletTransactionService = walletTransactionService;
        }

        [HttpPost("credit")]
        public async Task<ActionResult> CreditTransaction([FromBody] WalletTransactionRequestDto transactionDto)
        {
            if (transactionDto == null || transactionDto.Amount <= 100)
            {
                return BadRequest(new { Message = "not valid transaction" });
            }

            try
            {
                var transactionId = await _walletTransactionService.CreditTransaction(transactionDto);
                return Ok(new { transaction_id = transactionId });

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in Credit Transaction");
                return StatusCode(500, new { Message = "Internal server error. Please try again later." });
            }
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult> WithdrawTransaction([FromBody] WalletTransactionRequestDto transactionDto)
        {
            if (transactionDto == null || transactionDto.Amount <= 0)
            {
                return BadRequest(new { Message = "Invalid transaction details." });
            }

            try
            {
                var transactionId = await _walletTransactionService.WithdrawTransaction(transactionDto);
                return Ok(new {transaction_id = transactionId});
            }
            catch (InsufficientBalanceException ex)
            {
                _logger.LogError(ex,"Error INSUFFICIENT BALANCE");
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in Withdraw Transaction");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }
    }
}
