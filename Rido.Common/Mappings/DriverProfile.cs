using AutoMapper;
using Rido.Common.Models.Requests;
using Rido.Data.Entities;
using Rido.Data.Enums;
using System.Text.Json;

namespace Rido.Common.Mappings
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<RegisterDriverDto, DriverData>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.VehicleMake, opt => opt.MapFrom(src => src.VehicleMake));

            CreateMap<JsonElement, RegisterDriverDto>().ConvertUsing((src, dest) =>
            {
                var dto = new RegisterDriverDto();

                if (src.TryGetProperty("VehicleRegistrationNumber", out JsonElement registrationNumberElement))
                {
                    dto.VehicleRegistrationNumber = registrationNumberElement.GetString();
                }

                if (src.TryGetProperty("VehicleMake", out JsonElement vehicleMakeElement))
                {
                    dto.VehicleMake = vehicleMakeElement.GetString();
                }

                if (src.TryGetProperty("VehicleType", out JsonElement vehicleTypeElement) &&
                    Enum.TryParse<VehicleType>(vehicleTypeElement.GetString(), true, out VehicleType vehicleType))
                {
                    dto.VehicleType = vehicleType;
                }
                else
                {
                    throw new ArgumentException("Vehicle Type is Required");
                }

                if (src.TryGetProperty("LicenceType", out JsonElement licenceTypeElement))
                {
                    dto.LicenseType = licenceTypeElement.GetString();
                }

                if (src.TryGetProperty("LicenceNumber", out JsonElement licenceNumberElement))
                {
                    dto.LicenseNumber = licenceNumberElement.GetString();
                }

                

                if (src.TryGetProperty("VehicleModel", out JsonElement vehicleModelElement))
                {
                    dto.VehicleModel = vehicleModelElement.GetString();
                }

                return dto;
            });
        }
    }
}
