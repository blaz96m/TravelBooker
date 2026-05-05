namespace TravelBooker.Application.Common.Enums
{
    public enum DateFilterComparisonType
    {
        DateTime = 0,
        Date = 1,
        Year = 2,
    }

    public enum SingleDateFilterExpressionType
    {
        Equal = 0,
        Before = 1,
        BeforeOrEqual = 2,
        After = 3,
        AfterOrEqual = 4,
    }
}
