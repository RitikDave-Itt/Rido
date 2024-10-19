using Microsoft.Extensions.DependencyInjection;
using Rido.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common
{
    public static class UtilityProvider
    {
        public static void ConfigureUtility(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddAutoMapper(typeof(UserProfile), typeof(DriverProfile), typeof(RideRequestProfile));
        }
    }
}
