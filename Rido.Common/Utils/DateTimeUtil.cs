using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Utils
{
    public static class DateTimeUtil
    {
        public static DateTime CurrentDateTime()
        {
            return DateTime.UtcNow;

        }

    }
}
