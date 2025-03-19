using CatStealer.Domain.Cats;
using ErrorOr;
using MediatR;

namespace CatStealer.Application.Cats.Queries
{
    public record GetCatByIdQuery(int catId) : IRequest<ErrorOr<CatEntity>>;
}
