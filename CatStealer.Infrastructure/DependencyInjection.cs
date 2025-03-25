using CatStealer.Application.Common.Interfaces;
using CatStealer.Application.Common.Models;
using CatStealer.Infrastructure.CatApi;
using CatStealer.Infrastructure.Cats.Persistence;
using CatStealer.Infrastructure.Common.Persistence;
using CatStealer.Infrastructure.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatStealer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatStealDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("SqlDbConnection"),
                   b => b.MigrationsAssembly(typeof(CatStealDbContext).Assembly.FullName)));
            services.AddScoped<ICatStealerRepository, CatsRepository>();
            services.AddScoped<ITagRepository, TagRepository>();

            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<CatStealDbContext>());

            services.Configure<CatApiOptions>(configuration.GetSection(CatApiOptions.SectionName));
            services.AddHttpClient<ICatApiClient, CatApiClient>();

            return services;
        }
    }
}
