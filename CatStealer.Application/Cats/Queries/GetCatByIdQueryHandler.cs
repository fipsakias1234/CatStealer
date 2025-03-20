using CatStealer.Application.Common.Interfaces;
using CatStealer.Application.DTOs;
using ErrorOr;
using MediatR;

namespace CatStealer.Application.Cats.Queries
{
    public class GetCatByIdQueryHandler : IRequestHandler<GetCatByIdQuery, ErrorOr<CatWithTagsDTO>>
    {
        private readonly ICatStealerRepository _catStealRepository;

        public GetCatByIdQueryHandler(ICatStealerRepository catStealRepository)
        {
            _catStealRepository = catStealRepository;
        }

        public async Task<ErrorOr<CatWithTagsDTO>> Handle(GetCatByIdQuery request, CancellationToken cancellationToken)
        {
            var catEntity = await _catStealRepository.GetCatWithTagsById(request.id);
            if (catEntity == null)
            {
                return Error.NotFound(description: "Cat Not Found");
            }

            var catDto = new CatWithTagsDTO
            {
                Id = catEntity.Id,
                CatId = catEntity.CatId,
                Weight = catEntity.Weight,
                Height = catEntity.Height,
                Image = catEntity.Image,
                Tags = catEntity.CatTags.Select(ct => ct.Tag.Name).ToList()
            };

            return catDto;
        }
    }
}
