using Rido.Common.Models.Responses;
using Rido.Data.DataTypes;
using Rido.Data.DTOs;
using Rido.Data.Entities;
using Rido.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface  IRideService
    {
        Task<RideRequest> CreateRideRequest(RideRequestDto rideRequestDto);

        Task<List<RideRequestResponseDto>> GetRideRequestList(LocationType location);

        Task<bool> CancelRideByUser(string rideRequestId);

        Task<bool> CancelRideByDriver(string rideRequestId);

        Task<RideRequest> AssignRideDriver(string rideRequestId);
        Task<dynamic> GetRideAndDriverDetail(string rideRequestId);
        Task<OTPVerificationStatus> VerifyOTP(string otp, string rideRequestId);


    }
}
