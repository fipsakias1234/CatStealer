using CatStealer.Application.DTOs;
using ErrorOr;
using MediatR;

namespace CatStealer.Application.Cats.Commands.AddCats
{
    public class AddCatsCommandHandler : IRequestHandler<AddCatsCommand, ErrorOr<AddCatsDTO>>
    {
        public async Task<ErrorOr<AddCatsDTO>> Handle(AddCatsCommand request, CancellationToken cancellationToken)
        {
            return new AddCatsDTO();
        }
    }
}
