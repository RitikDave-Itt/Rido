using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Entities
{
    public class RefreshToken
    {
        public string Id = Guid.NewGuid().ToString();
        public string Token  { get; set; }
        public string UserId { get; set; }
        public DateTime Expiry { get; set; }
        public bool IsRevoked { get; set; }

        public User User { get; set; }
    }

}
