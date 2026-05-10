using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TravelBooker.Api.Common.Controllers;
using TravelBooker.Application.User.Contracts.Services;
using TravelBooker.Application.User.Dto;

namespace TravelBooker.Api.User.Controllers
{
    [EnableRateLimiting("SensitiveEndpoint")]
    public class UserAuthenticationController : BaseController
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly IValidator<UserLoginDto> _userLoginValidator;

        public UserAuthenticationController(IUserAuthenticationService userAuhenticationService, IValidator<UserLoginDto> userLoginValidator)
        {
            _userAuthenticationService = userAuhenticationService;
            _userLoginValidator = userLoginValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserLoginDto registerRequest)
        {
            var validationResult = _userLoginValidator.Validate(registerRequest);
            if (!validationResult.IsValid)
            {
                return ProcessInvalidPayload(validationResult);
            }
            var result = await _userAuthenticationService.RegisterUser(registerRequest);
            return result.Resolve(() => Ok(), ProcessError);
        }
    }
}
