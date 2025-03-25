using CatStealer.Application.Common.Interfaces;
using CatStealer.Application.Common.Pagination;
using CatStealer.Domain.Cats;
using CatStealer.Infrastructure.Common.Persistence;
using CatStealer.Infrastructure.PaginationRepository;
using Microsoft.EntityFrameworkCore;

namespace CatStealer.Infrastructure.Cats.Persistence
{
    public class CatsRepository : EfPagedRepositoryBase<CatEntity>, ICatStealerRepository
    {
        private readonly CatStealDbContext _catStealDbContext;

        protected override DbContext Context => _catStealDbContext;

        public CatsRepository(CatStealDbContext catStealDbContext)
        {
            _catStealDbContext = catStealDbContext;
        }
        public async Task AddCatAsync(CatEntity cat)
        {
            cat.CreatedOn = DateTime.UtcNow;
            await _catStealDbContext.StolenCats.AddAsync(cat);
        }

        public async Task AddCats(List<CatEntity> cats)
        {
            await _catStealDbContext.StolenCats.AddRangeAsync(cats);
        }

        public async Task<CatEntity?> GetCatById(int id)
        {
            return await _catStealDbContext.StolenCats.FindAsync(id);
        }

        public async Task<CatEntity?> GetCatWithTagsById(int id)
        {
            return await _catStealDbContext.StolenCats
                .Include(c => c.CatTags)
                    .ThenInclude(ct => ct.Tag)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<PagedResult<CatEntity>> GetPagedCatsAsync(
          PaginationParams paginationParams,
          FilterParams filterParams)
        {
            IQueryable<CatEntity> query = _catStealDbContext.StolenCats
                .Include(c => c.CatTags)
                    .ThenInclude(ct => ct.Tag);

            // Apply filtering
            query = ApplyFiltering(query, filterParams);

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            query = query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                         .Take(paginationParams.PageSize);

            // Execute query and get items
            var items = await query.ToListAsync();

            return new PagedResult<CatEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize
            };
        }

        protected override IQueryable<CatEntity> ApplyFiltering(
           IQueryable<CatEntity> query,
           FilterParams filterParams)
        {
            if (!string.IsNullOrEmpty(filterParams.TagName))
            {
                // Filter by tag name
                query = query.Where(c => c.CatTags.Any(ct => ct.Tag.Name == filterParams.TagName));
            }

            return query;
        }
    }
}
