using Rido.Data.Contexts;
using Rido.Data.Entities;
using Rido.Model.Enums;
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

        public RideTransactionService(IServiceProvider serviceProvider  ) : base(serviceProvider) { 
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
            if (rideRequest == null||rideRequest.Rider.Wallet.Balance < rideRequest.Amount) {
                return null;
            }

            rideRequest.Rider.Wallet.Balance -= rideRequest.Amount;

            rideRequest.Driver.Wallet.Balance += rideRequest.Amount;


            var transaction = new RideTransaction()
            {
                UserId = rideRequest.RiderId,
                DriverId = rideRequest.DriverId,
                Status = RideTransactionStatus.Completed,
                Amount = rideRequest.Amount,
                Remarks ="",
                Rider = rideRequest.Rider,
                Driver = rideRequest.Driver,

            };
            rideRequest.Status = RideRequestStatus.Completed;
            rideRequest.RideTransaction = transaction;
            rideRequest.IsActive = false;

         

            var result = await _repository.UpdateAsync(rideRequest);


            
            
            return  rideRequest.Id;

        }



    }
}
