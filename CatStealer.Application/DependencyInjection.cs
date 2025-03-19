using CatStealer.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CatStealer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICatStealer, CatStealerService>();
            return services;
        }
    }
}
