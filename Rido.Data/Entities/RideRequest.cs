using Rido.Model.Enums;

namespace Rido.Data.Entities
{
    public class RideRequest
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? RiderId { get; set; }
        public string? DriverId { get; set; }


        public string PickupLatitude { get; set; }
        public string PickupLongitude { get; set; }
        public string PickupAddress { get; set; }
        public DateTime PickupTime { get; set; }
        public DateTime DropoffTime { get; set; }

        public bool IsActive { get; set; } = true;



        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
        public string DestinationAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string GeohashCode { get; set; }

        public double Amount { get; set; }
        public string? TransactionId { get; set; }

        public UserRole? CancelBy { get; set; }
        public string? CancelReason { get; set; }

        



        public VehicleType VehicleType { get; set; }
        public double DistanceInKm { get; set; }
        public RideRequestStatus Status { get; set; } = RideRequestStatus.Requested;

        public User? Rider { get; set; }
        public User? Driver { get; set; }
        public RideTransaction? RideTransaction { get; set; }
        public RideReview? RideReview { get; set; }

    }
}
