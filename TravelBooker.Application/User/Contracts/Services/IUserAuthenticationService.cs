using TravelBooker.Application.Common.Models.Response;
using TravelBooker.Application.User.Dto;

namespace TravelBooker.Application.User.Contracts.Services
{
    public interface IUserAuthenticationService
    {
        public Task<Result> RegisterUser(UserLoginDto userLoginDto);
    }
}
