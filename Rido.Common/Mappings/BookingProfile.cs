using AutoMapper;
using Rido.Data.Entities;
using Rido.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile() {
            CreateMap<RideBooking, BookingsResponseDto>()
                 .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType.ToString()));

        }
    }
}
