using Rido.Data.Contexts;
using Rido.Data.Entities;
using Rido.Data.Enums;
using Rido.Data.Repositories.Interfaces;
using Rido.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services
{
    public class RideTransactionService : BaseService<RideRequest> ,IRideTransactionService
    {

        private IBaseRepository<RideBooking> _rideBookingRepository;
        public RideTransactionService(IServiceProvider serviceProvider ,IBaseRepository<RideBooking> rideBookingRepository ) : base(serviceProvider) { 
            _rideBookingRepository = rideBookingRepository;
        }

        public async  Task<string> SpendTransaction(string rideRequestId)
        {
            var rideRequest = await _repository.FindAsync(ride => 
            ride.Id == rideRequestId
            ,ride=>ride.Rider
            ,ride=>ride.Driver
            ,ride=>ride.Rider.Wallet
            ,ride=>ride.Driver.Wallet
            
            );
            if (rideRequest == null) {
                return null;
            }

            rideRequest.Rider.Wallet.Balance -= rideRequest.MaxPrice;

            rideRequest.Driver.Wallet.Balance += rideRequest.MaxPrice;


            var transaction = new RideTransaction()
            {
                UserId = rideRequest.UserId,
                DriverId = rideRequest.DriverId,
                Status = RideTransactionStatus.Completed,
                Amount = rideRequest.MaxPrice,
                Remarks ="",
                Rider = rideRequest.Rider,
                Driver = rideRequest.Driver,

            };

            var booking = new RideBooking()
            {
                RideTransaction = transaction,
                UserId = rideRequest.UserId,
                DriverId = rideRequest?.DriverId,
                TransactionId = transaction.Id,
                PickupTime = rideRequest.PickupTime,
                DropoffTime = DateTime.UtcNow,
                PickupLatitude = rideRequest.PickupLatitude,
                PickupLongitude = rideRequest.PickupLongitude,
                PickupAddress = rideRequest.PickupAddress,
                DestinationLatitude = rideRequest.DestinationLatitude,
                DestinationLongitude = rideRequest.DestinationLongitude,
                DestinationAddress = rideRequest.DestinationAddress,
                VehicleType = rideRequest.VehicleType,
                DistanceInKm = rideRequest.DistanceInKm,
                GeohashCode = rideRequest.GeohashCode,
                Amount = rideRequest.MaxPrice,



            };
            var deleteRideRequest = await _repository.DeleteAsync(rideRequestId);

            var result = await _rideBookingRepository.AddAsync(booking);


            
            
            return  result.Id;

        }

    }
}
