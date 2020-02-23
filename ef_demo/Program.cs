using ef_demo.Helpers;
using ef_demo.Infrastructure.Core;
using ef_demo.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace ef_demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    using var db = new BloggingContext();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    if (!db.Blogs.Any())
                    {
                        var blogs = new List<Blog>();

                        for (int i = 0; i < 100; i++)
                        {
                            blogs.Add(new Blog { 
                                Url = $"http://{Randomizer.RandomString(6)}.com",
                                Rating = Randomizer.RandomNumber(1, 5)
                            });
                        }

                        db.Blogs.AddRange(blogs);
                        db.SaveChanges();
                    }
                });
    }
}
