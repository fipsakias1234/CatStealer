using CatStealer.Application.Cats.Commands.AddCats;
using ErrorOr;

namespace CatStealer.Application.Common.Interfaces
{
    public interface ICatApiClient
    {
        Task<ErrorOr<List<AddCatsCommandHandler.CatApiResponse>>> GetCatsAsync(int limit, CancellationToken cancellationToken);
    }
}
