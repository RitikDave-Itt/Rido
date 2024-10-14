using Microsoft.Extensions.DependencyInjection;
using Rido.Services;

namespace Rido.Web
{
    public static class HttpClientsExtensions
    {
        public static void AddHttpClients(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddHttpClient<LocationServices>("location",
                client =>{
                client.BaseAddress = new Uri("https://us1.locationiq.com/v1/");
                    var apiKey = configuration["LocationIQ:ApiKey"];
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                });
        }
    }
}
