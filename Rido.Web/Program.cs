using Rido.Common;
using Rido.Data;
using Rido.Services;
using Rido.Web.Hubs;

namespace Rido.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.ConfigureDbContext(builder.Configuration);
            builder.Services.ConfigureRepositories();
            builder.Services.AddHttpClients(builder.Configuration);
            builder.Services.ConfigureAuthentication(builder.Configuration);
            builder.Services.ConfigureServices();
            builder.Services.ConfigureUtility();
            builder.Services.AddHttpContextAccessor();
            builder.Services.ConfigureSwagger();
            builder.Services.AddControllers();
            builder.Services.SetupCors();

            //builder.Services.AddSingleton<ILogger<T>, Logger<T>>();
            builder.Services.AddLogging();


            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            builder.Services.AddSignalR();


            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }


            app.UseCors("AllowOrigins");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatHub>("/chat");
            app.MapControllers();

            app.Run();
        }
    }
}
