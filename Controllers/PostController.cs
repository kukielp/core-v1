using pgapp.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace pgapp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostsController : ControllerBase
	{
		private readonly ApplicationContext _context;

		public PostsController(ApplicationContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult Get()
		{
			var posts = _context.Posts;

			return Ok(posts);
		}

		[HttpGet("{id}", Name = "GetById")]
		public IActionResult GetById(int id)
		{
			var post = _context.Posts
                       .Where(p => p.PostId == id)
                       .Include(c => c.Comments)
                       .FirstOrDefault();

			return Ok(post);
		}

		[HttpPost]
		public IActionResult Post([FromBody] Post post)
		{
			_context.Add(post);

			_context.SaveChanges();

			return CreatedAtRoute(nameof(GetById), new { id = post.PostId }, post);
		}

        [HttpPost("{id}/addcomment", Name = "AddComment")]
        public ActionResult AddComment(int id, [FromBody] Comment comment)
        {
			var post = _context.Posts.Include(p => p.Comments).First();
			
			post.Comments.Add(comment);
			_context.SaveChanges();

			var result = _context.Posts
                       .Where(p => p.PostId == id)
					   .Include(c => c.Comments)
					   .FirstOrDefault();
			
			return CreatedAtRoute(nameof(AddComment), result);
		}
	}
}