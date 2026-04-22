namespace TravelBooker.Application.Common.Contracts.Mapping
{
    public interface IBaseEntityMapper<TDomain, TEntity>
    {
        TDomain ToDomain(TEntity entity);
        TEntity ToEntity(TDomain domainModel);
    }
}
