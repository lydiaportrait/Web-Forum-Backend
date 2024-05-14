using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class UserLoginResponseDTO
    {
        public long Id { get; set; }
        public bool isAdmin { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        
        public UserLoginResponseDTO(User user, string token)
        {
            Id = user.Id;
            isAdmin = user.isAdmin;
            Name = user.Name;
            Token = token;
        }
    }
}
