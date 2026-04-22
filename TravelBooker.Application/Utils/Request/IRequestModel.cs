namespace TravelBooker.Application.Utils.Request
{
    public interface IRequestModel
    {
        string? Embeds { get; set; }
        bool Tracking { get; set; }
        SortOptions? SortOptions { get; set; }
    }

    public interface IRequestModel<TFilter> : IRequestModel
        where TFilter : IBaseFilter
    {
        TFilter Filter { get; set; }
    }

}
