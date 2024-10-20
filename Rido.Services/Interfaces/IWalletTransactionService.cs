using Rido.Common.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface IWalletTransactionService
    {
        Task<string> CreditTransaction(WalletTransactionRequestDto transactionDto);
        Task<string> WithdrawTransaction(WalletTransactionRequestDto transactionDto);


    }
}
