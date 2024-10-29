using Rido.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface IRideTransactionService:IBaseService<RideRequest>
    { 
        Task<string> SpendTransaction(string rideRequestId);

    }
}
