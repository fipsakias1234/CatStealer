using CatStealer.Domain.Cats;
using ErrorOr;
using MediatR;

namespace CatStealer.Application.Cats.Queries
{
    public class GetCatByIdQueryHandler : IRequestHandler<GetCatByIdQuery, ErrorOr<CatEntity>>
    {
        public Task<ErrorOr<CatEntity>> Handle(GetCatByIdQuery request, CancellationToken cancellationToken)
        {
            var catEntity = new CatEntity();
            return Task.FromResult<ErrorOr<CatEntity>>(
                        catEntity is null
                            ? Error.NotFound(description: "Cat Not Found")
                            : catEntity);
        }
    }
}
