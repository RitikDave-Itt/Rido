using AutoMapper;
using Rido.Common.Models.Requests;
using Rido.Data.Entities;

namespace Rido.Common.Mappings
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<RegisterDriverDto, DriverData>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
