using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;
using TravelBooker.Application.Common.Contracts.Factories;

namespace TravelBooker.Infrastructure.Common.Factories
{
    public class DataEncryptionService : IDataEncryptionService
    {
        private readonly IDataProtectionProvider _dataProtector;
        private const string tokenDetailsSeparator = "|";
        public DataEncryptionService(IDataProtectionProvider dataProtector)
        {
            _dataProtector = dataProtector;
        }

        private string GenerateTokenPayload(string[] tokenDetails) => string.Join(tokenDetailsSeparator, tokenDetails);

        private string[] DestructurePayloadFromToken(string token) => token.Split(tokenDetailsSeparator);

        public string CreateEncryptionKey(string identifier, string[] tokenDetails)
        {
            var protector = _dataProtector.CreateProtector(identifier);
            return protector.Protect(GenerateTokenPayload(tokenDetails));
        }

        public string CreateTimeLimitedEncryptionKey(string identifier, string[] tokenDetails, TimeSpan expiresAfter)
        {
            var protector = _dataProtector.CreateProtector(identifier).ToTimeLimitedDataProtector();
            return protector.Protect(GenerateTokenPayload(tokenDetails), expiresAfter);
        }

        public string[]? DecryptKey(string identifier, string token)
        {
            try
            {
                var protector = _dataProtector.CreateProtector(identifier);
                return DestructurePayloadFromToken(protector.Unprotect(token));
            }
            catch (CryptographicException)
            {
                return null;
            }

        }
    }
}
