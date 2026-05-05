using TravelBooker.Application.Common.Enums;

namespace TravelBooker.Application.Common.Models.Request
{
    public class DateFilter
    {
        public DateFilterValue FilterValue { get; init; }

        public SingleDateFilterExpressionType? SingleDateFilterExpression { get; init; }

        public DateFilterComparisonType ComparisonType { get; init; }

        private DateFilter(DateFilterValue filterValue,
            DateFilterComparisonType comparisonType,
            SingleDateFilterExpressionType? singleDateFilterExpressionType = null)
        {
            FilterValue = filterValue;
            ComparisonType = comparisonType;
            SingleDateFilterExpression = singleDateFilterExpressionType;
        }

        public static DateFilter Initialize(DateFilterValue.Range filterValue, DateFilterComparisonType comparisonType = DateFilterComparisonType.Date)
        {
            return new DateFilter(filterValue, comparisonType);
        }

        public static DateFilter Initialize(DateFilterValue.Single filterValue,
          DateFilterComparisonType comparisonType = DateFilterComparisonType.Date,
          SingleDateFilterExpressionType singleDateFilterExpressionType = SingleDateFilterExpressionType.Equal)
        {
            return new DateFilter(filterValue, comparisonType, singleDateFilterExpressionType);
        }

    }
}
