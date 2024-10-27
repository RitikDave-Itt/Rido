namespace Rido.Web
{
    public static class CorsPolicy
    {
        public static void SetupCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigins", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")      
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();      
                });

            });
        }
    }

}