using CatStealer.Application.DTOs;
using MediatR;

namespace CatStealer.Application.Cats.Commands.AddCats
{
    public record AddCatsCommand(int numberOfCatsToAdd) : IRequest<AddCatsDTO>;
}
