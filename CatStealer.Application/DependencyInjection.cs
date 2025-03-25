using CatStealer.Application.Cats.Validators;
using CatStealer.Application.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace CatStealer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(
                options =>
                {
                    options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
                }
                );
            services.AddHttpClient();


            services.AddScoped<IValidatorService, ValidatorService>();
            services.AddScoped<ICatStealerValidator, CatStealerValidator>();

            return services;
        }
    }
}
