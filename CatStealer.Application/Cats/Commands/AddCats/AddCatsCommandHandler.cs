using CatStealer.Application.Common.Interfaces;
using CatStealer.Application.DTOs;
using CatStealer.Domain.Cats;
using CatStealer.Domain.CatsTagsBridge;
using CatStealer.Domain.Tags;
using ErrorOr;
using MediatR;
using System.Text.Json;

namespace CatStealer.Application.Cats.Commands.AddCats
{
    public class AddCatsCommandHandler : IRequestHandler<AddCatsCommand, ErrorOr<AddCatsDTO>>
    {

        private readonly ICatStealerRepository _catStealRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private const string ApiKey = "live_2l9wxyFcIbu2xTuzZP0udijzFNlt5MhpSGVMP4c3ByBAs4OPDKqxSbnq4usUGLNk";

        public AddCatsCommandHandler(ICatStealerRepository catStealRepository, IUnitOfWork unitOfWork, ITagRepository tagRepository, HttpClient httpClient)
        {
            _catStealRepository = catStealRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
        }
        public async Task<ErrorOr<AddCatsDTO>> Handle(AddCatsCommand request, CancellationToken cancellationToken)
        {

            // API request setup remains the same
            var apiUrl = $"https://api.thecatapi.com/v1/images/search?limit={request.numberOfCatsToAdd}&api_key={ApiKey}&has_breeds=1";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            httpRequest.Headers.Add("x-api-key", ApiKey);

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return Error.Failure("Failed to retrieve cats from the API.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var catApiResponses = JsonSerializer.Deserialize<List<CatApiResponse>>(jsonResponse, options);
            if (catApiResponses == null || !catApiResponses.Any())
            {
                return Error.Failure("No cats were retrieved from the API.");
            }

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
