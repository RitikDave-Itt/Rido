using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Model.Requests
{
    public class VerifyOtpDto
    {
        public string RideRequestId { get; set; }
        public string Otp { get; set; }
    }

}
