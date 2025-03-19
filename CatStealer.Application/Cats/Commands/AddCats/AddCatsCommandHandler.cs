using CatStealer.Application.DTOs;
using MediatR;

namespace CatStealer.Application.Cats.Commands.AddCats
{
    public class AddCatsCommandHandler : IRequestHandler<AddCatsCommand, AddCatsDTO>
    {
        public Task<AddCatsDTO> Handle(AddCatsCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new AddCatsDTO());
        }
    }
}
