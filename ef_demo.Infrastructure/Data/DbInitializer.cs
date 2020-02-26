using ef_demo.Infrastructure.Core;
using FizzWare.NBuilder;
using System.Linq;

namespace ef_demo.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BloggingContext context)
        {
            #if DEBUG  // preprocessor directives. Ha a DEBUG symbol definiálva van, akkor a fordító le fogja buildelni
                // context.Database.EnsureDeleted(); // DEBUG
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

            var blogs = Builder<Blog>.CreateListOfSize(10000)
                .All()
                    .With(blog => blog.Url = $"http://{Faker.Internet.DomainWord()}.{Faker.Internet.DomainSuffix()}")
                    .With(blog => blog.Rating = Faker.RandomNumber.Next(1, 5))
                .Build();

            var posts = Builder<Post>.CreateListOfSize(10000)
                .All()
                    .With(post => post.Blog = Pick<Blog>.RandomItemFrom(blogs))
                    .With(post => post.Title = Faker.Lorem.GetFirstWord())
                    .With(post => post.Content = Faker.Lorem.Paragraph())
                .Build();

            context.BeginTransaction();

            context.Blogs.AddRange(blogs);
            context.Posts.AddRange(posts);

            context.Commit();
        }
    }
}
