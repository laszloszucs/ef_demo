using System.Collections.Generic;

namespace ef_demo.Core.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public List<PostTag> PostTags { get; set; }
    }
}
