using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Model.Responses
{
    public class FareListResponseDto
    {
        public VehicleDto? Bike { get; set; }
        public VehicleDto? Sedan { get; set; }
        public VehicleDto? SUV { get; set; }
        public VehicleDto? Coupe { get; set; }
        public VehicleDto? Van { get; set; }
        public VehicleDto? AutoRikshaw { get; set; }
        public VehicleDto? Other { get; set; }
    }
}
