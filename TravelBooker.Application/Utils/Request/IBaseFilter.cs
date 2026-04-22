namespace TravelBooker.Application.Utils.Request
{
    public interface IBaseFilter
    {
        int[] Ids { get; set; }

        DateTime? DateCreated { get; set; }

        DateTime? DateUpdated { get; set; }
    }
}
