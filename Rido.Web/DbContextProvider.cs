using Microsoft.EntityFrameworkCore;
using Rido.Data.Contexts;

namespace Rido.Web
{
    public static class DbContextProvider
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RidoDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("connectionString")));
        }
    }
}
