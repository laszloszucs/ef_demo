using ef_demo.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ef_demo.Infrastructure.Data
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseNpgsql(
                @"Host=localhost;Database=Blogging;Username=ef_demo;Password=Password01");
            //optionsBuilder.UseSqlServer(
            //    @"Server=(localdb)\mssqllocaldb;Database=Blogging;Integrated Security=True");
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
