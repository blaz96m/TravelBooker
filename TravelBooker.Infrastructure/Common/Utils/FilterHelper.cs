using System.Linq.Expressions;
using TravelBooker.Application.Common.Contracts.Persistence;
using TravelBooker.Application.Common.Contracts.Request;
using TravelBooker.Application.Common.Enums;
using TravelBooker.Application.Common.Extensions;
using TravelBooker.Application.Common.Models.Request;

namespace TravelBooker.Infrastructure.Utils
{
    public static class FilterHelper<TEntity> where TEntity : IBaseEntity
    {

        public static IQueryable<TEntity> ApplyBaseFilter<TFilter>(IQueryable<TEntity> query, TFilter filter)
            where TFilter : IBaseFilter
        {
            if (!filter.Ids.IsEmpty())
            {
                query = query.Where(x => filter.Ids.Contains(x.Id));
            }
            if (filter.DateCreatedFilter != null)
            {
                query = ApplyDateFilter(filter.DateCreatedFilter, query, x => x.DateCreated);
            }
            if (filter.DateUpdatedFilter != null)
            {
                query = ApplyDateFilter(filter.DateUpdatedFilter, query, x => x.DateUpdated);
            }
            return query;

        }

        public static IQueryable<TEntity> ApplyDateFilter(DateFilter dateFilter, IQueryable<TEntity> query, Expression<Func<TEntity, DateTime>> dateSelector)
        {
            var (min, max) = GenerateDateFilterRange(dateFilter);
            var parameter = dateSelector.Parameters[0];
            var sourceDateExpression = dateSelector.Body;
            var minConstant = Expression.Constant(min);
            var compareStart = Expression.GreaterThanOrEqual(sourceDateExpression, minConstant);
            Expression<Func<TEntity, bool>> lambda;

            if (max is not null)
            {
                var endConstant = Expression.Constant(max);
                var compareEnd = Expression.LessThan(sourceDateExpression, endConstant);
                var finalExpression = Expression.AndAlso(compareStart, compareEnd);
                lambda = Expression.Lambda<Func<TEntity, bool>>(finalExpression, parameter);
                return query.Where(lambda);
            }
            lambda = Expression.Lambda<Func<TEntity, bool>>(compareStart, parameter);
            return query.Where(lambda);

        }

        private static (DateTime min, DateTime? max) GenerateDateFilterRange(DateFilter dateFilter)

        {
            var filterValue = dateFilter.FilterValue;
            var comparisonType = dateFilter.ComparisonType;
            var singleDateFilterExpression = dateFilter.SingleDateFilterExpression;
            var (min, max) = GetMinAndMaxFromDateFilterValue(filterValue);
            SetMinMaxBasedOnDateFilterType(ref min, ref max, comparisonType);

            if (max is not null)
            {
                max = IncreaseDateTimeUnitByFilterType(max.Value, comparisonType);
            }
            else
            {
                SetMinMaxBasedOnSingleDateFilterExpressionType(ref min, ref max, comparisonType, singleDateFilterExpression);
            }
            return (min, max);
        }

        private static void SetMinMaxBasedOnSingleDateFilterExpressionType(ref DateTime min,
            ref DateTime? max,
            DateFilterComparisonType comparisonType,
            SingleDateFilterExpressionType? singleDateFilterExpression)
        {
            switch (singleDateFilterExpression)
            {
                case SingleDateFilterExpressionType.Equal:
                    max = IncreaseDateTimeUnitByFilterType(min, comparisonType);
                    break;
                case SingleDateFilterExpressionType.After:
                    max = null;
                    min = IncreaseDateTimeUnitByFilterType(min, comparisonType);
                    break;
                case SingleDateFilterExpressionType.AfterOrEqual:
                    max = null;
                    break;
                case SingleDateFilterExpressionType.Before:
                    max = min;
                    min = DateTime.MinValue;
                    break;
                case SingleDateFilterExpressionType.BeforeOrEqual:
                    max = IncreaseDateTimeUnitByFilterType(min, comparisonType);
                    min = DateTime.MinValue;
                    break;
            }
        }

        private static (DateTime min, DateTime? max) GetMinAndMaxFromDateFilterValue(DateFilterValue filterValue)
            => filterValue switch
            {
                DateFilterValue.Range filter => (filter.Value.Min, filter.Value.Max),
                DateFilterValue.Single filter => (filter.Value, null),
                _ => throw new InvalidOperationException("Unsupported Type!")
            };

        private static void SetMinMaxBasedOnDateFilterType(ref DateTime min, ref DateTime? max, DateFilterComparisonType comparisonType)
        {
            switch (comparisonType)
            {
                case DateFilterComparisonType.Date:
                    min = min.Date;
                    max = max.HasValue ? max.Value.Date : null;
                    break;

                case DateFilterComparisonType.Year:
                    min = new DateTime(min.Year, 1, 1);
                    max = max.HasValue ? new DateTime(max.Value.Year, 1, 1) : null;
                    break;

                case DateFilterComparisonType.DateTime:
                    min = new DateTime(min.Year, min.Month, min.Day, min.Hour, min.Minute, 0);
                    max = max.HasValue ?
                        new DateTime(max.Value.Year, max.Value.Month, max.Value.Day, max.Value.Hour, max.Value.Minute, 0)
                        : null;
                    break;
            }
        }

        private static DateTime IncreaseDateTimeUnitByFilterType(DateTime dateValue, DateFilterComparisonType filterType) => filterType switch
        {
            DateFilterComparisonType.DateTime => dateValue.AddMinutes(1),
            DateFilterComparisonType.Date => dateValue.AddDays(1),
            DateFilterComparisonType.Year => dateValue.AddYears(1),
            _ => throw new NotImplementedException()
        };

    }
}
