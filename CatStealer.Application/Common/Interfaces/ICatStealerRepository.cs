using CatStealer.Application.Common.Pagination;
using CatStealer.Domain.Cats;

namespace CatStealer.Application.Common.Interfaces
{
    public interface ICatStealerRepository : IPagedRepository<CatEntity>
    {
        Task AddCatAsync(CatEntity cat);

        Task AddCats(List<CatEntity> cats);

        Task<CatEntity?> GetCatById(int id);

        Task<CatEntity?> GetCatWithTagsById(int id);

        Task<PagedResult<CatEntity>> GetPagedCatsAsync(
                                        PaginationParams paginationParams,
                                        FilterParams filterParams);
    }
}
