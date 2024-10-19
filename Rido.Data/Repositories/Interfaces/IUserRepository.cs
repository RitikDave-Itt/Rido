using System.Collections.Generic;
using System.Threading.Tasks;
using Rido.Data.Entities;

namespace Rido.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {

        public Task<string> CreateUser(User user, Wallet wallet, DriverData driver = null);

    }
}
