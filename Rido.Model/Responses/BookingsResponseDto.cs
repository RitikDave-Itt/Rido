using Rido.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Model.Responses
{
    public class BookingsResponseDto
    {
        public string Id { get; set; }
        public string PickupAddress { get; set; }
        public string DestinationAddress { get; set; }
        public string VehicleType { get; set; }
        public decimal Amount { get; set; }
        public DateTime PickupTime { get; set; }
    }
}
