using CatStealer.Api.Controllers.Models;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CatStealer.Api.Controllers.CustomController
{
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            if (errors.Count is 0)
            {
                return Problem();
            }

            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            return Problem(errors[0]);
        }

        protected IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            var response = new ApiResponse
            {
                Success = false,
                Errors = new List<Error> { error }
            };

            return StatusCode(statusCode, response);
        }

        protected IActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                    error.Description);
            }

            var response = new ApiResponse
            {
                Success = false,
                Errors = errors
            };

            return BadRequest(response);
        }

        protected IActionResult OkResult<T>(T data)
        {
            var response = new ApiResponse<T>
            {
                Success = true,
                Data = data
            };

            return Ok(response);
        }

    }
}
