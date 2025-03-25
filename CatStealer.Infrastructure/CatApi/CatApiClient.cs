using CatStealer.Application.Cats.Commands.AddCats;
using CatStealer.Application.Common.Interfaces;
using CatStealer.Application.Common.Models;
using ErrorOr;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CatStealer.Infrastructure.CatApi
{
    public class CatApiClient : ICatApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public CatApiClient(HttpClient httpClient, IOptions<CatApiOptions> options)
        {
            _httpClient = httpClient;
            _apiKey = options.Value.ApiKey;

            // Configure base address from options
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);

            // Add default headers
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
        }

        public async Task<ErrorOr<List<AddCatsCommandHandler.CatApiResponse>>> GetCatsAsync(
            int limit,
            CancellationToken cancellationToken)
        {
            var apiUrl = $"images/search?limit={limit}&api_key={_apiKey}&has_breeds=1";

            var response = await _httpClient.GetAsync(apiUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return Error.Failure("Failed to retrieve cats from the API.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var catApiResponses = JsonSerializer.Deserialize<List<AddCatsCommandHandler.CatApiResponse>>(jsonResponse, options);
            if (catApiResponses == null || !catApiResponses.Any())
            {
                return Error.Failure("No cats were retrieved from the API.");
            }

            return catApiResponses;
        }
    }
}