using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Exceptions
{
   
    public class NotValidUserException : Exception
    {
        public NotValidUserException() { }

        public NotValidUserException(string message) : base(message) { }



    }
}
