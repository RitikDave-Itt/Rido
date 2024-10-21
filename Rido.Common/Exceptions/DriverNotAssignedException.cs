using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Exceptions
{
    public class DriverNotAssignedException  : Exception
    {
        public DriverNotAssignedException() { }

        public DriverNotAssignedException(string message) : base(message) {
        
        }
    }
}
