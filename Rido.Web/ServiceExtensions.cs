using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rido.Services;
using Rido.Services.Interfaces;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Rido.Common.Mappings;
using AutoMapper;

namespace Rido.Web
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthServices, AuthService>();
            services.AddAutoMapper(typeof(UserProfile));


            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rido API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
