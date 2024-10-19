using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Repositories.Interfaces
{
    public interface IRideTransactionRepository
    {
        public Task<string> RideSpendTransaction(string rideRequestId);

    }
}
