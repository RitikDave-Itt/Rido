using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Repositories.Interfaces
{
    public interface IRideRequestRepository
    {
        Task<RideAndDriverDetailJoin> GetRideAndDriverDetailsByIdAsync(string rideRequestId);

    }
}
