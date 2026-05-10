using TravelBooker.Application.Common.Enums;

namespace TravelBooker.Application.Common.Models.Response
{
    public record Error
    {
        public string Message { get; }

        public ErrorType Type { get; }

        public IDictionary<string, object?>? AdditionalData { get; }

        private Error(string message, ErrorType errorType, IDictionary<string, object?>? additionalData = null)
        {
            Message = message;
            Type = errorType;
            AdditionalData = additionalData;
        }

        public static readonly Error None = new(String.Empty, ErrorType.None);

        public static Error NotFound(string message, IDictionary<string, object?>? additionalData = null) => new(message, ErrorType.NotFound, additionalData);

        public static Error Unauthorized(string message, IDictionary<string, object?>? additionalData = null) => new(message, ErrorType.Unauthorized, additionalData);

        public static Error Conflict(string message, IDictionary<string, object?>? additionalData = null) => new(message, ErrorType.Conflict, additionalData);

        public static Error BadRequest(string message, IDictionary<string, object?>? additionalData = null) => new(message, ErrorType.BadRequest, additionalData);

        public static Error Forbidden(string message, IDictionary<string, object?>? additionalData = null) => new(message, ErrorType.Forbidden, additionalData);

        public static Error InternalError(string message, IDictionary<string, object?>? additionalData = null) => new(message, ErrorType.InternalError, additionalData);

    }
}
