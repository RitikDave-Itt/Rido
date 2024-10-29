using Microsoft.Extensions.DependencyInjection;
using Rido.Services.Interfaces;
using Rido.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services
{
    public static class ServiceProvider
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthServices, AuthService>();
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IDriverLocationService, DriverLocationService>();
            services.AddScoped<IRideService, RideService>();
            services.AddScoped<LocationUtils>();     
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            services.AddScoped<IRideTransactionService, RideTransactionService>();
            services.AddScoped<IRideReviewService, RideReviewService>();
            services.AddScoped<IWalletTransactionService, WalletTransactionService>();
            services.AddScoped<IUserService, UserService>();

        }
    }
}
