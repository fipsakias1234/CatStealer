using CatStealer.Application.Common.Interfaces;
using CatStealer.Domain.Cats;
using CatStealer.Domain.CatsTagsBridge;
using CatStealer.Domain.Tags;
using Microsoft.EntityFrameworkCore;

namespace CatStealer.Infrastructure.Common.Persistence
{
    public class CatStealDbContext : DbContext, IUnitOfWork
    {
        public DbSet<CatEntity> StolenCats { get; set; } = null!;
        public DbSet<TagEntity> Tags { get; set; } = null!;
        public DbSet<CatTags> CatTags { get; set; } = null!; // Add DbSet for CatTags

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

            // Configure the join entity CatTags
            modelBuilder.Entity<CatTags>(entity =>
            {
                entity.HasKey(e => new { e.CatsId, e.TagsId }); // Composite primary key

                entity.HasOne(e => e.Cat)
                    .WithMany(c => c.CatTags)
                    .HasForeignKey(e => e.CatsId);

                entity.HasOne(e => e.Tag)
                    .WithMany(t => t.CatTags)
                    .HasForeignKey(e => e.TagsId);

                entity.ToTable("CatTags"); // Map to existing table
            });
        }
    }
}
