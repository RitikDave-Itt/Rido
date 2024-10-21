using AutoMapper.Configuration.Annotations;
using Rido.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rido.Data.Entities
{
    public class RideBooking
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? UserId { get; set; }
        public string? DriverId { get; set; }

        public DateTime PickupTime { get; set; }
        public DateTime DropoffTime { get; set; }

        public string PickupLatitude { get; set; }
        public string PickupLongitude { get; set; }
        public string PickupAddress { get; set; }

        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
        public string DestinationAddress { get; set; }

     

        public VehicleType VehicleType { get; set; }
        public double DistanceInKm { get; set; }

        public string GeohashCode { get; set; }

        public string TransactionId { get; set; }      
        public decimal Amount { get; set; }


    

        public DateTime CreatedAt { get; set; } = DateTime.Now;     
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        public User? User { get; set; }
        public User? Driver { get; set; }

        public RideTransaction? RideTransaction { get; set; }
        

    }
}
