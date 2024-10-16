using AutoMapper;
using Rido.Common.Models.Requests;
using Rido.Data.DTOs;
using Rido.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
