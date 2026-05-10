namespace TravelBooker.Application.Common.Contracts.Persistence
{
    public interface IBaseEntity
    {
        long Id { get; set; }

        DateTime DateCreated { get; set; }

        DateTime DateUpdated { get; set; }
    }
}
