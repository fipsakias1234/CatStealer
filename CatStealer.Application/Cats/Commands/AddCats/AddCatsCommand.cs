using CatStealer.Application.DTOs;
using ErrorOr;
using MediatR;

namespace CatStealer.Application.Cats.Commands.AddCats
{
    public record AddCatsCommand(int numberOfCatsToAdd) : IRequest<ErrorOr<AddCatsDTO>>;
}
