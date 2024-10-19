using Newtonsoft.Json.Linq;
using Rido.Common.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Models.Responses
{
    public class ReverseGeocodeResponseDto
    {
        public string DisplayName { get; set; }
        public AddressType Address { get; set; }
    }
}
