using Serilog;
using TraverlBooker.Core.Services.Abstractions;

namespace TravelBooker.Core.Services
{
    public class LoggerManager(ILogger logger) : ILoggerService
    {
        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Info(string message)
        {
            logger?.Information(message);
        }

        public void Warning(string message)
        {
            logger.Warning(message);
        }
    }
}
