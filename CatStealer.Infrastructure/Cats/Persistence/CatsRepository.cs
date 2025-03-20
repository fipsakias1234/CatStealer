using CatStealer.Application.Common.Interfaces;
using CatStealer.Domain.Cats;
using CatStealer.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CatStealer.Infrastructure.Cats.Persistence
{
    public class CatsRepository : ICatStealerRepository
    {
        private readonly CatStealDbContext _catStealDbContext;

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
    }
}
