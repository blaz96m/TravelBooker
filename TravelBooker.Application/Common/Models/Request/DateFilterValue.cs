namespace TravelBooker.Application.Common.Models.Request
{
    public abstract class DateFilterValue
    {
        private DateFilterValue() { }

        public sealed class Single : DateFilterValue
        {
            public DateTime Value { get; }
            public Single(DateTime value) => Value = value;
        }

        public sealed class Range : DateFilterValue
        {
            public DateRange Value { get; }
            public Range(DateRange value) => Value = value;
        }
    }
}
