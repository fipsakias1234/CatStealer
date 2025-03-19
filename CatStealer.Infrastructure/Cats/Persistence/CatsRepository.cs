using CatStealer.Application.Common.Interfaces;
using CatStealer.Domain.Cats;
using CatStealer.Infrastructure.Common.Persistence;

namespace CatStealer.Infrastructure.Cats.Persistence
{
    public class CatsRepository : ICatStealerRepository
    {
        private readonly CatStealDbContext _catStealDbContext;

        public CatsRepository(CatStealDbContext catStealDbContext)
        {
            _catStealDbContext = catStealDbContext;
        }
        public async Task AddCats(List<CatEntity> cats)
        {
            cats.ForEach(cat =>
            {
                cat.CreatedOn = DateTime.UtcNow;
            });

            await _catStealDbContext.AddRangeAsync(cats);

            await _catStealDbContext.SaveChangesAsync();
        }

        public async Task<CatEntity?> GetCatByIid(int id)
        {
            return await _catStealDbContext.StolenCats.FindAsync(id);
        }
    }
}
