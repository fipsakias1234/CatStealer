using CatStealer.Application.Cats.Commands.AddCats;
using CatStealer.Contracts.AddCats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatStealer.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CatStealerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CatStealerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddCats")]
        public async Task<IActionResult> AddCats([FromBody] AddCatsRequest request)
        {
            //Logic which will call the API of the cats to steal
            var command = new AddCatsCommand(request.NumberOfCatsToAdd);


            var addCatsResult = await _mediator.Send(command);

            return addCatsResult.MatchFirst
                 (
                   addCatsDTO => Ok(addCatsDTO),
                     error => Problem()
                 );
        }
    }
}
