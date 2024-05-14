using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portrait_forum.Models;

namespace portrait_forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [portrait_forum.Helpers.Authorize]
    public class PostsController : ControllerBase
    {
        private readonly ForumContext _context;

        public PostsController(ForumContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostReturnDTO>>> GetPosts()
        {
            return await _context.Posts.Select(e => new PostReturnDTO
            {
                Content = e.Content,
                DateCreated = e.DateCreated,
                Owner = new UserDTO(e.Owner)
            }).ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(long id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(long id, PostUpdateDTO postDTO)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
                return NotFound();
            post.Content = postDTO.Content;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(PostDTO postDTO)
        {
            if (_context.Conversation.Find(postDTO.ConversationID) == null)
                return NotFound();
            User user = ((User)HttpContext.Items["User"]!);
            var post = new Post
            {
                ConversationID = postDTO.ConversationID,
                Content = postDTO.Content,
                Owner = user
            };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            var postReturnDTO = new PostReturnDTO(post);
            return CreatedAtAction("GetPost", new { id = post.Id }, postReturnDTO);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(long id)
        {
            User user = (User)HttpContext.Items["User"]!;
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            if(user.isAdmin || user.Id == post.OwnerID)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            return Unauthorized();
        }

        private bool PostExists(long id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
    
}
