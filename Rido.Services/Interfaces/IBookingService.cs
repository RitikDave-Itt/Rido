using Rido.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface IBookingService
    {
        Task<(List<RideBooking> Items, int TotalCount)> GetUserBookings(string UserId, int pageNo, int pageSize);

    }
}
