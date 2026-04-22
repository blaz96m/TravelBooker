namespace TravelBooker.Domain
{
    public class UserLogin
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public bool IsVerified { get; set; }

        public bool IsDeactivationNotificationSent { get; set; }
    }
}
