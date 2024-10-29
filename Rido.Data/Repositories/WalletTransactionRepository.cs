using Microsoft.EntityFrameworkCore;
using Rido.Data.Contexts;
using Rido.Data.Entities;
using Rido.Model.Enums;
using Rido.Data.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Rido.Data.Repositories
{
    public class WalletTransactionRepository: IWalletTransactionRepository
    {
        private readonly RidoDbContext _dbContext;

        public WalletTransactionRepository(RidoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreditWallet(WalletTransaction walletTransaction)
        {
            var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == walletTransaction.UserId);

            if (wallet == null)
            {
                throw new Exception("Wallet not found!");
            }

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.WalletTransactions.AddAsync(walletTransaction);

                    wallet.Balance += walletTransaction.Amount;

                    wallet.UpdatedAt = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return walletTransaction.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<string> WithdrawWallet(WalletTransaction walletTransaction,Wallet wallet)
        {

            if (wallet == null)
            {
                throw new Exception("Wallet not found!");
            }


            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.WalletTransactions.AddAsync(walletTransaction);

                    wallet.Balance -= walletTransaction.Amount;    

                    wallet.UpdatedAt = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return walletTransaction.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
