using CatStealer.Application.Common.Pagination;
using CatStealer.Application.DTOs;
using ErrorOr;
using MediatR;

namespace CatStealer.Application.Cats.Queries.GetPagedCats
{
    public record GetPagedCatsQuery(
            PaginationParams PaginationParams,
            FilterParams FilterParams) : IRequest<ErrorOr<PagedResult<CatWithTagsDTO>>>;
}
