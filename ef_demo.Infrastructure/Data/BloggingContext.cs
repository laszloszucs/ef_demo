using ef_demo.Core.Interfaces;
using ef_demo.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ef_demo.Infrastructure.Data
{
    public class BloggingContext : DbContext, IBloggingContext
    {
        public BloggingContext(DbContextOptions options) : base(options)
        {
            //Database..Log = Console.WriteLine;
        }

        /*
            DbSet = Entitáns készlet

            Egy DbSet megfelel az adatbázis egy táblájának.
            Egy Entity megfelel egy tábla egy sorának (rekordjának).
            Az Entity property-k pedig a mezők.

        */
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        private IDbContextTransaction _transaction;

        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                SaveChanges();
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        // Workaround for [Range] Validation https://www.bricelam.net/2016/12/13/validation-in-efcore.html
        public override int SaveChanges()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;

            var entities2 = ChangeTracker.Entries()
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
                    validateAllProperties: true);
            }

            return base.SaveChanges();
        }
    }
}
