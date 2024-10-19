using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rido.Data.Entities
{
    public class OneTimePassword
    {
        public string Id = Guid.NewGuid().ToString();
        public string RideRequestId {  get; set; }
        public string OTP {  get; set; }

        public int TryCount { get; set; } = 0;

        [JsonIgnore]
        [Ignore]

        public RideRequest RideRequest { get; set; }

    }
}
