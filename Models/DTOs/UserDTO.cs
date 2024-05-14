using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class UserDTO
    {
        public long Id { get; set; }
        public bool isAdmin { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }

        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Description = user.Description;
            DateCreated = user.DateCreated;
        }
        
        public UserDTO() { }
    }
}
