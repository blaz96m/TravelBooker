using TravelBooker.Application.Common.Enums;
using TravelBooker.Application.Common.Models.Response;

namespace TravelBooker.Application.Common.Utils
{
    public static class ResponseHelpers
    {

        public static Result GenerateFailedResult(ErrorType errorType, string errorMessage)
        {
            var error = GenerateErrorByType(errorType, errorMessage);
            return Result.Failure(error);
        }

        public static Result<T> GenerateFailedResult<T>(ErrorType errorType, string errorMessage)
        {
            var error = GenerateErrorByType(errorType, errorMessage);
            return Result<T>.Failure(error);
        }


        public static Error GenerateErrorByType(ErrorType errorType, string errorMessage, IDictionary<string, object?>? additionalData = null)
        {
            return errorType switch
            {
                ErrorType.NotFound => Error.NotFound(errorMessage, additionalData),
                ErrorType.Forbidden => Error.Forbidden(errorMessage, additionalData),
                ErrorType.Conflict => Error.Conflict(errorMessage, additionalData),
                ErrorType.Unauthorized => Error.Unauthorized(errorMessage, additionalData),
                ErrorType.BadRequest => Error.BadRequest(errorMessage, additionalData),
                _ => throw new Exception($"Unsupported error type provided when attempting to generate a failed Result: {errorType}")
            };
        }
    }
}
