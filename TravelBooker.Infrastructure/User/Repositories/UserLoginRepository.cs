using TravelBooker.Application.Common.Contracts.Mapping;
using TravelBooker.Application.User.Contracts.Repositories;
using TravelBooker.Application.User.Filters;
using TravelBooker.Domain;
using TravelBooker.Infrastructure.Common.Repositories;
using TravelBooker.Infrastructure.Context;
using TravelBooker.Infrastructure.Entities;


namespace TravelBooker.Infrastructure.User.Repositories
{
    public class UserLoginRepository(AppDbContext context, IBaseEntityMapper<UserLogin, UserLoginEntity> mapper)
        : BaseRepository<UserLoginEntity, UserLogin, UserLoginFilter>(context, mapper), IUserLoginRepository
    {
        public Task<bool> UserLoginDataExistsAsync(string email)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<UserLoginEntity> ApplyEmbeds(string[] embeds, IQueryable<UserLoginEntity> query)
        {
            return query;
        }

        protected override IQueryable<UserLoginEntity> ApplyFilter(UserLoginFilter filter, IQueryable<UserLoginEntity> query)
        {
            if (filter.Emails.Any())
            {
                query = query.Where(x => filter.Emails.Contains(x.Email));
            }

            return query;
        }
    }
}
