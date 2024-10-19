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
                      FirstName = src.GetProperty("FirstName").GetString(),
                      LastName = src.GetProperty("LastName").GetString(),
                      Email = src.GetProperty("Email").GetString(),
                      PhoneNumber = src.GetProperty("PhoneNumber").GetString(),
                      PasswordHash = src.GetProperty("Password").GetString()      
                  };

                  if (src.TryGetProperty("Gender", out JsonElement genderElement) &&
                      Enum.TryParse<Gender>(genderElement.GetString(), true, out Gender gender))
                  {
                      dto.Gender = gender;
                  }

                  if (src.TryGetProperty("Role", out JsonElement roleElement) &&
                      Enum.TryParse<UserRole>(roleElement.GetString(), true, out UserRole role))
                  {
                      dto.Role = role;
                  }
                  return dto;
              });
        }
    }
}
