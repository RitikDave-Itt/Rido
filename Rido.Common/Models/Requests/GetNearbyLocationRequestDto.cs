using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Models.Requests
{
    public class GetNearbyLocationRequestDto
    {
        
        public string Destination { get; set; }
        public string? City { get; set; }
        public string? County { get; set; }
        public string? State { get; set; }
        public string? StateCode { get; set; }
        public string? Postcode { get; set; }
        public string? Country { get; set; }
        public string? Country_code { get; set; }
    }

}
