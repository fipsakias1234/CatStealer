using CatStealer.Application.Common.Interfaces;
using CatStealer.Application.DTOs;
using CatStealer.Domain.Cats;
using ErrorOr;
using MediatR;

namespace CatStealer.Application.Cats.Commands.AddCats
{
    public class AddCatsCommandHandler : IRequestHandler<AddCatsCommand, ErrorOr<AddCatsDTO>>
    {

        private readonly ICatStealerRepository _catStealRepository;

        private readonly IUnitOfWork _unitOfWork;

        public AddCatsCommandHandler(ICatStealerRepository catStealRepository, IUnitOfWork unitOfWork)
        {
            _catStealRepository = catStealRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<AddCatsDTO>> Handle(AddCatsCommand request, CancellationToken cancellationToken)
        {

            List<CatEntity> stolencats = new List<CatEntity>();

            await _catStealRepository.AddCats(stolencats);

            await _unitOfWork.CommitChangesAsync();

            return new AddCatsDTO();
        }
    }
}
