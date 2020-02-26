using System.Collections.Generic;
using ef_demo.ApiModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ef_demo.Infrastructure.Data;
using System.Linq;
using System;
using System.Diagnostics;
using ef_demo.Infrastructure.Core;

namespace ef_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BloggingContext _bloggingContext;
        private readonly IMapper _mapper;

        public BlogController(BloggingContext bloggingContext, IMapper mapper)
        {
            _bloggingContext = bloggingContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<BlogDTO>> GetAsync()
        {
            var blogs = await _bloggingContext.Blogs.Include(blog => blog.Posts).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<BlogDTO>>(blogs);
            return dtos;
        }

        [HttpGet("postperformance")]
        public async Task<IEnumerable<PostDTO>> GetPostPerformanceAsync()
        {
            // A .com végződésű Blog Url-hez tartozó L betűvel kezdődő Post Content-eket kérdezzük le

            // Pl. Blog: googleblog.com
            //          Post Content: "Large Company"


            #region 1. Don't
            var stopWatch = Stopwatch.StartNew();

            var blogs = await _bloggingContext.Blogs.Where(blog => blog.Url.EndsWith(".com")).ToListAsync();
            List<Post> posts = null;
            foreach (var blog in blogs)  // <-- N + 1 lekérdezés
            {
                posts = await _bloggingContext.Posts
                    .Where(post => 
                        post.BlogId == blog.Id &&
                        post.Content.StartsWith("C"))
                    .ToListAsync();   // <-- N + 1 lekérdezés
            }

            var dtos = _mapper.Map<IEnumerable<PostDTO>>(posts);
            Console.WriteLine($"1. Don't Elapsed Time: {stopWatch.Elapsed}-------------------");

            #endregion

            #region 2. Not Bad
            stopWatch = Stopwatch.StartNew();

            var blogs2 = await _bloggingContext.Blogs.Where(blog => blog.Url.EndsWith(".com"))
                .Include(blog => blog.Posts).ToListAsync();

            foreach (var blog in blogs2)
            {
                posts = blog.Posts.Where(post => post.Content.StartsWith("C")).ToList();
            }

            dtos = _mapper.Map<IEnumerable<PostDTO>>(posts);
            Console.WriteLine($"2. Still bad Elapsed Time: {stopWatch.Elapsed}-------------------");

            #endregion

            #region 3. Good One
            stopWatch = Stopwatch.StartNew();

            posts = await _bloggingContext.Blogs
                .Where(blog => blog.Url.EndsWith(".com"))
                .SelectMany(blog => blog.Posts.Where(post => post.Content.StartsWith("C")) 
                ).ToListAsync();

            dtos = _mapper.Map<IEnumerable<PostDTO>>(posts);
            Console.WriteLine($"3. Good One Elapsed Time: {stopWatch.Elapsed}-------------------");

            return dtos;

            #endregion
        }
    }
}
