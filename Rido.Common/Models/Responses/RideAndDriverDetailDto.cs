using Rido.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Models.Responses
{
    public class RideAndDriverDetailDto
    {
        public string User_Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Gender { get; set; }
        public string? ProfileImageUrl { get; set; }

        public string RideRequestId { get; set; }
        public string PickupLatitude { get; set; }
        public string PickupLongitude { get; set; }
        public string PickupAddress { get; set; }
        public DateTime PickupTime { get; set; }
        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
        public string DestinationAddress { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public double DistanceInKm { get; set; }

        public VehicleType VehicleType { get; set; }
        public string? VehicleModel { get; set; }
        public string? VehicleMake { get; set; }

    }
}
