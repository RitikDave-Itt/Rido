using Rido.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(string userId, string email , UserRole role);

    }
}
