
using Ocelot.DependencyInjection;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Ocelot with configuration
            builder.Configuration.AddJsonFile("ocelot.json");
            builder.Services.AddOcelot(builder.Configuration);

            // Add CORS for local Angular development
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("LocalAngular", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Middleware pipeline
            app.UseCors("LocalAngular");
            app.UseOcelot().Wait();  // For .NET 6+ without async main

            app.Run();
        }
    }
}
