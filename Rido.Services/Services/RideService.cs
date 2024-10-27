using AutoMapper;
using Microsoft.AspNetCore.Http;
using Rido.Common.Exceptions;
using Rido.Common.Models.Responses;
using Rido.Common.Utils;
using Rido.Data.DataTypes;
using Rido.Data.DTOs;
using Rido.Data.Entities;
using Rido.Data.Enums;
using Rido.Data.Repositories.Interfaces;
using Rido.Services.Interfaces;
using System.Text.RegularExpressions;

namespace Rido.Services
{
    public class RideService : BaseService<RideRequest>, IRideService
    {

        private IBaseRepository<DriverLocation> _driverLocationRepository;
        private IBaseRepository<RideRequest> _rideRequestRepository;
        private IBaseRepository<User> _userRepository;


        public RideService( IBaseRepository<DriverLocation> driverLocationRepository
            ,IBaseRepository<RideRequest> rideRequestRepository
            ,IServiceProvider serviceProvider
            ,IBaseRepository<User> userRepository
            ) : base(serviceProvider)
        {
            _driverLocationRepository = driverLocationRepository;
            _rideRequestRepository = rideRequestRepository;
            _userRepository = userRepository;
        }


        public async Task<RideRequest> CreateRideRequest(RideRequestDto rideRequestDto)
        {
            string currentUserId = GetCurrentUserId();

            var checkRide = _repository.FindAsync(ride => ride.UserId == currentUserId);
            if (checkRide.Result != null)
            {
                throw new ALreadyRideExistsException();
            }

            var rideRequest = _mapper.Map<RideRequest>(rideRequestDto);
            if (Enum.TryParse<VehicleType>(rideRequestDto.VehicleType, true, out var vehicleType))
            {
                rideRequest.VehicleType = vehicleType;
            }
            else
            {
                throw new ArgumentException($"Invalid VehicleType: {rideRequestDto.VehicleType}");
            }
            rideRequest.GeohashCode = GeoHasherUtil.Encoder(new LocationType(rideRequestDto.PickupLatitude, rideRequestDto.PickupLongitude));
            RideCalculations rideCalculations = new RideCalculations();
            LocationType destination = new LocationType(rideRequestDto.DestinationLatitude, rideRequestDto.DestinationLongitude);
            LocationType pickup = new LocationType(rideRequestDto.PickupLatitude, rideRequestDto.PickupLongitude);


            rideRequest.DistanceInKm = rideCalculations.CalculateDistance(
               pickup, destination
            );
            double[] fare = rideCalculations.CalculateFare(rideRequest.DistanceInKm, rideRequest.VehicleType);

            rideRequest.MinPrice = (decimal)fare[0];
            rideRequest.MaxPrice = (decimal)fare[1];



            rideRequest.UserId = currentUserId;
            var SavedRideRequest = await _repository.AddAsync(rideRequest);

            var createdRideRequest = await _repository.GetByIdAsync(SavedRideRequest.Id);

            return createdRideRequest;

        }
        public async Task<List<RideRequestResponseDto>> GetRideRequestList(LocationType location)
        {

            string geohash = GeoHasherUtil.Encoder(location);

            var userId = GetCurrentUserId();

            var user = await _userRepository.FindAsync(user => user.Id==userId, user=>user.DriverData);



            IEnumerable<RideRequest> createdRideRequest = await _repository.FindAllAsync(item => 
            item.GeohashCode == geohash
            && item.Status == RideRequestStatus.Requested
            &&item.VehicleType==user.DriverData.VehicleType,item=>item.Rider);



            return _mapper.Map<List<RideRequestResponseDto>>(createdRideRequest);

        }

        public async Task<bool> CancelRideByUser(string RideRequestId)
        {

            string currentUserId = GetCurrentUserId();


            bool result = await _repository.DeleteAsync(RideRequestId);

            return result;
        }

        public async Task<bool> CancelRideByDriver(string rideRequestId)
        {


            var rideRequest = await _repository.GetByIdAsync(rideRequestId);
            rideRequest.Status = RideRequestStatus.Requested;
            rideRequest.DriverId = null;

            bool result = await _repository.UpdateAsync(rideRequest);


            return result;

        }

        public async Task<RideRequest> AssignRideDriver(string rideRequestId)
        {
            string currentUserId = GetCurrentUserId();

            var rideRequest = await _repository.GetByIdAsync(rideRequestId);

            if(rideRequest.DriverId != null || rideRequest.Status != RideRequestStatus.Requested)
            {
                return null;
            }

            rideRequest.DriverId = currentUserId;
            rideRequest.Status = RideRequestStatus.Accepted;

            bool updateResult = await _repository.UpdateAsync(rideRequest);
            return rideRequest;

           

        }

        public async Task<dynamic> GetRideAndDriverDetail(string rideRequestId)
        {
            var result = await _rideRequestRepository.FindAsync(r=>r.Id==rideRequestId, result=>result.Driver.location,r=>r.Driver.DriverData);

            if (result == null)
            {
                return null;
            }

            if (result.DriverId == null&&result.Status == RideRequestStatus.Requested)
            {
                throw new DriverNotAssignedException();
            }

            var response = new
            {
                DriverAssigned = true,
                Id = rideRequestId,
                DriverName = result.Driver.FirstName + " " + result.Driver.LastName,
                MobileNo = result.Driver.PhoneNumber,
                VehicleType = result.VehicleType.ToString(),
                Latitude = result?.Driver?.location?.Latitude?.ToString(),
                Longitude = result?.Driver?.location?.Longitude?.ToString(),
                Otp = OtpUtils.GenerateOtp(rideRequestId)



            };

            return response;
        }
        public async Task<OTPVerificationStatus> VerifyOTP(string otp, string rideRequestId)
        {
            var UserId = GetCurrentUserId();
            bool otpVerify = OtpUtils.MatchOTP(rideRequestId, otp);


            if (!otpVerify)
            {


                return OTPVerificationStatus.InvalidOTP;

            }

            var rideRequest = await _rideRequestRepository.GetByIdAsync(rideRequestId);

            if (rideRequest == null || rideRequest.Status != RideRequestStatus.Accepted || rideRequest.DriverId != UserId)
            {
                return OTPVerificationStatus.InvalidRideRequestStatus;
            }

            rideRequest.Status = RideRequestStatus.Started;

            var update = await _rideRequestRepository.UpdateAsync(rideRequest);




            return OTPVerificationStatus.Success;
        }



    }
}
