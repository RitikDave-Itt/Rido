using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rido.Data.Contexts;
using Rido.Data.Repositories;
using Rido.Data.Repositories.Interfaces;

namespace Rido.Web
{
    public static class RepositoryExtensions
    {
        public static void ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RidoDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("connectionString")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }
    }
}
