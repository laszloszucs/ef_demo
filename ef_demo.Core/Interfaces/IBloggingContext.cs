using System.Threading.Tasks;
using ef_demo.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ef_demo.Core.Interfaces
{
    public interface IBloggingContext
    {
        DbSet<Blog> Blogs { get; set; }
        DbSet<Post> Posts { get; set; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
