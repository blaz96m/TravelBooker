namespace TravelBooker.Application.Common.Constants
{
    public static class ErrorMessages
    {
        public const string SortingFieldsMissing = "Invalid sorting parameters! The orderByFields have not been provided";

        public const string SortingFieldsMissmatch = "Invalid sorting parameters! The orderByFields and orderByDirections lengths do not match.";
        public const string InvalidEmailProvided = "Invalid Email provided";
        public const string ModelValidationError = "One or more validation errors occurred.";
        public static string FieldRequired(string propetyName) => $"{propetyName} is required";



    }
}
