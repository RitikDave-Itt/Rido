using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rido.Data.Contexts;
using Rido.Data.Repositories;
using Rido.Data.Repositories.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Rido.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClients(builder.Configuration);

            builder.Services.ConfigureServices(builder.Configuration);
            builder.Services.ConfigureRepositories (builder.Configuration);



            builder.Logging.AddConsole();
            builder.Logging.AddDebug();



            var app = builder.Build();

            //app.Use(async(context, next) =>
            //{
            //    Console.WriteLine("before req");
            //    await next.Invoke();
            //    Console.WriteLine("after req");
            //});
            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            app.UseHttpsRedirection();
            app.UseAuthentication();  // Add Authentication before Authorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
