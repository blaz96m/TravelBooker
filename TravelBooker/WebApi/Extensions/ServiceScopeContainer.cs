using TraverlBooker.Core.Services.Abstractions;

namespace TravelBooker.WebApi.Extensions
{
    public static class ServiceScopeContainer
    {
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerService, ILoggerService>();

    }
}
