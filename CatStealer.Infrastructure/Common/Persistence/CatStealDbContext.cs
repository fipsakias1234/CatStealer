using CatStealer.Application.Common.Interfaces;
using CatStealer.Domain.Cats;
using CatStealer.Domain.Tags;
using Microsoft.EntityFrameworkCore;

namespace CatStealer.Infrastructure.Common.Persistence
{
    public class CatStealDbContext : DbContext, IUnitOfWork
    {
        public DbSet<CatEntity> StolenCats { get; set; } = null!;
        public DbSet<TagEntity> Tags { get; set; } = null!;

        public CatStealDbContext(DbContextOptions<CatStealDbContext> options) : base(options)
        {

        }
        public async Task CommitChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configured many-to-many relationship between CatEntity and TagEntity
            modelBuilder.Entity<CatEntity>()
                .HasMany(c => c.Tags)
                .WithMany(t => t.Cats)
                .UsingEntity(j => j.ToTable("CatTags")); // Optional: specify join table name
        }
    }
}
