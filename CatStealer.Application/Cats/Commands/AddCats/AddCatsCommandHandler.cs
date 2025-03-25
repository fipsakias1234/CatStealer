using CatStealer.Application.Common.Interfaces;
using CatStealer.Application.DTOs;
using CatStealer.Domain.Cats;
using CatStealer.Domain.CatsTagsBridge;
using CatStealer.Domain.Tags;
using ErrorOr;
using MediatR;

namespace CatStealer.Application.Cats.Commands.AddCats
{
    public class AddCatsCommandHandler : IRequestHandler<AddCatsCommand, ErrorOr<AddCatsDTO>>
    {

        private readonly ICatStealerRepository _catStealRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICatApiClient _catApiClient;

        public AddCatsCommandHandler(ICatStealerRepository catStealRepository, IUnitOfWork unitOfWork, ITagRepository tagRepository, ICatApiClient catApiClient)
        {
            _catStealRepository = catStealRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _catApiClient = catApiClient;
        }
        public async Task<ErrorOr<AddCatsDTO>> Handle(AddCatsCommand request, CancellationToken cancellationToken)
        {

            // Get cats from the API using the client
            var catApiResponsesResult = await _catApiClient.GetCatsAsync(request.numberOfCatsToAdd, cancellationToken);

            if (catApiResponsesResult.IsError)
            {
                return catApiResponsesResult.Errors;
            }

            var catApiResponses = catApiResponsesResult.Value;

            var existingTagsByName = new Dictionary<string, TagEntity>(StringComparer.OrdinalIgnoreCase);
            var newTags = new List<TagEntity>();
            var catsToAdd = new List<CatEntity>();
            var addedCats = new List<AddCatDescriptionDTO>();

            foreach (var catApiResponse in catApiResponses)
            {
                var catEntity = new CatEntity
                {
                    CatId = catApiResponse.Id,
                    Weight = catApiResponse.Width,
                    Height = catApiResponse.Height,
                    Image = catApiResponse.Url,
                    CreatedOn = DateTime.UtcNow,
                    CatTags = new List<CatTags>()
                };

                catsToAdd.Add(catEntity);

                var catTagNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Extract all temperament tags
                foreach (var breed in catApiResponse.Breeds)
                {
                    if (!string.IsNullOrEmpty(breed.Temperament))
                    {
                        var temperaments = breed.Temperament.Split(',', StringSplitOptions.TrimEntries);
                        foreach (var temperament in temperaments)
                        {
                            catTagNames.Add(temperament);
                        }
                    }
                }

                // Store the cat for Response
                addedCats.Add(new AddCatDescriptionDTO
                {
                    CatId = catEntity.CatId,
                    Tags = catTagNames.ToList()
                });
            }

            // Batch query for all existing tags we need
            var allTagNames = addedCats.SelectMany(cat => cat.Tags).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            foreach (var tagName in allTagNames)
            {
                var existingTag = await _tagRepository.GetTagByNameAsync(tagName);
                if (existingTag != null)
                {
                    existingTagsByName[tagName] = existingTag;
                }
                else
                {
                    var newTag = new TagEntity
                    {
                        Name = tagName,
                        CreatedOn = DateTime.UtcNow
                    };
                    newTags.Add(newTag);
                    existingTagsByName[tagName] = newTag;
                }
            }

            // Batch insert all new tags
            if (newTags.Any())
            {
                await _tagRepository.AddTagsAsync(newTags);
            }

            // Create relationships between cats and tags
            for (int i = 0; i < catsToAdd.Count; i++)
            {
                var cat = catsToAdd[i];
                var tagNames = addedCats[i].Tags;

                foreach (var tagName in tagNames)
                {
                    if (existingTagsByName.TryGetValue(tagName, out var tag))
                    {
                        cat.CatTags.Add(new CatTags
                        {
                            Cat = cat,
                            Tag = tag
                        });
                    }
                }
            }

            // Batch insert all cats with their tag relationships
            await _catStealRepository.AddCats(catsToAdd);

            // Commit all changes in a single transaction
            await _unitOfWork.CommitChangesAsync();

            return new AddCatsDTO { AddedCats = addedCats };
        }

        public class CatApiResponse
        {
            public string Id { get; set; }
            public string Url { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public List<Breed> Breeds { get; set; } = new();
        }

        public class Breed
        {
            public string Temperament { get; set; }
        }
    }
}
