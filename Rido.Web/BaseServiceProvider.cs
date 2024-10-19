using Rido.Common.Mappings;

namespace Rido.Web
{
    public static class BaseServiceProvider
    {
        public static void BaseServiceConfig(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddAutoMapper(typeof(UserProfile),typeof(DriverProfile),typeof(RideRequestProfile));
            services.AddHttpContextAccessor();
        }
    }
}
