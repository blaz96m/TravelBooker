using TravelBooker.Application.Common.Contracts.Services;
using TravelBooker.Application.Logging.Contracts;

namespace TravelBooker.Application.User.Facades
{
    public class UserAuthenticationServiceFacade
    {
        private readonly ILoggerService _loggerService;
        private readonly IEmailService _emailService;
        public UserAuthenticationServiceFacade(ILoggerService loggerService, IEmailService emailService)
        {
            _loggerService = loggerService;
            _emailService = emailService;
        }

        public IEmailService EmailService => _emailService;

        public ILoggerService LoggerService => _loggerService;
    }
}
