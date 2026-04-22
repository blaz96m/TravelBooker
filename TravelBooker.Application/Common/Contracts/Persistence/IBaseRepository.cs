using TravelBooker.Application.Utils.Request;

namespace TravelBooker.Application.Common.Contracts.Infrastructure
{
    public interface IBaseRepository<TDomain>
    {
        Task<IEnumerable<TDomain>> GetAllAsnyc(RequestModel requestModel, CancellationToken cancellationToken = default);

        Task<TDomain?> GetSingleAsync(int id, bool track = false, CancellationToken cancellationToken = default);

        Task Create(TDomain domainModel);

        void Update(TDomain domainModel);

        void Delete(TDomain domainModel);

    }
}
