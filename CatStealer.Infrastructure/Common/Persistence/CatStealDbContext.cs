using CatStealer.Application.Common.Interfaces;
using CatStealer.Domain.Cats;
using Microsoft.EntityFrameworkCore;

namespace CatStealer.Infrastructure.Common.Persistence
{
    public class CatStealDbContext : DbContext, IUnitOfWork
    {
        public DbSet<CatEntity> StolenCats { get; set; } = null!;

        public CatStealDbContext(DbContextOptions<CatStealDbContext> options) : base(options)
        {

        }
        public async Task CommitChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
