using System.Collections.Generic;

namespace ef_demo.ApiModels
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int? Rating { get; set; }
        public IEnumerable<PostDto> Posts { get; set; }
    }

    public class PostBlogDto
    {
        public string Url { get; set; }
        public int? Rating { get; set; }
        public IEnumerable<PostPostDto> Posts { get; set; }
    }
}
