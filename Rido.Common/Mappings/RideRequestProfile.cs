using AutoMapper;
using Rido.Common.Models.Responses;
using Rido.Data.DTOs;
using Rido.Data.Entities;
using Rido.Data.Enums;

namespace Rido.Common.Mappings
{
    public class RideRequestProfile : Profile
    {
        public RideRequestProfile()
        {
            CreateMap<RideRequestDto, RideRequest>();
 


        }
    }
}
