using AutoMapper.Configuration.Annotations;
using Rido.Model.Enums;
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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public WalletStatus WalletStatus { get; set; } = WalletStatus.Active;

   


    }
}
