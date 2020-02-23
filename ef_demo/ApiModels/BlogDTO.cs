using ef_demo.Infrastructure.Core;
using System.Collections.Generic;
using System.Linq;

namespace ef_demo.ApiModels
{
    public class BlogDTO
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int? Rating { get; set; }
        public IEnumerable<PostDTO> Posts { get; set; }

        public static BlogDTO FromBlog(Blog blog)
        {
            return new BlogDTO
            {
                BlogId = blog.BlogId,
                Url = blog.Url,
                Rating = blog.Rating,
                Posts = blog.Posts?.Select(post => PostDTO.FromPost(post))
            };
        }
    }
}
