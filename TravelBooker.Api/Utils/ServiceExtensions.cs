using Microsoft.EntityFrameworkCore;
using TravelBooker.Application.Common.Contracts.Mapping;
using TravelBooker.Application.Logging.Contracts;
using TravelBooker.Application.Logging.Services;
using TravelBooker.Domain;
using TravelBooker.Infrastructure.Context;
using TravelBooker.Infrastructure.Entities;
using TravelBooker.Infrastructure.User.Mapping;

namespace TravelBooker.Api.Common.Extensions
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

        public static void RegisterEntityMappers(this IServiceCollection services)
        {
            services.AddSingleton<IBaseEntityMapper<UserLogin, UserLoginEntity>, UserLoginEntityMapper>();
        }
    }
}
