using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rido.Common.Mappings;
using Rido.Common.Secrets;
using Rido.Data.Repositories.Interfaces;
using Rido.Data.Repositories;
using Rido.Services;
using Rido.Services.Interfaces;
using System.Reflection;
using System.Text;

namespace Rido.Web
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<JwtUtil>();
            services.AddScoped<IAuthServices, AuthService>();
            services.AddScoped<IDriverService, DriverService>();

            services.AddScoped<IDriverLocationService, DriverLocationService>();
            services.AddScoped<IRideService, RideService>();
            services.AddScoped<LocationUtils>();
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<RideService>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rido API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter your Bearer token in the format **Bearer {token}**",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                     {
                         {
                             new OpenApiSecurityScheme
                                  {
                                     Reference = new OpenApiReference
                                                  {
                                      Type = ReferenceType.SecurityScheme,
                                       Id = "Bearer"
                                   }
                       },
            Array.Empty<string>()
        }
    });
            });


            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

            services.Configure<LocationSecrets>(configuration.GetSection("LocationIQ"));

            var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            services.AddAuthorization();
        }
    }
}
