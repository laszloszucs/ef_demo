using ef_demo.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;

namespace ef_demo.Core.Interfaces
{
    public interface IBloggingContext
    {
        DbSet<Blog> Blogs { get; set; }
        DbSet<Post> Posts { get; set; }

        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
