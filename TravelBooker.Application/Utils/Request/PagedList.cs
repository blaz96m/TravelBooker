namespace TravelBooker.Application.Utils.Request
{
    public sealed class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(IEnumerable<T> list, int count, int pageSize, int pageNumber)
        {
            MetaData = new MetaData()
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(list);
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> range, int count, int pageNumber, int pageSize)
        {
            var result = range.ToList();
            return new(result, count, pageNumber, pageSize);
        }






    }
}
