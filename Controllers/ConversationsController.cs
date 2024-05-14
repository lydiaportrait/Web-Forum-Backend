using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ConversationsController : ControllerBase
    {
        private readonly ForumContext _context;

        public ConversationsController(ForumContext context)
        {
            _context = context;
        }

        // api/id/Posts
        [HttpGet("{id}/Posts")]
        public async Task<ActionResult<IEnumerable<PostReturnDTO>>> GetPosts(long id)
        {
            if (!await _context.Conversation.AnyAsync(e => e.Id == id))
                return NotFound();
            var posts = await _context.Posts.Select(e => new PostReturnDTO
            {
                Content = e.Content,
                Id = e.Id,
                ConversationID = e.ConversationID,
                DateCreated = e.DateCreated,
                Owner = new UserDTO(e.Owner),
            })
                .Where(e => e.ConversationID == id).ToListAsync();
            return posts;
        }
        // GET: api/Conversations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConversationReturnDTO>>> GetConversation()
        {
            return await _context.Conversation.Select(e => new ConversationReturnDTO
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Owner = new UserDTO(e.Owner)
            }).ToListAsync();
        }

        // GET: api/Conversations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConversationReturnDTO>> GetConversation(long id)
        {
            var conversation = await _context.Conversation.FindAsync(id);

            if (conversation == null)
            {
                return NotFound();
            }

            return new ConversationReturnDTO(conversation);
        }

        // PUT: api/Conversations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConversation(long id, ConversationPostDTO conversationPostDTO)
        {
            var conversation = _context.Conversation.Find(id);
            if (conversation == null)
                return NotFound();
            if (_context.Boards.Find(conversationPostDTO.BoardId) != null)
                conversation.BoardId = conversationPostDTO.BoardId;
            conversation.Name = conversationPostDTO.Name;
            conversation.Description = conversationPostDTO.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Conversations
        [HttpPost]
        public async Task<ActionResult<Conversation>> PostConversation(ConversationPostDTO conversationPostDTO)
        {
            var board = _context.Boards.Find(conversationPostDTO.BoardId);
            if (board == null)
                return NotFound();
            User user = ((User)HttpContext.Items["User"]!);
            var conversation = new Conversation
            {
                BoardId = conversationPostDTO.BoardId,
                Name = conversationPostDTO.Name,
                Description = conversationPostDTO.Description,
                OwnerID = user.Id,
                Owner = user,
            };
            board.Conversations.Add(conversation);
            //_context.Conversation.Add(conversation);
            await _context.SaveChangesAsync();
            var conversationReturnDTO = new ConversationReturnDTO(conversation);
            return CreatedAtAction("GetConversation", new { id = conversation.Id }, conversationReturnDTO);
        }

        // DELETE: api/Conversations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConversation(long id)
        {
            var conversation = await _context.Conversation.FindAsync(id);
            User user = (User)HttpContext.Items["User"]!;
            if (conversation == null)
            {
                return NotFound();
            }
            if (user.Id == conversation.OwnerID || user.isAdmin)
            {
                _context.Conversation.Remove(conversation);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            return Unauthorized();
        }

        private bool ConversationExists(long id)
        {
            return _context.Conversation.Any(e => e.Id == id);
        }
    }
}
