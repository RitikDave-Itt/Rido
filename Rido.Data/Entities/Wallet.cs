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
    public class Wallet
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public decimal Balance { get; set; }
        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public WalletStatus WalletStatus { get; set; } = WalletStatus.Active;

        [JsonIgnore]
        [Ignore]

        public User User { get; set; }


    }
}
