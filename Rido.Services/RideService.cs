using AutoMapper;
using Geohash;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;
using Rido.Data.DTOs;
using Rido.Data.Entities;
using Rido.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Geohash;
using Rido.Common.Models.Types;
using Rido.Common.Utils;
using Rido.Data.Enums;
using Rido.Services.Interfaces;

namespace Rido.Services
{
    public class RideService :BaseService<RideRequest>,IRideService
    {

        private IBaseRepository<DriverLocation> _driverLocationRepository;
        private IMapper _mapper;


        public RideService(IBaseRepository<DriverLocation> driverLocationRepository, IBaseRepository<RideRequest> rideRequestRepository,IMapper mapper,IHttpContextAccessor httpContextAccessor): base(rideRequestRepository,httpContextAccessor)
        {
            _driverLocationRepository = driverLocationRepository;
            _mapper = mapper;

        }


        public async Task<RideRequest> CreateRideRequest(RideRequestDto rideRequestDto)
        {
            string currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var rideRequest = _mapper.Map<RideRequest>(rideRequestDto);
            rideRequest.UserId = currentUserId;
            var createdRideRequestId = await _repository.AddAsync(rideRequest);

            var createdRideRequest = await _repository.GetByIdAsync(createdRideRequestId);
            
            return createdRideRequest;

        }
        public async Task<List<RideRequest>> GetRideRequestByLocation(LocationType location)         {

            string geohash = GeoHasherUtil.Encoder(location);
            


            IEnumerable<RideRequest> createdRideRequest = await _repository.FindAllAsync(item=>item.GeohashCode==geohash&&item.Status==RideRequestStatus.Requested);

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

        public async Task<RideRequest> GetRideAndDriverDetail(string rideRequestId)
        {





        }


    }
}
