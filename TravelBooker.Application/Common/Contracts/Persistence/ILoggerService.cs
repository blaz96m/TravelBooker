namespace TravelBooker.Application.Common.Contracts.Persistence
{
    public interface ILoggerService
    {
        void LogDebug(string message);
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);

    }
}
