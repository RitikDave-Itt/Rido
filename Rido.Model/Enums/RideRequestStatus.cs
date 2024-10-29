using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Model.Enums
{
    public enum RideRequestStatus
    {
        Requested,              
        Accepted,               
        InProgress,   
        Unpaid,
        Completed,            
        Canceled,                  
        Failed                  
    }
}
