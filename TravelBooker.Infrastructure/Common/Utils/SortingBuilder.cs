using System.Reflection;
using System.Text;
using TravelBooker.Application.Common.Extensions;
using TravelBooker.Application.Utils.Request;
using TravelBooker.Infrastructure.Common.Models;

namespace TravelBooker.Infrastructure.Common.Utils
{
    public static class SortingBuilder
    {
        public static string CreateSortingQuery<TEntity>(SortOptions sortModel)
            where TEntity : BaseEntity
        {
            var orderByFields = sortModel.OrderByFields;
            var orderByDirections = sortModel.OrderByDirections;
            if (orderByFields.IsEmpty() || orderByDirections.IsEmpty())
            {
                return String.Empty;
            }
            var entityProperites = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sortingBuilder = new StringBuilder();
            var idx = 0;
            foreach (var orderByField in orderByFields)
            {
                var entityField = entityProperites.FirstOrDefault(
                    x => x.Name.Equals(orderByField, StringComparison.InvariantCultureIgnoreCase));
                if (entityField is null)
                {
                    throw new ArgumentException(
                        $"Invalid orderByField parameter. Field {orderByField} does not exist in {typeof(TEntity).Name}"
                        , nameof(orderByField)
                        );
                }
                var orderDirection = orderByDirections[idx];
                sortingBuilder.Append($"{entityField.Name.ToString()} {orderDirection}, ");
                idx++;
            }
            return sortingBuilder.ToString().TrimEnd(',', ' ');
        }

    }
}
