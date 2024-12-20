﻿using Rido.Model.Enums;

namespace Rido.Data.Entities
{
    public class RideRequest
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public string? DriverId { get; set; }


        public string PickupLatitude { get; set; }
        public string PickupLongitude { get; set; }
        public string PickupAddress { get; set; }
        public DateTime PickupTime { get; set; }


        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
        public string DestinationAddress { get; set; }

        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        public VehicleType VehicleType { get; set; }
        public string GeohashCode { get; set; }
        public double DistanceInKm { get; set; }
        public RideRequestStatus Status { get; set; } = RideRequestStatus.Requested;

        public User? Rider { get; set; }
        public User? Driver { get; set; }






    }
}
