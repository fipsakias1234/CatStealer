using CatStealer.Contracts.AddCats;
using Microsoft.AspNetCore.Mvc;

namespace CatStealer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatStealerController : ControllerBase
    {
        [HttpPost("AddCats")]
        public IActionResult AddCats([FromBody] AddCatsRequest request)
        {
            var addedCats = new List<AddCatDescriptionResponse>();

            //Logic which will call the API of the cats to steal

            return Ok(request);
        }
    }
}
