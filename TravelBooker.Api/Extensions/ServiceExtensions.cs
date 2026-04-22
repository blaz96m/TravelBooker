using Microsoft.EntityFrameworkCore;
using TravelBooker.Application.Logging.Contracts;
using TravelBooker.Application.Logging.Services;
using TravelBooker.Infrastructure.Context;

namespace TravelBooker.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                 builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());
            });

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {

            });

        public static void ConfigureLogger(this IServiceCollection services) =>
            services.AddSingleton<ILoggerService, LoggerService>();

        public static void ConfigurePostgresContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("travelBooker")));

        }
    }
}
