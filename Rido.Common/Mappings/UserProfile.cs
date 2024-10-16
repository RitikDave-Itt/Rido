using AutoMapper;
using Rido.Common.Models.Requests;
using Rido.Data.Entities;

namespace Rido.Common.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDto, User>()
                ;
        }
    }
}
