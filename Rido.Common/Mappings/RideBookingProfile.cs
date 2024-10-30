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
    public class RideBookingProfile:Profile
    {
        public RideBookingProfile() {

            CreateMap<RideRequest, BookingsResponseDto>()
                            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType.ToString()))
                            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => (decimal)src.Amount))
                            .ForMember(dest => dest.PickupAddress, opt => opt.MapFrom(src => src.PickupAddress))
                            .ForMember(dest => dest.DestinationAddress, opt => opt.MapFrom(src => src.DestinationAddress))
                            .ForMember(dest => dest.PickupTime, opt => opt.MapFrom(src => src.PickupTime));
        
    }
    }
}
