using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CatStealer.Application.Validation
{
    public class ValidatorService : IValidatorService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ValidatorService> _logger;

        public ValidatorService(IServiceProvider serviceProvider, ILogger<ValidatorService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<ErrorOr<Success>> ValidateAsync<T>(T entity, Type validatorType) where T : class
        {
            try
            {
                var validator = _serviceProvider.GetService(validatorType) as IValidator<T>;

                if (validator == null)
                {
                    return Error.Unexpected(description: "A validation configuration for the specified request has not been found", code: $"Validation.NotFound");
                }

                var validationResult = await validator.ValidateAsync(entity);

                if (!validationResult.IsValid)
                {
                    return Error.Validation(description: $"The request is not valid: {validationResult.Errors[0].ErrorMessage}");
                }

                return Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validation.UnexpectedError");
                return Error.Failure(ex.Message, "Validation.Unexpected Error");
            }
        }
    }
}
