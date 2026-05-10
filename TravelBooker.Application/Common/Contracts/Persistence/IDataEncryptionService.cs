namespace TravelBooker.Application.Common.Contracts.Persistence
{
    public interface IDataEncryptionService
    {
        string CreateEncryptionKey(string identifier, string[] tokenDetails);

        string CreateTimeLimitedEncryptionKey(string identifier, string[] tokenDetails, TimeSpan expiresAfter);

        string[]? DecryptKey(string identifier, string token);
    }
}
