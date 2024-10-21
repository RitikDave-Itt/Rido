﻿using Rido.Common.Models.Requests;
using Rido.Common.Models.Responses;
using Rido.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface IAuthServices
    {
        public Task<LoginResponse> LoginUserAsync(LoginUserDto loginUserDto);
        public Task<string> RegisterUserAsync(RegisterUserDto userDto ,RegisterDriverDto driverDto);

        public Task<OTPVerificationStatus> VerifyOTP(string otp, string rideRequestId);
        Task<(bool IsValid, string? RefreshToken, string? JwtToken)> VerifyAndGenerateRefreshToken(string token);


    }
}
