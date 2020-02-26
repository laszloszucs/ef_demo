using FizzWare.NBuilder;
using System.Linq;
using ef_demo.Core.Entities;

namespace ef_demo.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BloggingContext context)
        {
            #if DEBUG  // preprocessor directives. Ha a DEBUG symbol definiálva van, akkor a fordító le fogja buildelni
                context.Database.EnsureDeleted(); // DEBUG
            #endif

            context.Database.EnsureCreated();
            context.Seed();            
        }   

        private static void Seed(this BloggingContext context)
        {
            if (context.Blogs.Any())
            {
                return;
            }
            
            var blogs = Builder<Blog>.CreateListOfSize(100)
                .All()
                .With(blog => blog.Id = 0)
                .With(blog => blog.Url = $"http://{Faker.Internet.DomainWord()}.{Faker.Internet.DomainSuffix()}")
                .With(blog => blog.Rating = Faker.RandomNumber.Next(1, 5))
                .Build();

            var posts = Builder<Post>.CreateListOfSize(100)
                .All()
                //.With(post => post.Blog = Pick<Blog>.RandomItemFrom(blogs))
                .With(post => post.Id = 0)
                .With(post => post.BlogId = null)
                .With(post => post.Blog = null)
                .With(post => post.Title = Faker.Lorem.GetFirstWord())
                .With(post => post.Content = Faker.Lorem.Paragraph())
                .Build();

            var tags = Builder<Tag>.CreateListOfSize(30)
                .All()
                .With(tag => tag.Id = 0)
                .Build();
            
            context.Blogs.AddRange(blogs);
            context.Posts.AddRange(posts);

            var randTags = Pick<Tag>.UniqueRandomList(With.Exactly(15).Elements).From(tags);
            var randPosts = Pick<Post>.UniqueRandomList(With.Exactly(15).Elements).From(posts);

            for (var i = 0; i < 15; i++)
            {
                context.PostTags.Add(new PostTag
                {
                    Post = randPosts[i],
                    Tag = randTags[i]
                });
            }
            
            context.SaveChanges();
        }
    }
}
