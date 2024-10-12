using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rido.Common.Utils
{
    using System;
    using System.Security.Cryptography;

    public static class PasswordHasher
    {
        private static readonly string _key = "ritikdave";

       

        
        public static string HashPassword(string password)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_key)))
            {
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {
            var hashedInputPassword = HashPassword(password);     
            return hashedInputPassword == hashedPassword;    
        }
    }

}
