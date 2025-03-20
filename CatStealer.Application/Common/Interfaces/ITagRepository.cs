using CatStealer.Domain.Tags;

namespace CatStealer.Application.Common.Interfaces
{
    public interface ITagRepository
    {
        Task AddTagAsync(TagEntity tag);
        Task AddTagsAsync(List<TagEntity> tags);
        Task<TagEntity?> GetTagByIdAsync(int tagId);
        Task<TagEntity?> GetTagByNameAsync(string tagName);
        Task<List<TagEntity>?> GetTagsByCatId(int catId);
    }
}
