using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Enums
{
    public enum OTPVerificationStatus
    {
        Success,
        InvalidOTP,
        RideRequestNotFound,
        InvalidRideRequestStatus,
        UnauthorizedDriver  
    }

}
