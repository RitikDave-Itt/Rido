using AutoMapper;
using Rido.Model.Responses;
using Rido.Data.DTOs;
using Rido.Data.Entities;
using Rido.Model.Enums;

namespace Rido.Common.Mappings
{
    public class RideRequestProfile : Profile
    {
        public RideRequestProfile()
        {
            CreateMap<RideRequestDto, RideRequest>()
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => VehicleType.Other));

            CreateMap<RideRequest, RideRequestResponseDto>()
        .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.MaxPrice))
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Rider != null ? src.Rider.FirstName + " " + src.Rider.LastName : null))
        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Rider != null ? src.Rider.Gender.ToString() : null))
        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Rider != null ? src.Rider.PhoneNumber : null));


        }
    }
}
