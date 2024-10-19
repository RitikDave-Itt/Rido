using System;
using System.ComponentModel.DataAnnotations;
using Rido.Data.Enums;

namespace Rido.Common.Models.Requests
{
    public class RegisterDriverDto
    {

        [StringLength(50, ErrorMessage = "License Type cannot exceed 50 characters.")]
        public string? LicenseType { get; set; }

        [StringLength(50, ErrorMessage = "License Number cannot exceed 50 characters.")]
        public string? LicenseNumber { get; set; }

        [Required(ErrorMessage = "Vehicle Type is required.")]
        [EnumDataType(typeof(VehicleType), ErrorMessage = "Invalid Vehicle Type.")]
        public VehicleType VehicleType { get; set; }

        [StringLength(50, ErrorMessage = "Vehicle Registration Number cannot exceed 50 characters.")]
        public string? VehicleRegistrationNumber { get; set; }

        [StringLength(50, ErrorMessage = "Vehicle Model cannot exceed 50 characters.")]
        public string? VehicleModel { get; set; }

        [StringLength(50, ErrorMessage = "Vehicle Make cannot exceed 50 characters.")]
        public string VehicleMake { get; set; }
    }
}
