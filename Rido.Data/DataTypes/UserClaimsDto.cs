using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.DataTypes
{
    public class UserClaimsDto
    {
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Jti { get; set; }
    }
}
