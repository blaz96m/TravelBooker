namespace TravelBooker.Application.User.Constants
{
    public static class UserErrorMessages
    {
        public const string UserEmailAlreadyExists = "A user with the same email address already exists.";

        public const string PasswordToShort = "Password must be at least 6 characters long";

        public const string PasswordNumericSymbolMissing = "Password must contain at least one numeric symbol";

        public const string PasswordUpercaseCharMissing = "Password must contain at least one uppercase character!";

        public static string PasswordMissingSpecialChar(string specialCharacters) => $"Password must contain at least one special character: '{specialCharacters}'";
    }
}
