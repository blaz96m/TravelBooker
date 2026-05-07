using Microsoft.EntityFrameworkCore;
using TravelBooker.Application.Common.Contracts.Mapping;
using TravelBooker.Application.Common.Extensions;
using TravelBooker.Application.User.Contracts.Repositories;
using TravelBooker.Application.User.Filters;
using TravelBooker.Domain;
using TravelBooker.Infrastructure.Common.Repositories;
using TravelBooker.Infrastructure.Context;
using TravelBooker.Infrastructure.Entities;


namespace TravelBooker.Infrastructure.User.Repositories
{
    public sealed class UserLoginRepository(AppDbContext context, IBaseEntityMapper<UserLogin, UserLoginEntity> mapper)
        : BaseRepository<UserLoginEntity, UserLogin, UserLoginFilter>(context, mapper), IUserLoginRepository
    {
        public async Task<bool> UserLoginDataExistsAsync(string email)
        {
            var dbSet = _context.Set<UserLogin>();
            return await dbSet.AnyAsync(x => x.Email == email);
        }

        protected override IQueryable<UserLoginEntity> ApplyEmbeds(string[] embeds, IQueryable<UserLoginEntity> query)
        {
            return query;
        }

        protected override IQueryable<UserLoginEntity> ApplyFilter(UserLoginFilter filter, IQueryable<UserLoginEntity> query)
        {
            base.ApplyFilter(filter, query);
            if (!filter.Emails.IsEmpty())
            {
                query = query.Where(x => filter.Emails.Contains(x.Email));
            }

            return query;
        }
    }
}
