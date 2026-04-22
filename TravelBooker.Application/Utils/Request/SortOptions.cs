using TravelBooker.Application.Common.Constants;

namespace TravelBooker.Application.Utils.Request
{
    public class SortOptions
    {
        public string[] OrderByFields { get; } = [];

        public string[] OrderByDirections { get; } = [];

        public SortOptions(string? orderByFields, string? orderByDirections)
        {
            if (!String.IsNullOrWhiteSpace(orderByFields))
            {
                OrderByFields = orderByFields!.Split(",");
                if (String.IsNullOrWhiteSpace(orderByDirections))
                {
                    OrderByDirections = OrderByFields.Select(_ => UtilConstants.SortOrderAscending).ToArray();
                }
                else
                {
                    OrderByDirections = orderByDirections!.Split(",");
                    ValidateOrderByDirections(OrderByDirections, OrderByFields);
                }
            }
            else if (String.IsNullOrWhiteSpace(orderByFields) && !String.IsNullOrWhiteSpace(orderByDirections))
            {
                throw new ArgumentException(ErrorMessages.SortingFieldsMissing);
            }
        }

        public static void ValidateOrderByDirections(string[] orderByDirections, string[] orderByFields)
        {
            if (orderByDirections.Length != orderByFields.Length)
            {
                throw new ArgumentException(ErrorMessages.SortingFieldsMissmatch);
            }
            var invalidDirections = orderByDirections.Where(
                dir => dir != UtilConstants.SortOrderAscending || dir != UtilConstants.SortOrderDescending
                );
            if (invalidDirections.Any())
            {
                throw new ArgumentException($"Invalid orderByDirection key provided. The value ${invalidDirections.First()} is not valid");
            }
        }
    }
}
