using System.Reflection;
using System.Text;
using TravelBooker.Application.Common.Extensions;
using TravelBooker.Application.Common.Models.Request;
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
            var entityType = typeof(TEntity);
            var entityProperites = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sortingBuilder = new StringBuilder();
            var idx = 0;
            foreach (var orderByField in orderByFields)
            {
                var orderField = ResolvePropertyPath(entityType, orderByField)
                var orderDirection = orderByDirections[idx];
                sortingBuilder.Append($"{orderField} {orderDirection}, ");
                idx++;
            }
            return sortingBuilder.ToString().TrimEnd(',', ' ');
        }

        private static string ResolvePropertyPath(Type rootType, string fieldPath)
        {
            var pathParts = fieldPath.Split(".");
            var currentType = rootType;
            var pathBuilder = new StringBuilder();

            foreach (var part in pathParts)
            {
                var typeProperties = currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var property = typeProperties.FirstOrDefault(x => x.Name.Equals(part, StringComparison.InvariantCultureIgnoreCase));
                if (property is null)
                {
                    throw new ArgumentException(
                    $"Invalid orderByField parameter. '{part}' does not exist in '{currentType.Name}'.",
                    nameof(fieldPath));
                }
                pathBuilder.Append($"{property.Name}.");
                currentType = property.PropertyType;
            }
            return pathBuilder.ToString().TrimEnd('.');
        }

    }
}
