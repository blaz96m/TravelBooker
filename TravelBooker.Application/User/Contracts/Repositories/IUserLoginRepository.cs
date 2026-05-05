using TravelBooker.Application.Common.Contracts.Infrastructure;
using TravelBooker.Domain;

namespace TravelBooker.Application.User.Contracts.Repositories
{
    public interface IUserLoginRepository : IBaseRepository<UserLogin>
    {
        Task<bool> UserLoginDataExistsAsync(string email);
    }
}
