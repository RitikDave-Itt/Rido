using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Utils
{
    public static class OtpGenerator
    {
        public static string GenerateOtp()
        {
            Random rand = new Random();
            int otp = rand.Next(1000, 10000);
            return otp.ToString();
        }
    }
}
