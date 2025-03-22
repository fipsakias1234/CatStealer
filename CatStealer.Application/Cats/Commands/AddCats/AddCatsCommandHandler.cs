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

            var addedCats = new List<AddCatDescriptionDTO>();

            foreach (var catApiResponse in catApiResponses)
            {

                var catEntity = new CatEntity
                {
                    CatId = catApiResponse.Id,
                    Weight = catApiResponse.Width,
                    Height = catApiResponse.Height,
                    Image = catApiResponse.Url
                };


                var tags = new List<TagEntity>();
                foreach (var breed in catApiResponse.Breeds)
                {
                    if (!string.IsNullOrEmpty(breed.Temperament))
                    {
                        var temperaments = breed.Temperament.Split(',', StringSplitOptions.TrimEntries);
                        foreach (var temperament in temperaments)
                        {
                            var tag = await _tagRepository.GetTagByNameAsync(temperament);
                            if (tag == null)
                            {
                                tag = new TagEntity { Name = temperament };
                                await _tagRepository.AddTagAsync(tag);
                            }
                            tags.Add(tag);
                        }
                    }
                }


                catEntity.CatTags = tags.Select(tag => new CatTags
                {
                    Cat = catEntity,
                    Tag = tag
                }).ToList();


                await _catStealRepository.AddCatAsync(catEntity);

                addedCats.Add(new AddCatDescriptionDTO
                {
                    CatId = catEntity.CatId,
                    Tags = tags.Select(t => t.Name).ToList()
                });
            }

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
