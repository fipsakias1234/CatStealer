using CatStealer.Application.DTOs;

namespace CatStealer.Application.Services
{
    public class CatStealerService : ICatStealer
    {
        public AddCatsDTO addCatsDTO(int numberOfCatsToAdd)
        {
            return new AddCatsDTO();
        }
    }
}
