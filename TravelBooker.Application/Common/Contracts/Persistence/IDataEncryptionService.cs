namespace TravelBooker.Application.Common.Contracts.Factories
{
    public interface IDataEncryptionFactory
    {
        string CreateEncryptionKey(string identifier, string[] tokenDetails);

        string CreateTimeLimitedEncryptionKey(string identifier, string[] tokenDetails, TimeSpan expiresAfter);

        string[]? DecryptKey(string identifier, string token);
    }
}
