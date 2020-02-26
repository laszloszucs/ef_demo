using System;
using System.Collections.Generic;

namespace ef_demo.Core.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Posted { get; set; }
        public int? BlogId { get; set; }
        public Blog Blog { get; set; } // Navigation property
        public List<PostTag> PostTags { get; set; }
    }
}
