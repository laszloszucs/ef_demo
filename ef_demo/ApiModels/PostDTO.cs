using System;

namespace ef_demo.ApiModels
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Posted { get; set; }
    }

    public class PostPostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}