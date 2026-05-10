using Microsoft.AspNetCore.Identity;
using TravelBooker.Application.Common.Contracts.Persistence;
using TravelBooker.Application.Common.Contracts.Services;
using TravelBooker.Application.Common.Enums;
using TravelBooker.Application.Common.Models;
using TravelBooker.Application.Common.Models.Response;
using TravelBooker.Application.Common.Utils;
using TravelBooker.Application.User.Constants;
using TravelBooker.Application.User.Contracts.Repositories;
using TravelBooker.Application.User.Contracts.Services;
using TravelBooker.Application.User.Dto;
using TravelBooker.Application.User.Mapping;
using TravelBooker.Domain;

namespace TravelBooker.Application.User.Services
{
    public class UserAuthenticationService(
        IUserLoginRepository userLoginRepository,
        IPasswordHasher<UserLogin> passwordHasher,
        IDataEncryptionService dataEncryptionFactory,
        ILoggerService loggerService,
        IEmailService emailService
        ) : IUserAuthenticationService
    {
        private readonly IUserLoginRepository _userLoginRepository = userLoginRepository;
        private readonly IPasswordHasher<UserLogin> _passwordHasher = passwordHasher;
        private readonly IDataEncryptionService _dataEncryptionFactory = dataEncryptionFactory;
        private readonly ILoggerService _loggerService = loggerService;
        private readonly IEmailService _emailService = emailService;

        private const string _emailVerificationTokenIdentifier = "EmailVerification";

        private string GenerateAccountVerificationLink(UserLogin userLoginData)
        {
            var verificationToken = _dataEncryptionFactory.CreateTimeLimitedEncryptionKey(
                _emailVerificationTokenIdentifier, [userLoginData.Id.ToString(), userLoginData.Email], TimeSpan.FromDays(13)
                );
            // ONLY TEMPORARY LINK
            return $"https://localhost:5001/api/account-verification?verificationToken={verificationToken}";
        }

        public async Task<Result> RegisterUser(UserLoginDto userLoginDto)
        {
            var userEmailExists = await _userLoginRepository.UserLoginDataExistsAsync(userLoginDto.Email);
            if (userEmailExists)
            {
                return ResponseHelpers.GenerateFailedResult(ErrorType.Conflict, UserErrorMessages.UserEmailAlreadyExists);
            }
            var mapper = new UserLoginDomainMapper();
            var domainModel = mapper.ToDomain(userLoginDto);

            var hashedPassword = _passwordHasher.HashPassword(domainModel, domainModel.Password);
            domainModel.Password = hashedPassword;


            var createdUser = await _userLoginRepository.CreateUserAsync(domainModel);
            var conformationLink = GenerateAccountVerificationLink(createdUser);
            var emailToSend = new EmailDetails
            {
                Subject = "Account Verification",
                HtmlBody = $"Please verify your account by clicking on the following link: {conformationLink}",
                ToEmail = createdUser.Email,
            };
            var emailResult = await _emailService.SendEmailAsync(emailToSend);
            if (!emailResult.IsSuccess)
            {
                _loggerService.LogError($"Failed to send verification link for email: {emailToSend}");
            }
            return Result.Success;
        }
    }
}
