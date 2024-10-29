using Rido.Model.Enums;
using System;

namespace Rido.Data.Entities
{
    public class WalletTransaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();      
        public string UserId { get; set; }          
        public double Amount { get; set; }      
        public WalletTransactionType Type { get; set; }
        public WalletTransactionStatus Status { get; set; } = WalletTransactionStatus.Completed;        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;        
        public DateTime? UpdatedAt { get; set; }

        public string RazorPayId { get; set; } = null;

        public string Remarks { get; set; }       

        public User User { get; set; }
    }

    
}
