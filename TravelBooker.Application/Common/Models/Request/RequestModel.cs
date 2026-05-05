using TravelBooker.Application.Common.Contracts.Request;

namespace TravelBooker.Application.Common.Models.Request
{
    public class RequestModel : IRequestModel
    {
        public string? Embeds { get; set; }
        public bool Tracking { get; set; } = false;
        public SortOptions? SortOptions { get; set; }
    }

    public class RequestModel<TFilter> : RequestModel, IRequestModel<TFilter> where TFilter : IBaseFilter
    {
        public TFilter Filter { get; set; }
    }
}
