using Rido.Data.Contexts;
using Rido.Data.Entities;
using Rido.Data.Repositories.Interfaces;
using Rido.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services
{
    public class RideTransactionService : BaseService<RideTransaction> ,IRideTransactionService
    {

        private IRideTransactionRepository _rideTransactionRepository;
        public RideTransactionService(IServiceProvider serviceProvider ,IRideTransactionRepository rideTransactionRepository ) : base(serviceProvider) { 
            _rideTransactionRepository = rideTransactionRepository;
        }

        public async  Task<string> SpendTransaction(string rideRequestId)
        {
            
            return  await _rideTransactionRepository.RideSpendTransaction(rideRequestId);


        }

    }
}
