using Rido.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rido.Services.Interfaces;
using Rido.Model.Responses;
using Rido.Model.Enums;
namespace Rido.Services.Services
{


    public class BookingService:BaseService<RideBooking>,IBookingService
    {
        public BookingService(IServiceProvider serviceProvider) : base(serviceProvider){  
        }



        public async Task<(List<BookingsResponseDto> Items, int TotalCount)> GetUserBookings(string UserId, int pageNo, int pageSize)
        {
            var role = GetCurrentUserRole();
            if (role == UserRole.User.ToString())
            {
                var (items, totalCount) = await _repository.FindPageAsync(b => b.UserId == UserId, pageSize, pageNo, [query => query.OrderByDescending(item => item.CreatedAt)]);

                var bookings = _mapper.Map<List<BookingsResponseDto>>(items);



                return (bookings, totalCount);
            }
            else if (role == UserRole.Driver.ToString())
            {
                var (items, totalCount) = await _repository.FindPageAsync(b => b.DriverId == UserId, pageSize, pageNo, [query => query.OrderByDescending(item => item.CreatedAt)]);

                var bookings = _mapper.Map<List<BookingsResponseDto>>(items);



                return (bookings, totalCount);

            }
            else
            {
                return (null, 0);
            }
        }



    }
}
