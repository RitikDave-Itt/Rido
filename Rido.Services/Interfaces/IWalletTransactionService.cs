using Rido.Data.Entities;
using Rido.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface IWalletTransactionService:IBaseService<WalletTransaction>
    {
        Task<string> CreditTransaction(WalletTransactionRequestDto transactionDto);
        Task<string> WithdrawTransaction(WalletTransactionRequestDto transactionDto);


    }
}
