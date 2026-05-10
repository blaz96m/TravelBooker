using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading.RateLimiting;
using TravelBooker.Application.Common.Config;
using TravelBooker.Application.Common.Contracts.Mapping;
using TravelBooker.Application.Common.Contracts.Persistence;
using TravelBooker.Application.Common.Contracts.Services;
using TravelBooker.Application.Logging.Services;
using TravelBooker.Application.User.Contracts.Repositories;
using TravelBooker.Application.User.Contracts.Services;
using TravelBooker.Application.User.Facades;
using TravelBooker.Application.User.Services;
using TravelBooker.Application.User.Validation;
using TravelBooker.Domain;
using TravelBooker.Infrastructure.Common.Factories;
using TravelBooker.Infrastructure.Common.Services;
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

        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpConfig>(configuration.GetSection("SmtpSettings"));
            services.AddOptions<SmtpConfig>()
                .Bind(configuration.GetSection("SmtpSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        public static void RegisterEmailService(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
        }

        public static void RegisterDataProtection(this IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddSingleton<IDataEncryptionService, DataEncryptionService>();
        }

        public static void RegisterUserServices(this IServiceCollection services)
        {
            services.AddScoped<UserAuthenticationServiceFacade>();
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        }
        public static void RegisterServices(this IServiceCollection services)
        {
            services.RegisterUserServices();
        }

        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<UserLoginValidator>();
        }

        public static void ConfigureRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, cancellationToken) =>
                {
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                    }

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken);

                };

                var globalLimit = configuration.GetSection("RateLimiting:Global");
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
                    RateLimitPartition.GetFixedWindowLimiter("global", _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = globalLimit.GetValue<int>("PermitLimit"),
                        Window = TimeSpan.FromSeconds(globalLimit.GetValue<int>("WindowSeconds"))
                    }));

                var sensitiveLimit = configuration.GetSection("RateLimiting:SensitiveEndpoint");
                options.AddPolicy("SensitiveEndpoint", context =>
                {
                    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    var path = context.Request.Path.ToString();
                    return RateLimitPartition.GetSlidingWindowLimiter($"{ip}:{path}", _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = sensitiveLimit.GetValue<int>("PermitLimit"),
                        Window = TimeSpan.FromSeconds(sensitiveLimit.GetValue<int>("WindowSeconds")),
                        SegmentsPerWindow = sensitiveLimit.GetValue<int>("SegmentsPerWindow")
                    });
                });
            });
        }
    }
}
