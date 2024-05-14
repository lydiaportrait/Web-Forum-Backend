using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class User
    {
        public long Id { get; set; }
        public bool isAdmin { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<Board> Boards { get; } = new List<Board>();
        public ICollection<Conversation> Conversations { get; } = new List<Conversation>();
        public ICollection<Post> Posts { get; } = new List<Post>();
        public DateTime DateCreated { get; set; }

        public void Update(UserCreationDTO dto)
        {
            Name = dto.Name;
            Email = dto.Email;
            Description = dto.Description;
        }
        public void Update(UserUpdateDTO dto)
        {
            Name = dto.Name;
            Email = dto.Email;
            Description = dto.Description;
        }
        public User(UserCreationDTO dto)
        {
            Name = dto.Name;
            Email = dto.Email;
            Description = dto.Description;
        }

        public User()
        {
        }

        public void UpdatePassword(IPasswordHasher<User> hasher, string password)
        {
            PasswordHash = hasher.HashPassword(this, password);
        }
        public PasswordVerificationResult VerifyPassword(IPasswordHasher<User> hasher, string password)
        {
            return hasher.VerifyHashedPassword(this, PasswordHash, password);
        }
    }
}
