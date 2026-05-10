using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using TravelBooker.Application.Common.Constants;
using TravelBooker.Application.Common.Enums;
using TravelBooker.Application.Common.Models.Response;
using TravelBooker.Application.Common.Utils;

namespace TravelBooker.Api.Common.Controllers
{
    [Route("api")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult ProcessError(Error error)
        {
            return error.Type switch
            {
                ErrorType.NotFound => NotFound(SetProblemDetails(error, StatusCodes.Status404NotFound, Constants.RfcNotFoundURI)),
                ErrorType.BadRequest => BadRequest(SetProblemDetails(error, StatusCodes.Status400BadRequest, Constants.RfcBadRequestURI)),
                ErrorType.Conflict => Conflict(SetProblemDetails(error, StatusCodes.Status409Conflict, Constants.RfcConflictURI)),
                ErrorType.Unauthorized => Unauthorized(SetProblemDetails(error, StatusCodes.Status401Unauthorized, Constants.RfcUnauthorizedURI)),
                ErrorType.Forbidden => new ObjectResult(SetProblemDetails(error, StatusCodes.Status403Forbidden, Constants.RfcForbiddenURI))
                {
                    StatusCode = StatusCodes.Status403Forbidden
                },
                ErrorType.InternalError => new ObjectResult(SetProblemDetails(error, StatusCodes.Status500InternalServerError, Constants.RfcForbiddenURI))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                },
                _ => throw new NotImplementedException($"The provided {nameof(ErrorType)}: {error.Type} does not match any object result")
            }; ;
        }

        protected IActionResult ProcessInvalidPayload(ValidationResult validationResult, ErrorType errorType = ErrorType.BadRequest)
        {
            IDictionary<string, object?>? errorAdditionalData = null;
            if (validationResult.Errors.Any())
            {
                errorAdditionalData = validationResult
                      .ToDictionary()
                      .ToDictionary(r => r.Key, r => (object?)r.Value);
            }
            var error = ResponseHelpers.GenerateErrorByType(errorType, ErrorMessages.ModelValidationError, errorAdditionalData);
            return ProcessError(error);
        }

        private static ProblemDetails SetProblemDetails(Error error, int status, string type)
        {
            var title = "An error occured";
            var problemDetails = new ProblemDetails()
            {
                Title = title,
                Detail = error.Message,
            };
            if (error.AdditionalData is not null)
            {
                problemDetails.Extensions = error.AdditionalData;
            }
            problemDetails.Status = status;
            problemDetails.Type = type;
            return problemDetails;
        }

    }
}
