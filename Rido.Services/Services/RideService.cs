using AutoMapper;
using Microsoft.AspNetCore.Http;
using Rido.Common.Exceptions;
using Rido.Model.Responses;
using Rido.Common.Utils;
using Rido.Model.DataTypes;
using Rido.Data.DTOs;
using Rido.Data.Entities;
using Rido.Model.Enums;
using Rido.Data.Repositories.Interfaces;
using Rido.Services.Interfaces;
using System.Text.RegularExpressions;

namespace Rido.Services
{
    public class RideService : BaseService<RideRequest>, IRideService
    {

        private IBaseRepository<DriverLocation> _driverLocationRepository;
        private IBaseRepository<User> _userRepository;


        public RideService( IBaseRepository<DriverLocation> driverLocationRepository
            ,IServiceProvider serviceProvider
            ,IBaseRepository<User> userRepository
            ) : base(serviceProvider)
        {
            _driverLocationRepository = driverLocationRepository;
            _userRepository = userRepository;
        }


        public async Task<RideRequest> CreateRideRequest(RideRequestDto rideRequestDto)
        {
            string currentUserId = GetCurrentUserId();



            var checkRide = await _repository.AnyAsync(ride =>ride.RiderId == currentUserId && ride.IsActive==true);
            if (checkRide)
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

            rideRequest.Amount = (double)fare[1];

            var user = await _userRepository.FindAsync(u => u.Id == currentUserId, u => u.Wallet);

            
            if (user != null&&user.Wallet.Balance < rideRequest.Amount)
            {
                throw new LowBalanceException();
                  

            }
            




            


            rideRequest.RiderId = currentUserId;
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
            &&item.IsActive == true
            &&item.VehicleType==user.DriverData.VehicleType,item=>item.Rider);



            return _mapper.Map<List<RideRequestResponseDto>>(createdRideRequest);

        }

        public async Task<bool> CancelRideByUser()
        {

            string currentUserId = GetCurrentUserId();

            var rideRequest = await _repository.FindAsync(rr=>rr.RiderId==currentUserId&&rr.IsActive);

            if (rideRequest == null)
            {
                return false;
            }


            rideRequest.Status= RideRequestStatus.Canceled;
            rideRequest.IsActive = false;
            rideRequest.CancelBy = UserRole.User;
            var update = await _repository.UpdateAsync(rideRequest);


            return update;
        }

        public async Task<bool> CancelRideByDriver()
        {

            var userId = GetCurrentUserId();

            var rideRequest = await _repository.FindAsync(rr=>rr.DriverId==userId&&rr.IsActive);

            if (rideRequest == null)
            {
                return false;
            }
           
            rideRequest.Status = RideRequestStatus.Canceled;
            rideRequest.IsActive= false;
            rideRequest.CancelBy= UserRole.Driver;

            bool result = await _repository.UpdateAsync(rideRequest);


            return result;

        }

        public async Task<RideRequest> AssignRideDriver(string rideRequestId)
        {
            string currentUserId = GetCurrentUserId();



            var checkRide = await _repository.AnyAsync(ride =>ride.DriverId== currentUserId&&ride.IsActive==true);
    
            if (checkRide)
            {
                throw new ALreadyRideExistsException();
            }

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
            var result = await _repository.FindAsync(r=>r.Id==rideRequestId, result=>result.Driver.location,r=>r.Driver.DriverData);

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

            var rideRequest = await _repository.GetByIdAsync(rideRequestId);

            if (rideRequest == null || rideRequest.Status != RideRequestStatus.Accepted || rideRequest.DriverId != UserId)
            {
                return OTPVerificationStatus.InvalidRideRequestStatus;
            }

            rideRequest.Status = RideRequestStatus.InProgress;

            var update = await _repository.UpdateAsync(rideRequest);




            return OTPVerificationStatus.Success;
        }

        public async Task<string> GetActiveRideStatusAsync()
        {
            var userId = GetCurrentUserId();
            var role = GetCurrentUserRole();
            if (role == UserRole.User.ToString())
            {
                var activeRide = await _repository.FindAsync(ride => ride.RiderId == userId && ride.IsActive);
                   
                return activeRide?.Status.ToString();

            }
            else if( role == UserRole.Driver.ToString())
            {
                var activeRide = await _repository.FindAsync(ride =>
                    ride.DriverId == userId &&ride.IsActive
                   
                );
                return activeRide?.Status.ToString();


            }
            return null;

        }




    }
}
