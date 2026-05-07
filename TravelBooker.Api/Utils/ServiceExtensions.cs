using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelBooker.Application.Common.Contracts.Mapping;
using TravelBooker.Application.Logging.Contracts;
using TravelBooker.Application.Logging.Services;
using TravelBooker.Application.User.Contracts.Repositories;
using TravelBooker.Domain;
using TravelBooker.Infrastructure.Context;
using TravelBooker.Infrastructure.Entities;
using TravelBooker.Infrastructure.User.Mapping;
using TravelBooker.Infrastructure.User.Repositories;

namespace TravelBooker.Api.Utils
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

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserLoginRepository, UserLoginRepository>();
        }

        public static void ConfigurePasswordHasher(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<UserLogin>, PasswordHasher<UserLogin>>();
        }
    }
}
