using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Exceptions
{
    public  class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException() { }

        public InsufficientBalanceException(string message) : base(message) { }
    }
}
