using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface IUserService
    {
        Task<dynamic> GetUser(string id = null);

    }
}
