using CatStealer.Domain.Cats;

namespace CatStealer.Application.Common.Interfaces
{
    public interface ICatStealerRepository
    {
        Task AddCatAsync(CatEntity cat);

        Task AddCats(List<CatEntity> cats);

        Task<CatEntity?> GetCatById(int id);

        Task<CatEntity?> GetCatWithTagsById(int id);
    }
}
