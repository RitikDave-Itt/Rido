using Microsoft.EntityFrameworkCore;
using Rido.Data.Contexts;
using Rido.Data.Entities;
using Rido.Model.Enums;
using Rido.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Repositories
{
    public class RideTransactionRepository :IRideTransactionRepository
    {
        private readonly RidoDbContext _dbContext;

        public RideTransactionRepository(RidoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> RideSpendTransaction(string rideRequestId)
        {
            var rideRequest = await _dbContext.RideRequests.FindAsync(rideRequestId);
            if (rideRequest == null)
            {
                throw new ArgumentException("Requested Ride not Found.");
            }

            var riderWallet = await _dbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == rideRequest.UserId);
            var driverWallet = await _dbContext.Wallets.FirstOrDefaultAsync(w=>w.UserId == rideRequest.DriverId);
            
            if (riderWallet == null)
            {
                throw new ArgumentException("Wallet not Found!");
            }

            if (riderWallet.Balance < rideRequest.MaxPrice)
            {
                throw new InvalidOperationException("Low Wallet Balance!");
            }

            var rideTransaction = new RideTransaction
            {
                UserId = rideRequest.UserId,
                DriverId = rideRequest.DriverId,
                Amount = rideRequest.MaxPrice,
                Status = RideTransactionStatus.Completed,
                Date = DateTime.UtcNow
            };

            var rideBooking = new RideBooking
            {
                UserId = rideRequest.UserId,
                DriverId = rideRequest.DriverId,
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
                TransactionId = rideTransaction.Id,
                Amount = rideTransaction.Amount,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.RideTransactions.AddAsync(rideTransaction);
                    await _dbContext.RideBookings.AddAsync(rideBooking);

                    riderWallet.Balance -= rideRequest.MaxPrice;
                    driverWallet.Balance += rideRequest.MaxPrice;
                    _dbContext.RideRequests.Remove(rideRequest);

                    riderWallet.UpdatedAt = DateTime.UtcNow;
                    driverWallet.UpdatedAt = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();
                  
                    await transaction.CommitAsync();

                    return rideBooking.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

    }
}
