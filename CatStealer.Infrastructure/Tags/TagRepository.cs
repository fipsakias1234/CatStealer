using CatStealer.Application.Common.Interfaces;
using CatStealer.Domain.Tags;
using CatStealer.Infrastructure.Common.Persistence;

namespace CatStealer.Infrastructure.Tags
{
    public class TagRepository : ITagRepository
    {
        private readonly CatStealDbContext _catStealDbContext;

        public TagRepository(CatStealDbContext catStealDbContext)
        {
            _catStealDbContext = catStealDbContext;

        }

        public async Task AddTagsAsync(List<TagEntity> tag)
        {
            await _catStealDbContext.AddRangeAsync(tag);
            await _catStealDbContext.SaveChangesAsync();
        }

        public async Task<TagEntity?> GetTagByIdAsync(int tagId)
        {
            return await _catStealDbContext.Tags.FindAsync(tagId);
        }

        public async Task<List<TagEntity>?> GetTagsByCatId(int catId)
        {
            throw new NotImplementedException();
        }

    }
}
