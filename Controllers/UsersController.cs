using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using portrait_forum.Helpers;
using portrait_forum.Models;

namespace portrait_forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ForumContext _context;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IConfiguration _configuration;

        public UsersController(ForumContext context, IPasswordHasher<User> hasher, IConfiguration configuration)
        {
            _context = context;
            _hasher = hasher;
            _configuration = configuration;
        }
        
        // GET: api/Users/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserLoginResponseDTO>> Login(UserLoginDTO userLoginDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Name == userLoginDTO.Name);
            if (user == null)
                return Unauthorized();
            if (user.VerifyPassword(_hasher, userLoginDTO.Password) == PasswordVerificationResult.Failed)
                return Unauthorized();
            var token = await generateJwtToken(user);
            return new UserLoginResponseDTO(user,token);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(x => UserToDTO(x)).ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return UserToDTO(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, UserUpdateDTO userUpdateDTO)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();
            if (user.VerifyPassword(_hasher, userUpdateDTO.OldPassword) == PasswordVerificationResult.Failed)
                return Forbid();

            user.Update(userUpdateDTO);
            user.UpdatePassword(_hasher, userUpdateDTO.NewPassword);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }

            return NoContent();
        }

        //PATCH: api/Users/5/Adminify
        [HttpPatch("{id}/Adminify")]
        [Authorize]
        public async Task<IActionResult> Adminify(long id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();
            user.isAdmin = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserCreationDTO userCreationDTO)
        {
            User user = new User(userCreationDTO);
            user.UpdatePassword(_hasher, userCreationDTO.Password);
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                return Conflict();
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, UserToDTO(user));
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private async Task<string> generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {
                var secret = _configuration["Secret"];
                if (String.IsNullOrEmpty(secret))
                    throw new Exception("set a secret, silly");
                var key = Encoding.ASCII.GetBytes(secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });
            return tokenHandler.WriteToken(token);
        }
        
        private static UserDTO UserToDTO(User user) =>
        new UserDTO
        {
            Id = user.Id,
            isAdmin = user.isAdmin,
            Name = user.Name,
            Description = user.Description,
            DateCreated = user.DateCreated,
        };
    }
}
