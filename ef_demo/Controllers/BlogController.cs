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
using ef_demo.Core.Entities;
using ef_demo.Extensions;
using Newtonsoft.Json;

namespace ef_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BloggingContext _context;
        private readonly IMapper _mapper;

        public BlogController(BloggingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<BlogDto>> GetAsync()
        {
            var blogs = await _context.Blogs.Include(blog => blog.Posts).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<BlogDto>>(blogs);
            return dtos;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var blog = await _context.Blogs.Include(b => b.Posts)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return NotFound(new { Message = "Blog cannot be found" });
            }

            return Ok(_mapper.Map<BlogDto>(blog));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(PostBlogDto postBlog)
        {
            var blog = _mapper.Map<Blog>(postBlog);
            await _context.BeginTransactionAsync();

            var newBlog = await _context.Blogs.AddAsync(blog);

            try
            {
                await _context.CommitAsync();
                return Ok(newBlog.Entity.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);

            if (blog == null)
            {
                return NotFound(new { Message = "Blog cannot be found" });
            }

            try
            {
                await _context.BeginTransactionAsync();

                _context.Blogs.Remove(blog);

                await _context.CommitAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromForm]int id, [FromForm]string json)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);

            if (blog == null)
            {
                return NotFound(new { Message = "Blog cannot be found" });
            }

            JsonConvert.PopulateObject(json, blog);

            if (!TryValidateModel(blog))
                return BadRequest(ModelState.GetFullErrorMessage());

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("postperformance")]
        public async Task<IEnumerable<PostDto>> GetPostPerformanceAsync()
        {
            // A .com végződésű Blog Url-hez tartozó L betűvel kezdődő Post Content-eket kérdezzük le

            // Pl. Blog: googleblog.com
            //          Post Content: "Large Company"


            #region 1. Don't
            var stopWatch = Stopwatch.StartNew();

            var blogs = await _context.Blogs.Where(blog => blog.Url.EndsWith(".com")).ToListAsync();
            List<Post> posts = null;
            foreach (var blog in blogs)  // <-- N + 1 lekérdezés
            {
                posts = await _context.Posts
                    .Where(post => 
                        //post.BlogId == blog.Id &&
                        post.Content.StartsWith("C"))
                    .ToListAsync();   // <-- N + 1 lekérdezés
            }

            var dtos = _mapper.Map<IEnumerable<PostDto>>(posts);
            Console.WriteLine($"1. Don't Elapsed Time: {stopWatch.Elapsed}-------------------");

            #endregion

            #region 2. Not Bad
            stopWatch = Stopwatch.StartNew();

            var blogs2 = await _context.Blogs.Where(blog => blog.Url.EndsWith(".com"))
                .Include(blog => blog.Posts).ToListAsync();

            foreach (var blog in blogs2)
            {
                posts = blog.Posts.Where(post => post.Content.StartsWith("C")).ToList();
            }

            dtos = _mapper.Map<IEnumerable<PostDto>>(posts);
            Console.WriteLine($"2. Still bad Elapsed Time: {stopWatch.Elapsed}-------------------");

            #endregion

            #region 3. Good One
            stopWatch = Stopwatch.StartNew();

            posts = await _context.Blogs
                .Where(blog => blog.Url.EndsWith(".com"))
                .SelectMany(blog => blog.Posts.Where(post => post.Content.StartsWith("C")) 
                ).ToListAsync();

            dtos = _mapper.Map<IEnumerable<PostDto>>(posts);
            Console.WriteLine($"3. Good One Elapsed Time: {stopWatch.Elapsed}-------------------");

            return dtos;

            #endregion
        }
    }
}
