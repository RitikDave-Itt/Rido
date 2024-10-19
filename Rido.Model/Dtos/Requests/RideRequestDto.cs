using Rido.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Rido.Data.DTOs
{
    public class RideRequestDto
    {
      

        [Required]
        public string PickupLatitude { get; set; }

        [Required]
        public string PickupLongitude { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "PickupAddress cannot exceed 255 characters.")]
        public string PickupAddress { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "PickupTime must be a valid date and time.")]
        public DateTime PickupTime { get; set; }

        [Required]
        public string DestinationLatitude { get; set; }

        [Required]
        public string DestinationLongitude { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "DestinationAddress cannot exceed 255 characters.")]
        public string DestinationAddress { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "MinPrice must be a positive value.")]
        public decimal MinPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "MaxPrice must be a positive value.")]
        public decimal MaxPrice { get; set; }

        [Required]
        public VehicleType VehicleType { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "GeohashCode cannot exceed 50 characters.")]
        public string GeohashCode { get; set; }

        [Required]
        [Range(0, 100.0, ErrorMessage = "DistanceInKm must be a positive value.")]
        public double DistanceInKm { get; set; }

        [EnumDataType(typeof(RideRequestStatus), ErrorMessage = "Invalid status value.")]
        public RideRequestStatus Status { get; set; } = RideRequestStatus.Requested;
    }
}
