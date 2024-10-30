using Rido.Model.Responses;
using Rido.Model.DataTypes;
using Rido.Data.DTOs;
using Rido.Data.Entities;
using Rido.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface  IRideService:IBaseService<RideRequest>
    {
        Task<RideRequest> CreateRideRequest(RideRequestDto rideRequestDto);

        Task<List<RideRequestResponseDto>> GetRideRequestList(LocationType location);

        Task<bool> CancelRideByUser();

        Task<bool> CancelRideByDriver();

        Task<RideRequest> AssignRideDriver(string rideRequestId);
        Task<dynamic> GetRideAndDriverDetail(string rideRequestId);
        Task<OTPVerificationStatus> VerifyOTP(string otp, string rideRequestId);
        Task<string> GetActiveRideStatusAsync();



    }
}
