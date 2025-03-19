using CatStealer.Application.Services;
using CatStealer.Contracts.AddCats;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CatStealer.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CatStealerController : ControllerBase
    {

        private readonly ICatStealer _catStealer;

        public CatStealerController(ICatStealer catStealer)
        {
            _catStealer = catStealer;
        }

        [HttpPost("AddCats")]
        public IActionResult AddCats([FromBody] AddCatsRequest request)
        {
            //Logic which will call the API of the cats to steal

            var addedCatsInformation = _catStealer.addCatsDTO(request.NumberOfCatsToAdd);

            var response = addedCatsInformation.Adapt<AddCatsResponse>();

            return Ok(request);
        }
    }
}
