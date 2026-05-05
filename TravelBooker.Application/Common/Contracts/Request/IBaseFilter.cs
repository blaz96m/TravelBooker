using TravelBooker.Application.Common.Models.Request;

namespace TravelBooker.Application.Common.Contracts.Request
{
    public interface IBaseFilter
    {
        int[] Ids { get; set; }

        DateFilter? DateCreatedFilter { get; set; }

        DateFilter? DateUpdatedFilter { get; set; }

    }
}
