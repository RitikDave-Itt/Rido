using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Entities
{


    
        public class RideReview
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();      
            public string? UserId { get; set; }         
            public string? DriverId { get; set; }        
            public string? BookingId { get; set; }

        [Range(0.0, 5.0, ErrorMessage = "Rating should be less then 5.")]

        public decimal Rating { get; set; }          
            public string? Comment { get; set; }       
            public DateTime CreatedAt { get; set; } = DateTime.Now;        

            public User? User { get; set; }      
            public User? Driver { get; set; }      
            public RideBooking? Booking { get; set; }      
        }
    }


