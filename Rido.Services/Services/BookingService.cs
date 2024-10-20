using Rido.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rido.Services.Interfaces;
namespace Rido.Services.Services
{


    public class BookingService:BaseService<RideBooking>,IBookingService
    {
        public BookingService(IServiceProvider serviceProvider) : base(serviceProvider){  
        }



        public async Task<(List<RideBooking> Items, int TotalCount)> GetUserBookings(string UserId, int pageNo, int pageSize)
        {
            var (items, totalCount) = await _repository.FindPageAsync(b => b.UserId == UserId, pageSize, pageNo, [query=>query.OrderByDescending(item=>item.CreatedAt)]);

            return (items.ToList(), totalCount);
        }



    }
}
