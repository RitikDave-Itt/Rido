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
        [StringLength(255, ErrorMessage = "PickupAddress exceed limit.")]
        public string PickupAddress { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "PickupTime not valid date and time.")]
        public DateTime PickupTime { get; set; }

        [Required]
        public string DestinationLatitude { get; set; }

        [Required]
        public string DestinationLongitude { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "DestinationAddress exceed limit.")]
        public string DestinationAddress { get; set; }

        

        [Required]
        public VehicleType VehicleType { get; set; }

     

       

       
    }
}
