//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rido.Common.Models.Requests
//{
//    public class RideRequestDto
//    {
//        [Required(ErrorMessage = "Pickup location is required.")]
//        [LocationValidate]      
//        public LocationType Pickup { get; set; }

//        [Required(ErrorMessage = "Destination location is required.")]
//        [LocationValidate]      
//        public LocationType Destination { get; set; }
//        public DateTime? Time { get; set; }
//    }

//}
