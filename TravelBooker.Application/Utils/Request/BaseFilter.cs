namespace TravelBooker.Application.Utils.Request
{
    public class BaseFilter : IBaseFilter
    {
        public int[] Ids { get; set; } = [];

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }
    }
}
