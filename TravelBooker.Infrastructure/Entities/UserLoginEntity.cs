using TravelBooker.Infrastructure.Common.Models;

namespace TravelBooker.Infrastructure.Entities;

public partial class UserLoginEntity : BaseEntity
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsVerified { get; set; }

    public bool IsDeactivationNotificationSent { get; set; }
}
