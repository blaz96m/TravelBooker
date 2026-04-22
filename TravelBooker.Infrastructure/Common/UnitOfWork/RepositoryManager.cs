using Microsoft.EntityFrameworkCore.Storage;
using TravelBooker.Application.Common.Contracts.Infrastructure;
using TravelBooker.Infrastructure.Context;

namespace TravelBooker.Infrastructure.Common.UnitOfWork
{
    public class RepositoryManager : IRepositoryManager
    {

        private readonly AppDbContext _context;

        public RepositoryManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
