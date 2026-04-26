using Microsoft.AspNetCore.Mvc;
using TravelBooker.Api.Common;
using TravelBooker.Application.Common.Enums;
using TravelBooker.Application.Utils.Response;

namespace TravelBooker.Api.Controllers
{

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
                _ => throw new NotImplementedException($"The provided {nameof(ErrorType)}: {error.Type} does not match any object result")
            }; ;
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
