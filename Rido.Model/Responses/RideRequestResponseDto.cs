using Rido.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Model.Responses
{
    public class RideRequestResponseDto
    {
        public string Id { get; set; }

        public string PickupLatitude { get; set; }
        public string PickupLongitude { get; set; }
        public string PickupAddress { get; set; }
        public DateTime PickupTime { get; set; }

        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
        public string DestinationAddress { get; set; }

        public decimal Price { get; set; }     

        public VehicleType VehicleType { get; set; }
        public string GeohashCode { get; set; }
        public double DistanceInKm { get; set; }

        public string? UserName { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
