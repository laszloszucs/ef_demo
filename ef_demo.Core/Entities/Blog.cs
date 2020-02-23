using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ef_demo.Infrastructure.Core
{
    public class Blog
    {
        public int BlogId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Url { get; set; }
        [Range(1, 5)]
        public int? Rating { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
