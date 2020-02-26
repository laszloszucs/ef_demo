using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ef_demo.Infrastructure.Core
{
    public class Blog
    {
        public int Id { get; set; }
        //public int BlogId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Url { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        public IEnumerable<Post> Posts { get; set; } // Navigation Property
        /*
            A navigation property-k más entityket tartalmaznak,
            melyek ehhez az entitánshoz kapcsolódnak.
            Külső kulcs hivatkozás alapján kapcsolódik.
         */
    }
}
