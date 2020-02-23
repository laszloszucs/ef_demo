using ef_demo.Infrastructure.Core;

namespace ef_demo.ApiModels
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public static PostDTO FromPost(Post post)
        {
            return new PostDTO
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content
            };
        }
    }
}