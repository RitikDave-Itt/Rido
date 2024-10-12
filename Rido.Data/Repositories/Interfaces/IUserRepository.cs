using System.Collections.Generic;
using System.Threading.Tasks;
using Rido.Data.Entities;

namespace Rido.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
       
        Task<User> GetUserByEmail(string email);

    }
}
