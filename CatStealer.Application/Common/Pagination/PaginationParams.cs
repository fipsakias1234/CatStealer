namespace CatStealer.Application.Common.Pagination
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1; // Default to first page
        public int PageSize { get; set; } = 10;  // Default page size
    }
}
