using TravelBooker.Application.Common.Contracts.Request;

namespace TravelBooker.Application.Common.Models.Request
{
    public class BaseFilter : IBaseFilter
    {
        public int[] Ids { get; set; } = [];

        public DateFilter? DateCreatedFilter { get; set; }

        public DateFilter? DateUpdatedFilter { get; set; }
    }
}
