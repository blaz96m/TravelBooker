using FluentValidation;
using TravelBooker.Application.Common.Constants;
using TravelBooker.Application.User.Constants;
using TravelBooker.Application.User.Dto;

namespace TravelBooker.Application.User.Validation
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public const int MinimunPasswordLength = 6;

        public const string SpecialCharacters = "!@#$%^&*()";
        public UserLoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(ErrorMessages.FieldRequired("Email"))
                .EmailAddress()
                .WithMessage(ErrorMessages.InvalidEmailProvided);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(ErrorMessages.FieldRequired("Password"))
                .MinimumLength(MinimunPasswordLength)
                .WithMessage(UserErrorMessages.PasswordToShort)
                .Must(UpperCaseCharacterExists)
                .WithMessage(UserErrorMessages.PasswordUpercaseCharMissing)
                .Must(NumericSymbolExists)
                .WithMessage(UserErrorMessages.PasswordNumericSymbolMissing)
                .Must(SpecialCharacterExists)
                .WithMessage(UserErrorMessages.PasswordMissingSpecialChar(SpecialCharacters));
        }
        private static bool UpperCaseCharacterExists(string password) => password.Any(char.IsUpper);

        private static bool NumericSymbolExists(string password) => password.Any(char.IsDigit);

        public static bool SpecialCharacterExists(string password) => password.Any(ch => SpecialCharacters.Contains(ch));
    }
}

