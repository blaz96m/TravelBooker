using TravelBooker.Application.Common.Models.Request;

namespace TravelBooker.Application.User.Filters
{
    public class UserLoginFilter : BaseFilter
    {
        public string[] Emails { get; set; } = [];
    }
}
