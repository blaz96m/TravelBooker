using TravelBooker.Application.Common.Contracts.Persistence;

namespace TravelBooker.Infrastructure.Common.Models
{
    public abstract class BaseEntity : IBaseEntity
    {
        public long Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
