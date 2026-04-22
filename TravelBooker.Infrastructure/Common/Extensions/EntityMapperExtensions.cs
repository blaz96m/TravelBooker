using TravelBooker.Application.Common.Contracts.Mapping;
using TravelBooker.Infrastructure.Common.Extensions;

namespace TravelBooker.Infrastructure.Common.Extensions
{
    public static class EntityMapperExtensions
    {
        public static IEnumerable<TDomain> ToDomain<TDomain, TEntity>(
            this IBaseEntityMapper<TDomain, TEntity> mapper,
            IEnumerable<TEntity> entityModels) => entityModels.Select(mapper.ToDomain);

        public static IEnumerable<TEntity> ToEntity<TDomain, TEntity>(
            this IBaseEntityMapper<TDomain, TEntity> mapper,
            IEnumerable<TDomain> domainModels) => domainModels.Select(mapper.ToEntity);
    }
}
