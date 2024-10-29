using Newtonsoft.Json.Linq;
using Rido.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Model.Responses
{
    public class ReverseGeocodeResponseDto
    {
        public string DisplayName { get; set; }
        public AddressType Address { get; set; }
    }
}
