using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Models.Types
{
    public class LocationType
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public LocationType(string latitude, string longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public LocationType() { }
    }
}
