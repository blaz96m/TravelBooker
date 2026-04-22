using Serilog;
using TravelBooker.Application.Logging.Contracts;

namespace TravelBooker.Application.Logging.Services
{
    public class LoggerService(ILogger logger) : ILoggerService
    {
        public void LogDebug(string message) => logger.Debug(message);
        public void LogInformation(string message) => logger.Information(message);
        public void LogWarning(string message) => logger.Warning(message);
        public void LogError(string message) => logger.Error(message);
    }
}
