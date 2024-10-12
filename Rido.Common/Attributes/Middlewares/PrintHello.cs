using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Attributes
{
    public class PrintHello : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("before req");
            try
            {
                var resultContext = await next();
            }
            finally
            {
                Console.WriteLine("after req");
            }
        }
    }


}
