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
        private IBaseRepository<RideRequest> _rideRequestRepository;

        public RideReviewService(IBaseRepository<RideRequest>rideRequestRepositor, IServiceProvider serviceProvider) :base(serviceProvider){

            _rideRequestRepository = rideRequestRepositor; 
        }

        public async Task<string> CreateReview(RideReviewRequestDto reviewDto)
        {
            var booking = await _rideRequestRepository.GetByIdAsync(reviewDto.BookingId);

            if (booking == null) {
                return null;
            
            }
            var userId = GetCurrentUserId();

            if (userId != booking.RiderId)
            {
                throw new NotValidUserException("Not Valid User");
            }

            RideReview review = new RideReview()
            {
                RideRequestId = booking.Id,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                DriverId = booking.DriverId,
                UserId = booking.RiderId,

            };

            var result = await _repository.AddAsync(review);

            return result!=null ? result.Id : null;




        }
    }
}
