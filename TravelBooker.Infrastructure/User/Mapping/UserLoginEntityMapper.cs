using Riok.Mapperly.Abstractions;
using TravelBooker.Application.Common.Contracts.Mapping;
using TravelBooker.Infrastructure.Entities;
using DomainUserLogin = TravelBooker.Domain.UserLogin;

namespace TravelBooker.Infrastructure.User.Mapping
{
    [Mapper]
    public partial class UserLoginEntityMapper : IBaseEntityMapper<DomainUserLogin, UserLogin>
    {
        public partial DomainUserLogin ToDomain(UserLogin entity);

        public partial UserLogin ToEntity(DomainUserLogin domainModel);
    }
}
