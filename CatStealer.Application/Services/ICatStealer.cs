using CatStealer.Application.DTOs;

namespace CatStealer.Application.Services
{
    public interface ICatStealer
    {
        AddCatsDTO addCatsDTO(int numberOfCatsToAdd);
    }
}
