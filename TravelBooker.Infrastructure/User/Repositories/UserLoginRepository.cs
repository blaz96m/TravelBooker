using TravelBooker.Application.Common.Contracts.Mapping;
using TravelBooker.Domain;
using TravelBooker.Infrastructure.Common.Repositories;
using TravelBooker.Infrastructure.Context;
using TravelBooker.Infrastructure.Entities;


namespace TravelBooker.Infrastructure.User.Repositories
{
    public class UserLoginRepository(AppDbContext context, IBaseEntityMapper<UserLogin, UserLoginEntity> mapper)
        : BaseRepository<UserLoginEntity, UserLogin>(context, mapper)
    {
        protected override IQueryable<UserLoginEntity> ApplyEmbeds(string[] embeds, IQueryable<UserLoginEntity> query)
        {
            return query;
        }

        protected override IQueryable<UserLoginEntity> ApplyFilter<TFilter>(TFilter filter, IQueryable<UserLoginEntity> query)
        {
            return query;
        }
    }
}
