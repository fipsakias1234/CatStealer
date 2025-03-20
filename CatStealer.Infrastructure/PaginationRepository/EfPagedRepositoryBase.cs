using CatStealer.Application.Common.Interfaces;
using CatStealer.Application.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CatStealer.Infrastructure.PaginationRepository
{
    public abstract class EfPagedRepositoryBase<T> : IPagedRepository<T> where T : class
    {
        protected abstract DbContext Context { get; }

        public virtual async Task<PagedResult<T>> GetPagedAsync(
            PaginationParams paginationParams,
            FilterParams filterParams)
        {
            IQueryable<T> query = Context.Set<T>();

            // Apply filtering
            query = ApplyFiltering(query, filterParams);

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            query = query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                         .Take(paginationParams.PageSize);

            var items = await query.ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize
            };
        }

        protected virtual IQueryable<T> ApplyFiltering(
            IQueryable<T> query,
            FilterParams filterParams)
        {
            // Default implementation (no filtering)
            return query;
        }
    }
}
