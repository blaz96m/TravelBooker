using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TravelBooker.Application.Common.Contracts.Infrastructure;
using TravelBooker.Application.Common.Contracts.Mapping;
using TravelBooker.Application.Utils.Request;
using TravelBooker.Infrastructure.Common.Extensions;
using TravelBooker.Infrastructure.Common.Models;
using TravelBooker.Infrastructure.Common.Utils;
using TravelBooker.Infrastructure.Context;

namespace TravelBooker.Infrastructure.Common.Repositories
{
    public abstract class BaseRepository<TEntity, TDomain>(AppDbContext context, IBaseEntityMapper<TDomain, TEntity> mapper) : IBaseRepository<TDomain>
        where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context = context;
        protected readonly IBaseEntityMapper<TDomain, TEntity> _entityMapper = mapper;

        protected abstract IQueryable<TEntity> ApplyFilter<TFilter>(TFilter filter, IQueryable<TEntity> query);

        protected abstract IQueryable<TEntity> ApplyEmbeds(string[] embeds, IQueryable<TEntity> query);

        public virtual IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, PaginationOptions paginationOptions)
            => query.Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize).Take(paginationOptions.PageSize);

        protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, SortOptions sortModel)
        {
            var sortingQuery = SortingBuilder.CreateSortingQuery<TEntity>(sortModel);
            if (!String.IsNullOrWhiteSpace(sortingQuery))
            {
                query = query.OrderBy(sortingQuery);
            }
            return query;
        }

        protected virtual IQueryable<TEntity> BuildQuery<FilterT>(IRequestModel<FilterT> requestModel)
            where FilterT : IBaseFilter
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            query = ApplyFilter(requestModel.Filter, query);

            var embeds = requestModel.Embeds;
            if (!String.IsNullOrWhiteSpace(embeds))
            {
                var embedList = embeds.Split(",");
                query = ApplyEmbeds(embedList, query);
            }

            var sortOptions = requestModel.SortOptions;
            if (sortOptions is not null)
            {
                query = ApplySorting(query, sortOptions);
            }

            return query;
        }

        protected virtual IQueryable<TEntity> BuildQuery(RequestModel requestModel)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            var embeds = requestModel.Embeds;
            if (!String.IsNullOrWhiteSpace(embeds))
            {
                var embedList = embeds.Split(",");
                query = ApplyEmbeds(embedList, query);
            }
            var sortOptions = requestModel.SortOptions;
            if (sortOptions is not null)
            {
                query = ApplySorting(query, sortOptions);
            }
            if (!requestModel.Tracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        protected virtual IQueryable<TEntity> BuildQuery(int id, bool track)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            query = query.Where(x => x.Id == id);

            if (!track)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        protected virtual void InitializeEntity(TEntity entity)
        {
            entity.DateCreated = DateTime.UtcNow;
            entity.DateUpdated = DateTime.UtcNow;
        }

        protected async Task<List<TDomain>> GetResultsAsync(
        IQueryable<TEntity> query,
        PaginationOptions? paginationModel,
        CancellationToken cancellationToken = default)
        {
            if (paginationModel is not null)
            {
                query = ApplyPagination(query, paginationModel);
            }

            var entities = await query.ToListAsync(cancellationToken);

            return _entityMapper.ToDomain(entities).ToList();
        }

        protected async Task<TDomain?> GetResultAsync(
        IQueryable<TEntity> query,
        CancellationToken cancellationToken = default)
        {
            var entity = await query.SingleOrDefaultAsync(cancellationToken);
            return entity is null ? default : _entityMapper.ToDomain(entity);
        }
        protected async Task<PagedList<TDomain>> MaterializeToPagedListAsync(IQueryable<TEntity> query, PaginationOptions paginationModel, CancellationToken cancellationToken = default)
        {
            var count = await query.CountAsync(cancellationToken);
            var result = await GetResultsAsync(query, paginationModel, cancellationToken);
            return PagedList<TDomain>.ToPagedList(result, count, paginationModel.PageNumber, paginationModel.PageSize);
        }


        public async Task Create(TDomain domainModel)
        {
            var entity = _entityMapper.ToEntity(domainModel);
            InitializeEntity(entity);
            await _context.AddAsync(entity);
        }

        public void Delete(TDomain domainModel)
        {
            var entity = _entityMapper.ToEntity(domainModel);
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TDomain>> GetAllAsnyc(RequestModel requestModel, CancellationToken cancellationToken = default)
        {
            var query = BuildQuery(requestModel);
            return await GetResultsAsync(query, null, cancellationToken);
        }

        public async Task<TDomain?> GetSingleAsync(int id, bool track, CancellationToken cancellationToken = default)
        {
            var query = BuildQuery(id, track);
            return await GetResultAsync(query, cancellationToken);
        }

        public void Update(TDomain domainModel)
        {
            var entity = _entityMapper.ToEntity(domainModel);
            _context.Update(entity);
        }
    }
}

