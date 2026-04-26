using Riok.Mapperly.Abstractions;
using TravelBooker.Application.Common.Contracts.Mapping;
using TravelBooker.Domain;
using TravelBooker.Infrastructure.Entities;

namespace TravelBooker.Infrastructure.User.Mapping
{
    [Mapper]
    public partial class UserLoginEntityMapper : IBaseEntityMapper<UserLogin, UserLoginEntity>
    {
        public partial UserLogin ToDomain(UserLoginEntity entity);

        public partial UserLoginEntity ToEntity(UserLogin domainModel);
    }
}
