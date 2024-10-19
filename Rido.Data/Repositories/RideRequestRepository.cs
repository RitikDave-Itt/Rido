using Microsoft.EntityFrameworkCore;
using Rido.Data.Contexts;
using Rido.Data.Enums;
using Rido.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Repositories
{
    public class RideRequestRepository :IRideRequestRepository
    {
        private readonly RidoDbContext _context;

        public RideRequestRepository(RidoDbContext context)
        {
            _context = context;
        }

        public async Task<RideAndDriverDetailJoin> GetRideAndDriverDetailsByIdAsync(string rideRequestId)
        {
            var query = from rideRequest in _context.RideRequests
                        join user in _context.Users on rideRequest.DriverId equals user.Id
                        join driverData in _context.DriverData on rideRequest.DriverId equals driverData.UserId
                        where rideRequest.Id == rideRequestId && rideRequest.Status == RideRequestStatus.Accepted

                        select new RideAndDriverDetailJoin
                        {
                            User_Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            Gender = (Gender) user.Gender,
                           ProfileImageUrl = user.ProfileImageUrl,

                            RideRequestId = rideRequest.Id,
                          PickupLatitude = rideRequest.PickupLatitude,
                            PickupLongitude = rideRequest.PickupLongitude,
                            PickupAddress = rideRequest.PickupAddress,
                            PickupTime = rideRequest.PickupTime,
                            DestinationLatitude = rideRequest.DestinationLatitude,
                           DestinationLongitude = rideRequest.DestinationLongitude,
                            DestinationAddress = rideRequest.DestinationAddress,
                            MinPrice = rideRequest.MinPrice,
                            MaxPrice = rideRequest.MaxPrice,
                           DistanceInKm = rideRequest.DistanceInKm,

                           VehicleType = (VehicleType)driverData.VehicleType,
                            VehicleModel = driverData.VehicleModel,
                            VehicleMake = driverData.VehicleMake,
                            
                        };

            return await query.FirstOrDefaultAsync();
        }

    }
}
