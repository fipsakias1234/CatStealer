namespace CatStealer.Application.Common.Pagination
{
    public class PagedResult<T>
    {
        public IList<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }    // Total items before pagination
        public int PageNumber { get; set; }    // Current page number
        public int PageSize { get; set; }      // Size of each page
    }
}
