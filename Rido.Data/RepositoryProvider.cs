using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rido.Data.Contexts;
using Rido.Data.Repositories.Interfaces;
using Rido.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Data
{
    public static class RepositoryProvider
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            //services.AddScoped<IRideRequestRepository, RideRequestRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRideTransactionRepository, RideTransactionRepository>();



        }

    }
}
