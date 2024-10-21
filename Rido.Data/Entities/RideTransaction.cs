using System;
using Rido.Data.Enums;

namespace Rido.Data.Entities
{
    public class RideTransaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();       
        public string? UserId { get; set; }      
        public string? DriverId { get; set; }     
        public decimal Amount { get; set; }      
        public DateTime Date { get; set; } = DateTime.Now;       
        public RideTransactionStatus Status { get; set; }       
        public string Remarks { get; set; }      

        public User? Rider { get; set; }

        public User? Driver { get; set; }
        

    }
}
