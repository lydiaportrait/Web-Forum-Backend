using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using portrait_forum.Helpers;
using portrait_forum.Models;
using System.Collections;

namespace portrait_forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [portrait_forum.Helpers.Authorize]
    public class BoardsController : ControllerBase
    {
        private readonly ForumContext _context;

        public BoardsController(ForumContext context)
        {
            _context = context;
        }

        [HttpGet("{id}/Conversations")]
        public async Task<ActionResult<IEnumerable<ConversationReturnDTO>>> GetConversations(long id)
        {
            if (!await _context.Boards.AnyAsync(e => e.Id == id))
                return NotFound();
            var conversations = await _context.Conversation
                .Select(e => new ConversationReturnDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    BoardId = e.BoardId,
                    Description = e.Description,
                    DateCreated = e.DateCreated,
                    Owner = new UserDTO(e.Owner)
                })
                .Where(e => e.BoardId == id).ToListAsync();
            return conversations;
        }

        // GET: api/Boards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardDTO>>> GetBoards()
        {
            return await _context.Boards.Select(e => new BoardDTO
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                DateCreated = e.DateCreated,
                Owner = new UserDTO(e.Owner)
            }).ToListAsync();
        }

        // GET: api/Boards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDTO>> GetBoard(long id)
        {
            var board = await _context.Boards.FindAsync(id);

            if (board == null)
            {
                return NotFound();
            }

            return new BoardDTO(board);
        }

        // PUT: api/Boards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoard(long id, BoardPostDTO boardPostDTO)
        {
            User user = (User)HttpContext.Items["User"]!;
            if (!user.isAdmin)
                return Forbid();
            var board = _context.Boards.Find(id);
            if(board == null)
                return NotFound();
            board.Name = boardPostDTO.Name;
            board.Description = boardPostDTO.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Boards
        [HttpPost]
        public async Task<ActionResult<Board>> PostBoard(BoardPostDTO boardPostDTO)
        {
            User user = (User)HttpContext.Items["User"]!;
            if (!user.isAdmin)
                return Forbid();
            var board = new Board
            {
                Name = boardPostDTO.Name,
                Description = boardPostDTO.Description,
                Owner = user
            };
            _context.Boards.Add(board);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoard", new { id = board.Id }, board);
        }

        // DELETE: api/Boards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(long id)
        {
            var board = await _context.Boards.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BoardExists(long id)
        {
            return _context.Boards.Any(e => e.Id == id);
        }
    }
}
