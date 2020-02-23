using System.Collections.Generic;
using System.Linq;
using ef_demo.ApiModels;
using ef_demo.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ef_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly ILogger<BlogController> _logger;

        public BlogController(ILogger<BlogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<BlogDTO> Get()
        {
            List<BlogDTO> blogs = null;

            using (var db = new BloggingContext())
            {
                blogs = db.Blogs
                    .Select(blog => BlogDTO.FromBlog(blog))
                    .ToList();

                _logger.LogInformation(blogs.ToString());
            }

            return blogs;
        }
    }
}
