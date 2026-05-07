using Microsoft.EntityFrameworkCore.Storage;
using TravelBooker.Application.Common.Models.Request;

namespace TravelBooker.Application.Common.Contracts.Persistence
{
    public interface IBaseRepository<TDomain>
    {
        Task<IEnumerable<TDomain>> GetAllAsnyc(RequestModel requestModel, CancellationToken cancellationToken = default);

        Task<TDomain?> GetSingleAsync(int id, bool track = false, CancellationToken cancellationToken = default);

        Task CreateAsync(TDomain domainModel);

        void Update(TDomain domainModel);

        void Delete(TDomain domainModel);

        Task SaveAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
