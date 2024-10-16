using Rido.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Repositories
{
    public class RideRequestRepository 
    {
        private readonly RidoDbContext _context;

        public RideRequestRepository(RidoDbContext context)
        {
            _context = context;
        }

        public async Task<RiderResponseDto> GetRideAndDriverDetail(string rideRequestId)
        {
        }
    }
}
