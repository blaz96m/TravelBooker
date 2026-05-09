namespace TravelBooker.Application.Common.Models
{
    public class EmailDetails
    {
        public string ToEmail { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string HtmlBody { get; set; } = string.Empty;
    }
}
