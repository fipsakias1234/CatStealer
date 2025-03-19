using CatStealer.Domain.Cats;

namespace CatStealer.Application.Common.Interfaces
{
    public interface ICatStealerRepository
    {
        Task AddCats(List<CatEntity> cats);

        Task<CatEntity?> GetCatByIid(int id);
    }
}
