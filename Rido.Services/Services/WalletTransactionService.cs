using Rido.Common.Exceptions;
using Rido.Common.Models.Requests;
using Rido.Data.Entities;
using Rido.Data.Enums;

using Rido.Data.Repositories.Interfaces;
using Rido.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Services
{
    public class WalletTransactionService:BaseService<WalletTransaction>, IWalletTransactionService
    {
        private readonly IWalletTransactionRepository _walletTransactionRepository;
        private readonly IBaseRepository<Wallet> _walletRepository;

        public WalletTransactionService(IServiceProvider serviceProvider, IWalletTransactionRepository wallet,IBaseRepository<Wallet> walletRepository) :base(serviceProvider) { 
         _walletTransactionRepository = wallet;
            _walletRepository = walletRepository;
        }


        public async Task<string> CreditTransaction(WalletTransactionRequestDto transactionDto)
        {
            WalletTransaction transaction = new WalletTransaction()
            {
                Amount = transactionDto.Amount,
                Type = WalletTransactionType.Credit,
                UserId = GetCurrentUserId(),
                UpdatedAt = DateTime.Now



            };

            var result = await _walletTransactionRepository.CreditWallet(transaction);

            return result;
       

           


        }
        public async Task<string> WithdrawTransaction(WalletTransactionRequestDto transactionDto)
        {
            var userId = GetCurrentUserId();

            var wallet = await _walletRepository.FindFirstAsync(w=>w.UserId == userId);

            if (wallet.Balance < transactionDto.Amount)
            {
                throw new InsufficientBalanceException("Insufficient Balance");
                
            }





            WalletTransaction transaction = new WalletTransaction()
            {
                Amount = transactionDto.Amount,
                Type =WalletTransactionType.Withdraw,
                UserId = GetCurrentUserId(),
                CreatedAt = DateTime.Now



            };

            var result = await _walletTransactionRepository.WithdrawWallet(transaction,wallet);

            return result;





        }

    }
}
