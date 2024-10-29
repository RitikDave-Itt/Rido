using Rido.Data.Contexts;
using Rido.Data.Entities;
using Rido.Model.Enums;
using Rido.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private RidoDbContext _dbContext;

        public UserRepository(RidoDbContext dbContext)
        {

        _dbContext = dbContext;
        }

        public async Task<string> CreateUser(User user , Wallet wallet, DriverData driver = null)
        {
            await using(var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var savedUser = await _dbContext.Users.AddAsync(user);       
                    if (user.Role == UserRole.Driver)
                    {
                        driver.UserId = savedUser.Entity.Id;
                        var savedDriver = await _dbContext.DriverData.AddAsync(driver);

                    }
                    wallet.UserId = savedUser.Entity.Id;       
                    var saveWallet = await _dbContext.Wallets.AddAsync(wallet);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return savedUser.Entity.Id;

                }
                catch (Exception ex) {
                    await transaction.RollbackAsync();
                    throw;
                }

            }



        }
    
    
    }

 }

