using CatStealer.Application.Common.Interfaces;
using CatStealer.Domain.Tags;
using CatStealer.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CatStealer.Infrastructure.Tags
{
    public class TagRepository : ITagRepository
    {
        private readonly CatStealDbContext _catStealDbContext;

        public TagRepository(CatStealDbContext catStealDbContext)
        {
            _catStealDbContext = catStealDbContext;

        }

        public async Task AddTagAsync(TagEntity tag)
        {
            tag.CreatedOn = DateTime.UtcNow;
            await _catStealDbContext.Tags.AddAsync(tag);
        }

        public async Task AddTagsAsync(List<TagEntity> tags)
        {
            await _catStealDbContext.Tags.AddRangeAsync(tags);
        }

        public async Task<TagEntity?> GetTagByIdAsync(int tagId)
        {
            return await _catStealDbContext.Tags.FindAsync(tagId);
        }

        public async Task<TagEntity?> GetTagByNameAsync(string tagName)
        {
            return await _catStealDbContext.Tags
                .FirstOrDefaultAsync(t => t.Name == tagName);
        }

        public async Task<List<TagEntity>?> GetTagsByCatId(int catId)
        {
            var tags = await _catStealDbContext.CatTags
                .Where(ct => ct.CatsId == catId)
                .Select(ct => ct.Tag)
                .ToListAsync();

            return tags.Any() ? tags : null;
        }
    }
}
