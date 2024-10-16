using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Models.Responses
{
    public class RideAndDriverDetailDto
    {
        public string RideRequestId { get; set; }
        public string PickupAddress { get; set; }
        public string PickupLatitude { get; set; }
        public string PickupLongitude { get; set; }
        public DateTime PickupTime { get; set; }
        public string DestinationAddress { get; set; }
        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string RideStatus { get; set; }         

        public string DriverId { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public string DriverPhoneNumber { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleMake { get; set; }

    }
}
