using CatStealer.Application.Common.Interfaces;
using CatStealer.Application.Common.Pagination;
using CatStealer.Application.DTOs;
using ErrorOr;
using MediatR;

namespace CatStealer.Application.Cats.Queries.GetPagedCats
{
    public class GetPagedCatsQueryHandler : IRequestHandler<GetPagedCatsQuery, ErrorOr<PagedResult<CatWithTagsDTO>>>
    {
        private readonly ICatStealerRepository _catsRepository;

        public GetPagedCatsQueryHandler(ICatStealerRepository catsRepository)
        {
            _catsRepository = catsRepository;
        }

        public async Task<ErrorOr<PagedResult<CatWithTagsDTO>>> Handle(
            GetPagedCatsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var pagedResult = await _catsRepository.GetPagedCatsAsync(
                    request.PaginationParams,
                    request.FilterParams);

                var dtoItems = pagedResult.Items.Select(cat => new CatWithTagsDTO
                {
                    Id = cat.Id,
                    CatId = cat.CatId,
                    Weight = cat.Weight,
                    Height = cat.Height,
                    Image = cat.Image,
                    Tags = cat.CatTags.Select(ct => ct.Tag.Name).ToList()
                }).ToList();

                var dtoPagedResult = new PagedResult<CatWithTagsDTO>
                {
                    Items = dtoItems,
                    TotalCount = pagedResult.TotalCount,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                };

                return dtoPagedResult;
            }
            catch (Exception ex)
            {
                return Error.Failure($"An error occurred while retrieving cats: {ex.Message}");
            }
        }
    }
}
