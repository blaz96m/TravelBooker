
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using TravelBooker.Api.Utils;

namespace TravelBooker.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.ConfigureCors();
            builder.Services.ConfigureIISIntegration();
            builder.Host.UseSerilog((hostContext, configuration) =>
            {
                configuration.ReadFrom.Configuration(hostContext.Configuration);
            });
            builder.Services.ConfigureLogger();
            builder.Services.RegisterDataProtection();
            builder.Services.ConfigurePasswordHasher();
            builder.Services.ConfigurePostgresContext(builder.Configuration);
            builder.Services.RegisterEntityMappers();
            builder.Services.RegisterRepositories();
            builder.Services.ConfigureAppSettings(builder.Configuration);

            builder.Services.RegisterEmailService();
            builder.Services.RegisterUserServices();
            builder.Services.RegisterValidators();
            builder.Services.ConfigureRateLimiting(builder.Configuration);
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseRateLimiter();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
