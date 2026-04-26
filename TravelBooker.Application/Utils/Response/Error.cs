using TravelBooker.Application.Common.Enums;

namespace TravelBooker.Application.Utils.Response
{
    public record Error
    {
        public string Message { get; }

        public ErrorType Type { get; }

        public Dictionary<string, object?>? AdditionalData { get; }

        private Error(string message, ErrorType errorType, Dictionary<string, object?>? additionalData = null)
        {
            Message = message;
            Type = errorType;
            AdditionalData = additionalData;
        }

        public static readonly Error None = new(String.Empty, ErrorType.None);

        public static Error NotFound(string message, Dictionary<string, object?>? additionalData = null) => new(message, ErrorType.NotFound, additionalData);

        public static Error Unauthorized(string message, Dictionary<string, object?>? additionalData = null) => new(message, ErrorType.Unauthorized, additionalData);

        public static Error Conflict(string message, Dictionary<string, object?>? additionalData = null) => new(message, ErrorType.Conflict, additionalData);

        public static Error BadRequest(string message, Dictionary<string, object?>? additionalData = null) => new(message, ErrorType.BadRequest, additionalData);

        public static Error Forbidden(string message, Dictionary<string, object?>? additionalData = null) => new(message, ErrorType.Forbidden, additionalData);

    }
}
