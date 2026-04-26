using System;
using System.Collections.Generic;

namespace TravelBooker.Infrastructure.Entities;

public partial class UserLoginEntity
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public bool IsVerified { get; set; }

    public bool IsDeactivationNotificationSent { get; set; }

    public int Id { get; set; }
}
