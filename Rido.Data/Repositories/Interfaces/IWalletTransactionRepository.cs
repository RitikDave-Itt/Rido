using Rido.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Repositories.Interfaces
{
    public interface IWalletTransactionRepository
    {
         Task<string> CreditWallet(WalletTransaction walletTransaction);
        Task<string> WithdrawWallet(WalletTransaction walletTransaction, Wallet wallet);


    }
}
