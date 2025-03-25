using CatStealer.Api.Controllers.CustomController;
using CatStealer.Application.Cats.Commands.AddCats;
using CatStealer.Application.Cats.Queries;
using CatStealer.Application.Cats.Queries.GetPagedCats;
using CatStealer.Application.Cats.Validators;
using CatStealer.Application.Common.Pagination;
using CatStealer.Application.DTOs;
using CatStealer.Application.Validation;
using CatStealer.Contracts.AddCats;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace CatStealer.Api.Controllers
{
    /// <summary>
    /// Controller for managing cat data operations including adding new cats and retrieving cat information
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CatStealerController : ApiController
    {
        private readonly IMediator _mediator;

        private readonly IValidatorService _validateService;
        public CatStealerController(IMediator mediator, IValidatorService validateService)
        {
            _mediator = mediator;
            _validateService = validateService;
        }

        /// <summary>
        /// Adds a specified number of cats fetched from The Cat API to the database
        /// </summary>
        /// <param name="request">The request containing the number of cats to add</param>
        /// <returns>
        /// 200 OK with details of the added cats including tags
        /// 400 Bad Request if the number of cats requested is invalid
        /// </returns>
        /// <remarks>
        /// The number of cats must be between 1 and 100.
        /// Each cat is fetched with breed information and tagged with temperament attributes.
        /// </remarks>
        [HttpPost("AddCats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCats([FromBody] AddCatsRequest request)
        {


            var dto = request.Adapt<AddCatRequestDTO>();

            var validationResult = await _validateService.ValidateAsync(dto, typeof(ICatStealerValidator));
            if (validationResult.IsError)
                return Problem(Error.Validation(validationResult.Errors.First().Code, validationResult.Errors.First().Description));

            //Logic which will call the API of the cats to steal
            var command = new AddCatsCommand(dto.NumberOfCatsToAdd);

            var addCatsResult = await _mediator.Send(command);

            return addCatsResult.Match
                 (
                   addCatsDTO => Ok(addCatsDTO),
                     error => Problem(error)
                 );
        }

        /// <summary>
        /// Retrieves a specific cat by its unique identifier
        /// </summary>
        /// <param name="id">The database ID of the cat to retrieve</param>
        /// <returns>
        /// 200 OK with the cat's details including associated tags
        /// 404 Not Found if no cat exists with the specified ID
        /// </returns>
        /// <remarks>
        /// This endpoint returns full cat details including image URL and all associated temperament tags.
        /// </remarks>

        [HttpGet("GetCatById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCatById([FromRoute] int id)
        {

            var getCatQueryResult = await _mediator.Send(new GetCatByIdQuery(id));

            return getCatQueryResult.Match(
                catDto => Ok(catDto),
                error => Problem(error)
            );
        }

        /// <summary>
        /// Retrieves a paginated list of cats with optional filtering by tag name
        /// </summary>
        /// <param name="paginationParams">
        /// Pagination parameters:
        /// - pageNumber: The page number to retrieve (1-based)
        /// - pageSize: The number of cats per page
        /// </param>
        /// <param name="filterParams">
        /// Filter parameters:
        /// - tagName: Optional name of a tag to filter cats by
        /// </param>
        /// <returns>
        /// 200 OK with a paged result containing:
        /// - Items: List of cats for the current page
        /// - TotalCount: Total number of cats (before pagination)
        /// - PageNumber: Current page number
        /// - PageSize: Number of items per page
        /// </returns>
        /// <remarks>
        /// Use this endpoint for efficiently browsing the cat collection with server-side pagination.
        /// The response includes metadata for building a pager control.
        /// </remarks>
        [HttpGet("GetPagedCats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
