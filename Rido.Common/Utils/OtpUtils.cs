using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Utils
{
    public static class OtpUtils
    {
        public static string GenerateOtp(string input)
        {
            StringBuilder digits = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    digits.Append(c);
                }
            }

            string otp =  digits.Length >= 4 ? digits.ToString().Substring(digits.Length - 4) : digits.ToString();
            return otp;
        }

        public static bool MatchOTP(string input, string otp)
        {
            StringBuilder digits = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    digits.Append(c);
                }
            }
            string matchOtp = digits.Length >= 4 ? digits.ToString().Substring(digits.Length - 4) : digits.ToString();

            return matchOtp == otp;

        }

    }
}
