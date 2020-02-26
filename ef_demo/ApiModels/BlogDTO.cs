using System.Collections.Generic;

namespace ef_demo.ApiModels
{
    public class BlogDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int? Rating { get; set; }
        public IEnumerable<PostDTO> Posts { get; set; }
    }
}
