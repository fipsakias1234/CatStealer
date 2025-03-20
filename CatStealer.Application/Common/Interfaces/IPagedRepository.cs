using CatStealer.Application.Common.Pagination;

namespace CatStealer.Application.Common.Interfaces
{
    public interface IPagedRepository<T> where T : class
    {
        Task<PagedResult<T>> GetPagedAsync(
            PaginationParams paginationParams,
            FilterParams filterParams);
    }
}
