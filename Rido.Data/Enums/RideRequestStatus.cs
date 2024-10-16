using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data.Enums
{
    public enum RideRequestStatus
    {
        Requested,              
        Pending,                 
        Accepted,               
        InProgress,            
        Completed,            
        Canceled,                  
        Failed                  
    }
}
