using CatStealer.Api.Controllers.CustomController;
using CatStealer.Application.Cats.Commands.AddCats;
using CatStealer.Application.Cats.Queries;
using CatStealer.Application.Cats.Queries.GetPagedCats;
using CatStealer.Application.Common.Pagination;
using CatStealer.Contracts.AddCats;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatStealer.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CatStealerController : ApiController
    {
        private readonly IMediator _mediator;
        public CatStealerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddCats")]
        public async Task<IActionResult> AddCats([FromBody] AddCatsRequest request)
        {

            if (request.NumberOfCatsToAdd < 0)
            {
                return Problem(Error.Validation(description: "Number of cat to Add cannot be less than zero", code: "AddCats.ValidationError"));
            }

            if (request.NumberOfCatsToAdd > 100)
            {
                return Problem(Error.Validation(description: "Please don't be greedy you can steal maximum 100 cats each time", code: "AddCats.ValidationError"));

            }
            //Logic which will call the API of the cats to steal
            var command = new AddCatsCommand(request.NumberOfCatsToAdd);

            var addCatsResult = await _mediator.Send(command);

            return addCatsResult.Match
                 (
                   addCatsDTO => Ok(addCatsDTO),
                     error => Problem(error)
                 );
        }

        [HttpGet("GetCatById/{id}")]
        public async Task<IActionResult> GetCatById([FromRoute] int id)
        {

            var getCatQueryResult = await _mediator.Send(new GetCatByIdQuery(id));

            return getCatQueryResult.Match(
                catDto => Ok(catDto),
                error => Problem(error)
            );
        }

        /// <summary>
        /// Retrieves paged cats with optional filtering by tag.
        /// </summary>
        /// <param name="paginationParams">Pagination parameters: pageNumber and pageSize.</param>
        /// <param name="filterParams">Filter parameter: tagName.</param>
        /// <returns>A paged list of cats.</returns>
        [HttpGet("GetPagedCats")]
        public async Task<IActionResult> GetPagedCats(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] FilterParams filterParams)
        {
            var query = new GetPagedCatsQuery(paginationParams, filterParams);
            var result = await _mediator.Send(query);

            return result.Match(
                pagedCats => Ok(pagedCats),
                error => Problem(error)
            );
        }
    }
}
