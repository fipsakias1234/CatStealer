using ErrorOr;

namespace CatStealer.Application.Validation
{
    public interface IValidatorService
    {
        Task<ErrorOr<Success>> ValidateAsync<T>(T entity, Type validatorType) where T : class;
    }
}
