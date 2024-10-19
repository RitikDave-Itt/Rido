using AutoMapper;
using Microsoft.AspNetCore.Http;
using Rido.Common.Models.Types;
using Rido.Common.Utils;
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


        public RideService( IBaseRepository<DriverLocation> driverLocationRepository, IBaseRepository<RideRequest> rideRequestRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _driverLocationRepository = driverLocationRepository;
            _rideRequestRepository = rideRequestRepository;
        }


        public async Task<RideRequest> CreateRideRequest(RideRequestDto rideRequestDto)
        {
            string currentUserId = GetCurrentUserId();

            var rideRequest = _mapper.Map<RideRequest>(rideRequestDto);
            rideRequest.GeohashCode = GeoHasherUtil.Encoder(new LocationType(rideRequestDto.PickupLatitude, rideRequestDto.PickupLongitude));
            RideCalculations rideCalculations = new RideCalculations();
            LocationType destination = new LocationType(rideRequestDto.DestinationLatitude, rideRequestDto.DestinationLongitude);
            LocationType pickup = new LocationType(rideRequestDto.PickupLatitude, rideRequestDto.PickupLongitude);


            rideRequest.DistanceInKm = rideCalculations.CalculateDistance(
               pickup, destination
            );
            decimal[] fare = rideCalculations.CalculateFare(rideRequest.DistanceInKm, rideRequest.VehicleType);

            rideRequest.MinPrice = fare[0];
            rideRequest.MaxPrice = fare[1];



            rideRequest.UserId = currentUserId;
            var SavedRideRequest = await _repository.AddAsync(rideRequest);

            var createdRideRequest = await _repository.GetByIdAsync(SavedRideRequest.Id);

            return createdRideRequest;

        }
        public async Task<List<RideRequest>> GetRideRequestByLocation(LocationType location)
        {

            string geohash = GeoHasherUtil.Encoder(location);



            IEnumerable<RideRequest> createdRideRequest = await _repository.FindAllAsync(item => item.GeohashCode == geohash && item.Status == RideRequestStatus.Requested);

            return createdRideRequest.ToList();

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

            bool result = await _repository.UpdateAsync(rideRequest);


            return result;

        }

        public async Task<RideRequest> AssignRideDriver(string rideRequestId)
        {
            string currentUserId = GetCurrentUserId();

            var rideRequest = await _repository.GetByIdAsync(rideRequestId);

            rideRequest.DriverId = currentUserId;
            rideRequest.Status = RideRequestStatus.Accepted;

            bool updateResult = await _repository.UpdateAsync(rideRequest);

            if (updateResult)
            {
                return rideRequest;

            }
            else
            {
                return null;
            }




        }

        public async Task<dynamic> GetRideAndDriverDetail(string rideRequestId)
        {
            var result = await _rideRequestRepository.FindFirstAsync(r=>r.Id==rideRequestId,r=>r.Driver, result=>result.Driver.location,r=>r.Driver.DriverData);

            var response = new
            {
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



    }
}
