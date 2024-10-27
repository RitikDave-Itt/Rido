using AutoMapper;
using Rido.Common.Models.Requests;
using Rido.Data.Entities;
using Rido.Data.Enums;
using System.Text.Json;

namespace Rido.Common.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDto, User>();

            CreateMap<JsonElement, RegisterUserDto>()
              .ConvertUsing((src, dest) =>
              {
                  var dto = new RegisterUserDto
                  {
                      FirstName = src.GetProperty("firstName").GetString(),
                      LastName = src.GetProperty("lastName").GetString(),
                      Email = src.GetProperty("email").GetString(),
                      PhoneNumber = src.GetProperty("phoneNumber").GetString(),
                      PasswordHash = src.GetProperty("password").GetString()      
                  };

                  if (src.TryGetProperty("gender", out JsonElement genderElement) &&
                      Enum.TryParse<Gender>(genderElement.GetString(), true, out Gender gender))
                  {
                      dto.Gender = gender;
                  }

                  if (src.TryGetProperty("role", out JsonElement roleElement) &&
                      Enum.TryParse<UserRole>(roleElement.GetString(), true, out UserRole role))
                  {
                      dto.Role = role;
                  }
                  return dto;
              });
        }
    }
}
