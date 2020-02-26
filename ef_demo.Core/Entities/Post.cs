using System;

namespace ef_demo.Infrastructure.Core
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Posted { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; } // Navigation property
    }
}
