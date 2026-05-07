using Riok.Mapperly.Abstractions;
using TravelBooker.Application.User.Dto;
using TravelBooker.Domain;

namespace TravelBooker.Application.User.Mapping
{
    [Mapper]
    public partial class UserLoginDomainMapper()
    {
        [MapperIgnoreTarget(nameof(UserLogin.Id))]
        [MapperIgnoreTarget(nameof(UserLogin.DateCreated))]
        [MapperIgnoreTarget(nameof(UserLogin.DateUpdated))]
        [MapperIgnoreTarget(nameof(UserLogin.IsVerified))]
        [MapperIgnoreTarget(nameof(UserLogin.IsDeactivationNotificationSent))]
        public partial UserLogin ToDomain(UserLoginDto userLoginCreateDto);

    }

}
