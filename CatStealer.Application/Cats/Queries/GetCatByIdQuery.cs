using CatStealer.Application.DTOs;
using ErrorOr;
using MediatR;

namespace CatStealer.Application.Cats.Queries
{
    public record GetCatByIdQuery(int id) : IRequest<ErrorOr<CatWithTagsDTO>>;
}
