using CatStealer.Domain.Tags;

namespace CatStealer.Application.Common.Interfaces
{
    public interface ITagRepository
    {
        Task<TagEntity?> GetTagByIdAsync(int tagId);
        //  Task<TagEntity> GetTagByNameAsync(string tagName);

        Task AddTagsAsync(List<TagEntity> tag);

        Task<List<TagEntity>?> GetTagsByCatId(int catId);
    }
}
