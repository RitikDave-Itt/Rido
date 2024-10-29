using Rido.Common.Exceptions;
using Rido.Model.Requests;
using Rido.Data.DTOs;
using Rido.Data.Entities;
using Rido.Data.Repositories.Interfaces;
using Rido.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Services
{
    public class RideReviewService : BaseService<RideReview> ,IRideReviewService
    {
        public IBaseRepository<RideReview> _rideReviewRepository;
        public IBaseRepository<RideBooking> _rideBookingRepository;

        public RideReviewService(IBaseRepository<RideReview> rideReviewRepo, IBaseRepository<RideBooking> rideBookingRepository ,IServiceProvider serviceProvider) :base(serviceProvider){
            _rideBookingRepository = rideBookingRepository;
        _rideReviewRepository = rideReviewRepo;
        }

        public async Task<string> CreateReview(RideReviewRequestDto reviewDto)
        {
            var booking = await _rideBookingRepository.GetByIdAsync(reviewDto.BookingId);

            if (booking == null) {
                return null;
            
            }
            var userId = GetCurrentUserId();

            if (userId != booking.UserId)
            {
                throw new NotValidUserException("Not Valid User");
            }

            RideReview review = new RideReview()
            {
                BookingId = booking.Id,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                DriverId = booking.DriverId,
                UserId = booking.UserId,

            };

            var result  = await _rideReviewRepository.AddAsync(review);

            return result!=null ? result.Id : null;




        }
    }
}
