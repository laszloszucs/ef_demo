using ef_demo.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ef_demo.Core.Entities;

namespace ef_demo.Infrastructure.Data
{
    public class BloggingContext : DbContext, IBloggingContext
    {
        public BloggingContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne(b => b.Blog)
                .WithMany(a => a.Posts)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);
        }

        /*
            DbSet = Entitáns készlet

            Egy DbSet megfelel az adatbázis egy táblájának.
            Egy Entity megfelel egy tábla egy sorának (rekordjának).
            Az Entity property-k pedig a mezők.

        */
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        private IDbContextTransaction _transaction;

        public async Task BeginTransactionAsync()
        {
            _transaction = await Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        // Workaround for [Range] Validation https://www.bricelam.net/2016/12/13/validation-in-efcore.html
        public override int SaveChanges()
        {
            Validate();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            Validate();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void Validate()
        {
            var entities = ChangeTracker.Entries()
                .Where(entity =>
                    entity.State == EntityState.Added ||
                    entity.State == EntityState.Modified)
                .Select(entity => entity.Entity);

            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(
                    entity,
                    validationContext,
                    true);
            }
        }
    }
}
