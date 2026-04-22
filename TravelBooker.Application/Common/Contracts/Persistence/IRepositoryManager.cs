using Microsoft.EntityFrameworkCore.Storage;

namespace TravelBooker.Application.Common.Contracts.Infrastructure
{
    public interface IRepositoryManager
    {

        Task SaveAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();

    }
}
